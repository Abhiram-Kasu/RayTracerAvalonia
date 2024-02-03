using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracerAvalonia.RayTracing.Shapes;
public struct Plane : IShape
{
    private readonly Vector3 normal;
    private readonly float distance;
    public IMaterial Appearance { get; }

    public Plane(Vector3 normal, float distance, IMaterial texture)
    {
        Appearance = texture;
        this.normal = normal;
        this.distance = distance;
    }

    private readonly List<float> _emptyList = [];
    public readonly List<float> FindIntersections(Ray ray)
    {
        var angle = Vector3.Dot(ray.Direction, normal);

        // If the dot-product is zero, the ray is perpendicular to the plane's normal,
        // therefore the ray is parallel to the plane and will never intersect.
        if (Math.Abs(angle) < float.Epsilon)
            return _emptyList;

        var b = Vector3.Dot(normal, ray.Start + normal * -distance);
        return [-b / angle];
    }

    public readonly Vector3 GetNormalAt(Vector3 _) => normal;

    public readonly List<float> Intersect(Ray ray) => FindIntersections(ray);
}
