using RayTracerAvalonia.RayTracing.Shapes;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RayTracerAvalonia.RayTracing;
public class Scene(IMaterial background, Camera camera, List<IShape> shapes, List<Light> lights)
{
    public IMaterial BackgroundMaterial { get; set; } = background;
    public Camera Camera { get; set; } = camera;

    public List<IShape> Shapes { get; set; } = shapes;
    public List<Light> Lights { get; set; } = lights;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color Trace(float x, float y) => Camera.Trace(this, x, y);
}