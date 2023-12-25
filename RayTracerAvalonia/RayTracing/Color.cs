using System.Numerics;

namespace RayTracerAvalonia.RayTracing;
public readonly record struct Color(byte R, byte G, byte B, byte Alpha = 255) : IMaterial
{
    public static readonly Color White = new Color(255, 255, 255);

    public static readonly Color Black = new Color(0, 0, 0);

    public static readonly Color Grey = new Color(127, 127, 127);

    public static readonly Color Red = new Color(255, 0, 0);

    public static readonly Color Green = new Color(0, 255, 0);

    public static readonly Color Blue = new Color(0, 0, 255);

    public static readonly Color Yellow = new Color(255, 255, 0);

    public static readonly Color Magenta = new Color(255, 0, 255);

    public static readonly Color Cyan = new Color(0, 255, 255);
    private static byte Clamp(float value) => value switch
    {
        < 0 => 0,
        > 255 => 255,
        _ => (byte)value
    };

    public Color GetColorAt(Vector3 point) => this;

    public static Color operator +(Color c1, Color c2)
    {
        return new Color(
            Clamp(c1.R + c2.R),
            Clamp(c1.G + c2.G),
            Clamp(c1.B + c2.B)
        );
    }

    public static Color operator -(Color c1, Color c2)
    {
        return new Color(
            Clamp(c1.R - c2.R),
            Clamp(c1.G - c2.G),
            Clamp(c1.B - c2.B)
        );
    }

    public static Color operator *(Color c1, Color c2)
    {
        return new Color(
            Clamp(c1.R * c2.R / 255f),
            Clamp(c1.G * c2.G / 255f),
            Clamp(c1.B * c2.B / 255f)
        );
    }

    public static Color operator *(Color c, float s)
    {
        return new Color(
            Clamp(c.R * s),
            Clamp(c.G * s),
            Clamp(c.B * s)
        );

    }
}
