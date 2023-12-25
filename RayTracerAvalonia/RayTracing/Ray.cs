using RayTracerAvalonia.RayTracing.Extensions;
using System;
using System.Linq;
using System.Numerics;

namespace RayTracerAvalonia.RayTracing;
public record class Ray
{
    public Vector3 Start { get; }
    public Vector3 Direction { get; }

    public Ray(Vector3 start, Vector3 direction)
    {
        Start = start;
        Direction = direction.Normalize();
    }
    public Color Trace(Scene scene)
    {

        var ray = this;
        var distances = scene.Shapes.Select(shape => (Shape: shape, Distance: shape.ClosestDistanceAlongRay(ray))).Where(x => x.Distance is not null).ToList();
        if (!distances.Any())
        {

            return scene.BackgroundMaterial.GetColorAt(default);
        }

        var closest = distances.MinBy(x => x.Distance);
        //var closestShape = scene.Shapes[distances.IndexOf(shortestDistance)];


        var point = Start + (Direction * closest.Distance!.Value);
        return closest.Shape.GetColorAt(point, scene);


    }
}
