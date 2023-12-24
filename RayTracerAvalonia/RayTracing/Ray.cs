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
    public Color Trace(in Scene scene)
    {

        var ray = this;
        var distances = scene.Shapes.Select(shape => (shape, shape.ClosestDistanceAlongRay(ray))).Where(x => x.Item2 is not null).ToList();
        if (!distances.Any())
        {

            return scene.BackgroundColor;
        }

        var closestShape = distances.MinBy(x => x.Item2).Item1;
        //var closestShape = scene.Shapes[distances.IndexOf(shortestDistance)];
        return closestShape.Color;


    }
}
