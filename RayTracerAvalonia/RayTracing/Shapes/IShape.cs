using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RayTracerAvalonia.RayTracing.Shapes;
public interface IShape
{
    public Color Color { get; }
    public List<double> Intersect(Ray ray);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double? ClosestDistanceAlongRay(Ray ray)
    {
        var distances = Intersect(ray);
        if (!distances.Any()) return null;
        var shortestDistance = distances.Min();
        return shortestDistance;
    }
}