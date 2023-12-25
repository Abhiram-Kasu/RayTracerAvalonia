using System.Numerics;

namespace RayTracerAvalonia.RayTracing;
public struct Light(Vector3 Position, Color Color)
{
    public Vector3 Position { get; set; } = Position;
    public Color Color { get; set; } = Color;
    public Color Illuminate(IMaterial material, Vector3 point, float brightness) => material.GetColorAt(point) * Color * brightness;
}
