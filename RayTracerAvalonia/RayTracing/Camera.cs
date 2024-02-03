using RayTracerAvalonia.RayTracing.Extensions;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayTracerAvalonia.RayTracing;

public struct Camera
{
    public Vector3 Location { get; set; }
    public Vector3 LookAt { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public Vector3 Direction { get; set; }
    public Vector3 Right { get; set; }
    public Vector3 Up { get; set; }

    public Camera(Vector3 location, Vector3 lookAt, float width = 4, float height = 9F / 4F)
    {
        Location = location;
        LookAt = lookAt;
        Width = width;
        Height = height;

        Direction = (lookAt - location).Normalize();
        Right = Vector3.Cross(Vector3.UnitY, Direction).Normalize() * (width / 2);
        Up = Vector3.Cross(Right, Direction).Normalize() * (-height / 2);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color Trace(Scene scene, float x, float y)
    {
        var xRay = Right * x;
        var yRay = -Up * y;
        var rayDir = Direction + xRay + yRay;
        var ray = new Ray(Location, rayDir);

        return ray.Trace(scene);

    }
}


