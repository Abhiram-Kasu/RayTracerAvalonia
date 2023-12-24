using System;
using System.Numerics;

namespace RayTracerAvalonia.RayTracing.Extensions;
public static class VectorExtensions
{
    public readonly record struct VectorPathContainer(Func<Vector3, Vector3> To);
    public static VectorPathContainer From(Vector3 origin) => new(destination => destination - origin);

    public static Vector3 Normalize(this Vector3 vec) => Vector3.Normalize(vec);
}
