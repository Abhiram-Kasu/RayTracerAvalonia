using System.Numerics;

namespace RayTracerAvalonia.RayTracing;
public interface Material
{
    public Color GetColorAt(Vector3 point);
}
