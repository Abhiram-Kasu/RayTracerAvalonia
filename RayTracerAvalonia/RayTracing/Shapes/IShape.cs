using System.Buffers;
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
    public static bool CastsShadowFor<T>(T instance, Vector3 point, Vector3 lightVector) where T : IShape
    {
        var distanceToLight = lightVector.Length();
        var ray = new Ray(point, lightVector);
        if (ClosestDistanceAlongRay(instance, ray) is { } num)
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


        var otherShapes = ArrayPool<IShape>.Shared.Rent(scene.Shapes.Count - 1);
        var index = -1;
        
        foreach (var shape in scene.Shapes)
        {
            if (shape != this)
            {
                otherShapes[++index] = shape;
                
            }
        }

        foreach (var light in scene.Lights)
        {
            var v = light.Position - point;
            var brightness = Vector3.Dot(normal, v.Normalize());

            var castsShadow = false;
            for(var i = 0; i <= index; i++)
            {
                
                if(CastsShadowFor(otherShapes[i], light.Position, -v))
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
        ArrayPool<IShape>.Shared.Return(otherShapes);
        return color;
        
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float? ClosestDistanceAlongRay<T>(T instance, Ray ray) where T : IShape
    {
        var distances = instance.Intersect(ray);
        if (distances.Count == 0) return null;
        var shortestDistance = distances.Min();
        return shortestDistance;
    }
}