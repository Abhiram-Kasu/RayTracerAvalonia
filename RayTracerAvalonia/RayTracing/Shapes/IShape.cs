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

        var otherShapes = new List<IShape>(scene.Shapes.Count-1);
        
        foreach (var shape in scene.Shapes)
        {
            if (shape != this)
            {
                otherShapes.Add(shape);
            }
        }

        foreach (var light in scene.Lights)
        {
            var v = light.Position - point;
            var brightness = Vector3.Dot(normal, v.Normalize());

            var castsShadow = false;
            foreach (var shape in otherShapes)
            {
                if (shape.CastsShadowFor(light.Position, -v))
                {
                    castsShadow = true;
                    break;
                }
            }

            if (castsShadow) continue;

            if (brightness <= 0) continue;
            var illumination = light.Illuminate(Appearance, point, brightness);
            color += illumination;
        }
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