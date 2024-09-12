using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RayTracerAvalonia.RayTracing;
using RayTracerAvalonia.RayTracing.Examples;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RayTracerAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    private WriteableBitmap? _bitmap;
    private WriteableBitmap Bitmap => _bitmap ??= new WriteableBitmap(new PixelSize(_width, _height), new Avalonia.Vector(96, 96), PixelFormat.Bgra8888);
    private const int _height = 720;
    private const int _width = 1280;
    private byte[] pixels = new byte[_width * _height * 4];

    private readonly Renderer _renderer = new(_width, _height);

    private Scene _currentScene = ExampleScenes.AssortedShapes();

    public MainViewModel() => (_cameraX, _cameraY, _cameraZ) = (_currentScene.Camera.Location.X, _currentScene.Camera.Location.Y, _currentScene.Camera.Location.Z);
    

    private float _cameraX;
    public float CameraX
    {
        get => _cameraX;
        set
        {
            _cameraX = value;
            OnPropertyChanged();
            RenderParalellCommand.Execute(null);
        }
    }

    private float _cameraY;
    public float CameraY
    {
        get => _cameraY;
        set
        {
            _cameraY = value;
            OnPropertyChanged();
            RenderParalellCommand.Execute(null);
        }
    }

    private float _cameraZ;
    public float CameraZ
    {
        get => _cameraZ;
        set
        {
            _cameraZ = value;
            OnPropertyChanged();
            RenderParalellCommand.Execute(null);
        }
    }

    [ObservableProperty]
    private string _elapsed;

    [ObservableProperty]
    private IImage? _imageSource;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy = false;
    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    public async Task RenderParalell()
    {
        if (IsBusy) return;
        IsBusy = true;

        _currentScene.Camera = _currentScene.Camera with { Location = new(CameraX, CameraY, CameraZ) };

        // _currentScene = new Scene(RayTracing.Color.Cyan, new Camera(new System.Numerics.Vector3(0, 0, -8), Vector3.UnitZ), []);

        var st = Stopwatch.StartNew();

        await Task.Run(() => _renderer.Render(ref pixels, _currentScene));

        //Parallel.For(0, _height, h =>
        //{
        //    for (int w = 0; w < _width; w++)
        //    {
        //        var color = _currentScene.Trace((w / _width) - .5f, (h / _height) - .5f);
        //        var index = (h * _width + w) * 4;
        //        pixels[index] = color.B;
        //        pixels[index + 2] = color.G;
        //        pixels[index + 1] = color.R;
        //        pixels[index + 3] = 255;
        //    }
        //});

        SetImage();
        Elapsed = $"Elapsed: {st.ElapsedMilliseconds}";
        
        _renderTimes.Add(st.ElapsedMilliseconds);

        MinTimeToRender = MathF.Min(MinTimeToRender, st.ElapsedMilliseconds);
        AverageTimeToRender = _renderTimes.Sum() / _renderTimes.Count;
        IsBusy = false;


    }


    private unsafe void SetImage()
    {

        ImageSource = null;
        using (var fb = Bitmap.Lock())
        {

            fixed (byte* ptr = pixels)
            {

                Unsafe.CopyBlock((byte*)fb.Address, ptr, (uint)pixels.Length);
            }
        }
        ImageSource = Bitmap;
        //this.OnPropertyChanged(nameof(ImageSource));

    }

    [ObservableProperty]
    private float _averageTimeToRender;
    [ObservableProperty, NotifyPropertyChangedFor(nameof(FormattedMinTimeToRender))]
    private float _minTimeToRender = float.MaxValue;
    
    public string FormattedMinTimeToRender => $"Min: {MinTimeToRender}ms";

    private readonly List<float> _renderTimes = [];
}
