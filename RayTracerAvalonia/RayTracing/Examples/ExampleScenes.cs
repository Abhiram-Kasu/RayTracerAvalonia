using RayTracerAvalonia.RayTracing.Shapes;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracerAvalonia.RayTracing.Examples;
public static class ExampleScenes
{
    public static Scene ColoredSpheres()
    {
        var camera = new Camera(new(0, 2, -8), Vector3.UnitZ);
        var backgroundColor = Color.Black;
        List<IShape> shapes = [
            new Sphere(Color.Yellow, new(-4, 0, 4), 1, new(Color.White)),
            new Sphere(Color.Red, new(-2, 0, 2), 1, new(Color.Red)),
            new Sphere(Color.White, new(+0, 0, 0), 1, new(Color.White)),
            new Sphere(Color.Green, new(+2, 0, 2), 1, new(Color.Green)),
            new Sphere(Color.Blue, new(+4, 0, 4), 1, new(Color.Blue)),
        ];
        return new Scene(backgroundColor, camera, shapes, [new(new(5, 10, -5), Color.White)]);
    }
}