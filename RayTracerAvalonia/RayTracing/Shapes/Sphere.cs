using RayTracerAvalonia.RayTracing.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayTracerAvalonia.RayTracing.Shapes;
public readonly struct Sphere(Vector3 center, float radius, IMaterial appearance) : IShape
{





    public Vector3 Center { get; } = center;
    public float Radius { get; } = radius;

    public IMaterial Appearance => appearance;

    public Vector3 GetNormalAt(Vector3 point)
    {
        // Calculate the normal vector at a point on the sphere
        var normal = VectorExtensions.From(Center).To(point);
        return normal.Normalize();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<float> Intersect(Ray ray)
    {
        var os = VectorExtensions.From(Center).To(ray.Start);
        var b = 2 * Vector3.Dot(os, ray.Direction);
        var c = os.LengthSquared() - Radius * Radius;
        var disc = b * b - 4 * c;
        switch (disc)
        {
            case < 0:
                return [];
            case 0:
                return [-b / 2];
            default:
                {
                    var root = Math.Sqrt(disc);
                    return [(float)((-b - root) / 2f), (float)((-b + root) / 2f)];
                }
        }
    }


}