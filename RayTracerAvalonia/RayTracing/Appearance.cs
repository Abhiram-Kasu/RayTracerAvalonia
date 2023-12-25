using System.Numerics;

namespace RayTracerAvalonia.RayTracing;
public struct Appearance : IMaterial
{
    public IMaterial Material { get; set; }
    public Appearance(IMaterial? material = null)
    {
        Material = material ?? Color.Grey;
    }

    public Color GetColorAt(Vector3 point) => Material.GetColorAt(point);
}
