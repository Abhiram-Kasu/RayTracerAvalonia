using System;
using System.Buffers;
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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using RayTracerAvalonia.RayTracing.Cpp;

namespace RayTracerAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase, IDisposable
{
    public string Greeting => "Welcome to Avalonia!";

    private WriteableBitmap? _bitmap;

    private WriteableBitmap Bitmap => _bitmap ??= new WriteableBitmap(new PixelSize(_width, _height),
        new Avalonia.Vector(96, 96), PixelFormat.Bgra8888);

    private const int _height = 720*4;
    private const int _width = 1280*4;
    private volatile byte[] pixels;
    private unsafe byte* pixels_ptr;

    private readonly Renderer _renderer = new(_width, _height);

    private nint _cppRenderer = CppInterop.CreateRenderer(_width, _height);
    private nint _cppScene = CppInterop.GetScene();

    private Scene _currentScene = ExampleScenes.AssortedShapes();

    public MainViewModel()
    {
        (_cameraX, _cameraY, _cameraZ) = (_currentScene.Camera.Location.X,
            _currentScene.Camera.Location.Y, _currentScene.Camera.Location.Z);
        pixels = GC.AllocateArray<byte>(_width * _height * 4,true);
        unsafe
        {
            fixed (byte* ptr = &pixels[0])
            {
                pixels_ptr = ptr;
            }
        }
    }


    private float _cameraX;

    public float CameraX
    {
        get => _cameraX;
        set
        {
            _cameraX = value;
            OnPropertyChanged();
            if(lastRenderWasCpp)
                RenderNativeCommand.Execute(null);
            else
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
            if(lastRenderWasCpp)
                RenderNativeCommand.Execute(null);
            else
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
            if(lastRenderWasCpp)
                RenderNativeCommand.Execute(null);
            else
                RenderParalellCommand.Execute(null);
        }
    }

    [ObservableProperty] private string _elapsed;

    [ObservableProperty] private IImage? _imageSource;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy = false;

    public bool IsNotBusy => !IsBusy;
    
    public bool lastRenderWasCpp = false;

    [RelayCommand]
    public async Task RenderNative()
    {
        if (IsBusy) return;
        IsBusy = true;
        lastRenderWasCpp = true;
        
        CppInterop.SetCamera(_cppScene, CameraX, CameraY, CameraZ);
        
        var st = Stopwatch.StartNew();
        
        await Task.Run( () =>
        {
            unsafe
            {
                CppInterop.Raytrace(pixels_ptr, _cppRenderer, _cppScene);
            }
        });
        
        SetImage();
        Elapsed = $"Elapsed: {st.ElapsedMilliseconds}";

        _renderTimes.Add(st.ElapsedMilliseconds);

        MinTimeToRender = MathF.Min(MinTimeToRender, st.ElapsedMilliseconds);
        AverageTimeToRender = _renderTimes.Sum() / _renderTimes.Count;
        IsBusy = false;
    }

    [RelayCommand]
    public async Task RenderParalell()
    {
        if (IsBusy) return;
        IsBusy = true;
        lastRenderWasCpp = false;

        _currentScene.Camera = _currentScene.Camera with { Location = new(CameraX, CameraY, CameraZ) };


        var st = Stopwatch.StartNew();

        await Task.Run(() => _renderer.Render(ref pixels, _currentScene));


            

        
                

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

    [ObservableProperty] private float _averageTimeToRender;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(FormattedMinTimeToRender))]
    private float _minTimeToRender = float.MaxValue;

    public string FormattedMinTimeToRender => $"Min: {MinTimeToRender}ms";

    private readonly List<float> _renderTimes = [];


    public void Dispose()
    {
        _bitmap?.Dispose();
        CppInterop.FreeRenderer(_cppRenderer);
        CppInterop.FreeScene(_cppScene);
    }
}