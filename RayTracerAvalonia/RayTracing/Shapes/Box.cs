using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RayTracerAvalonia.RayTracing.Shapes;
public readonly struct Box : IShape
{

    private readonly Vector3 lowerCorner;
    private readonly Vector3 upperCorner;
    private readonly List<Vector3> vertices;



    public IMaterial Appearance { get; }

    public Box(Vector3 corner1, Vector3 corner2, IMaterial texture)
    {
        Appearance = texture;
        lowerCorner = new Vector3(
            Math.Min(corner1.X, corner2.X),
            Math.Min(corner1.Y, corner2.Y),
            Math.Min(corner1.Z, corner2.Z));

        upperCorner = new Vector3(
            Math.Max(corner1.X, corner2.X),
            Math.Max(corner1.Y, corner2.Y),
            Math.Max(corner1.Z, corner2.Z));

        vertices = [lowerCorner, upperCorner];
    }

    private bool Contains(Vector3 point, int axis) => lowerCorner[axis] < point[axis] && point[axis] < upperCorner[axis];

    private static readonly int[] _zeroAxes = [ 1, 2 ];
    private static readonly int[] _oneAxes = [0, 2 ];
    private static readonly int[] _twoAxes = [ 0, 1 ];
    private static readonly List<float> Empty = [];
    private List<float> FindIntersectionsOnAxis(int axis, Ray ray, List<float> intersections)
    {
        var otherAxes = axis switch
        {
            0 => _zeroAxes,
            1 => _oneAxes,
            2 => _twoAxes,
        };
        

        if (Math.Abs(ray.Direction[axis]) < float.Epsilon)
            return Empty;



        foreach (var vertex in vertices)
        {
            var intersect = (vertex[axis] - ray.Start[axis]) / ray.Direction[axis];
            var intersectionPoint = ray.Start + ray.Direction * intersect;

            if (Contains(intersectionPoint, otherAxes[0]) && Contains(intersectionPoint, otherAxes[1]))
                intersections.Add(intersect);
        }

        return intersections;
    }

    public List<float> FindIntersections(Ray ray)
    {
        var intersections = new List<float>();

        for (var axis = 0; axis < 3; axis++)
        {
            FindIntersectionsOnAxis(axis, ray, intersections);

        }

        return intersections;
    }

    public Vector3 GetNormalAt(Vector3 pos)
    {
        const float threshold = 0.0001f;

        if (Math.Abs(lowerCorner.X - pos.X) < threshold)
            return Vector3.UnitX * -1;

        if (Math.Abs(upperCorner.X - pos.X) < threshold)
            return Vector3.UnitX;

        if (Math.Abs(lowerCorner.Y - pos.Y) < threshold)
            return Vector3.UnitY * -1;

        if (Math.Abs(upperCorner.Y - pos.Y) < threshold)
            return Vector3.UnitY;

        if (Math.Abs(lowerCorner.Z - pos.Z) < threshold)
            return Vector3.UnitZ * -1;

        if (Math.Abs(upperCorner.Z - pos.Z) < threshold)
            return Vector3.UnitZ;

        throw new Exception($"The point {pos.ToString()} is not on the surface of {ToString()}");
    }

    public override string ToString() => $"box({lowerCorner.ToString()}, {upperCorner.ToString()})";

    public List<float> Intersect(Ray ray) => FindIntersections(ray);
}


