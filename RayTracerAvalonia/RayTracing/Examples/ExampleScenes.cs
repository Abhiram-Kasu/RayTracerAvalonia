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
            new Sphere(new(-4, 0, 4), 1, new Appearance(Color.White)),
            new Sphere(new(-2, 0, 2), 1, new Appearance(Color.Red)),
            new Sphere(new(+0, 0, 0), 1, new Appearance(Color.White)),
            new Sphere(new(+2, 0, 2), 1, new Appearance(Color.Green)),
            new Sphere(new(+4, 0, 4), 1, new Appearance(Color.Blue)),
        ];
        return new Scene(backgroundColor, camera, shapes, [new(new(5, 10, -5), Color.White)]);
    }
    public static Scene AssortedShapes()
    {
        var camera = new Camera(new(-10, 10, -20), new(0, 4, 0));
        var background = Color.Black;
        List<Light> lights = [new(new(-30, 25, -12), Color.White)];

        List<IShape> shapes = [new Shapes.Plane(Vector3.UnitY, 0, new Appearance(Color.White)),
            new Box(new(-2, 0, -2), new(2, 4, 2), new Appearance(Color.Red)),
            new Sphere(new(6, 2, 0), 2, new Appearance(Color.Magenta)),
            new Sphere(new(6, 1, -4), 1, new Appearance(Color.Yellow)),
            new Sphere(new(-2, 2, 4), 2, new Appearance(Color.Green)),
            new Sphere(new(-4, 4, 10), 4, new Appearance(Color.Blue)),
            new Sphere(new(-3.2f, 1, -1), 1, new Appearance(Color.Cyan)),
        ];
        return new Scene(background, camera, shapes, lights);

    }
}