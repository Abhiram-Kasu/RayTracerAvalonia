using RayTracerAvalonia.RayTracing.Shapes;
using System.Collections.Generic;

namespace RayTracerAvalonia.RayTracing;
public struct Scene(Color backgroundColor, Camera camera, List<IShape> shapes)
{
    public Color BackgroundColor { get; set; } = backgroundColor;
    public Camera Camera { get; set; } = camera;

    public List<IShape> Shapes { get; set; } = shapes;

    public Color Trace(float x, float y) => Camera.Trace(this, x, y);
}