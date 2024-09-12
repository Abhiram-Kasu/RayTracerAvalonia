using RayTracerAvalonia.RayTracing.Extensions;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using RayTracerAvalonia.RayTracing.Shapes;

namespace RayTracerAvalonia.RayTracing;
public record struct Ray
{
    public Vector3 Start { get; }
    public Vector3 Direction { get; }

    public Ray(Vector3 start, Vector3 direction)
    {
        Start = start;
        Direction = direction.Normalize();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color Trace(Scene scene)
    {
        float? minDistance = null;
        IShape? closestShape = null;
        Vector3 point = default;

        foreach (var shape in scene.Shapes)
        {
            var distance = IShape.ClosestDistanceAlongRay(shape, this);
            if (distance is null) continue;
            if (minDistance is not null && !(distance < minDistance)) continue;
            minDistance = distance;
            closestShape = shape;
            point = Start + (Direction * minDistance.Value);
        }
        return closestShape?.GetColorAt(point, scene) ?? scene.BackgroundMaterial.GetColorAt(default);
    }
}
