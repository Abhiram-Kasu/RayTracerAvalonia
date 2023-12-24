using Avalonia.Controls;
using RayTracerAvalonia.ViewModels;

namespace RayTracerAvalonia.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        (this.DataContext as MainViewModel)?.RenderParalellCommand.Execute(null);
    }
}
