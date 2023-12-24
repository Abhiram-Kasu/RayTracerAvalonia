using System.Numerics;

namespace RayTracerAvalonia.RayTracing;
public struct Appearance : Material
{
    public Material Material { get; set; }
    public Appearance(Material? material = null)
    {
        Material = material ?? Color.Grey;
    }

    public Color GetColorAt(Vector3 point) => Material.GetColorAt(point);
}
