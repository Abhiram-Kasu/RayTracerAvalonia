using Avalonia.Controls;
using RayTracerAvalonia.ViewModels;
using System;

namespace RayTracerAvalonia.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        
        InitializeIfNeeded();


    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        (this.DataContext as MainViewModel)!.RenderParalellCommand.Execute(null);
    }
}
