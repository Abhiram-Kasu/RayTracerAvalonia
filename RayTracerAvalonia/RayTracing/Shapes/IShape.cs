using RayTracerAvalonia.RayTracing.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayTracerAvalonia.RayTracing.Shapes;
public interface IShape
{

    public IMaterial Appearance { get; }
    public List<float> Intersect(Ray ray);
    public Vector3 GetNormalAt(Vector3 point);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CastsShadowFor(Vector3 point, Vector3 lightVector)
    {
        var distanceToLight = lightVector.Length();
        var ray = new Ray(point, lightVector);
        if (ClosestDistanceAlongRay(ray) is { } num)
        {
            return num <= distanceToLight;
        }
        return false;

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color GetColorAt(Vector3 point, Scene scene)
    {
        var normal = GetNormalAt(point);
        var color = Color.Black;

        var otherShapes = scene.Shapes.Where(x => x != this).ToList();

        scene.Lights.ForEach(light =>
        {
            var v = VectorExtensions.From(point).To(light.Position);
            var brightness = Vector3.Dot(normal, v.Normalize());

            if (otherShapes.Any(x => x.CastsShadowFor(light.Position, -v))) return;

            if (brightness <= 0) return;
            var illumination = light.Illuminate(Appearance, point, brightness);
            color += illumination;
        });
        return color;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float? ClosestDistanceAlongRay(Ray ray)
    {
        var distances = Intersect(ray);
        if (!distances.Any()) return null;
        var shortestDistance = distances.Min();
        return shortestDistance;
    }
}