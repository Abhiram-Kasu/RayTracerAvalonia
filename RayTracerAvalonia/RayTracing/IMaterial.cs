using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayTracerAvalonia.RayTracing;
public interface IMaterial
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color GetColorAt(Vector3 point);
}
