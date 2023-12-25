using System.Numerics;

namespace RayTracerAvalonia.RayTracing;
public interface IMaterial
{
    public Color GetColorAt(Vector3 point);
}
