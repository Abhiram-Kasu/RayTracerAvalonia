using System;
using System.Runtime.InteropServices;

namespace RayTracerAvalonia.RayTracing.Cpp;



public static unsafe class CppInterop
{

    [DllImport("libcpp_raytracing_engine", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetCamera(nint scene, float x, float y, float z);


    [DllImport("libcpp_raytracing_engine", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint GetScene();
    [DllImport("libcpp_raytracing_engine", CallingConvention = CallingConvention.Cdecl)]

    public static extern void FreeScene(nint scene);
    [DllImport("libcpp_raytracing_engine", CallingConvention = CallingConvention.Cdecl)]

    public static extern nint CreateRenderer(UInt16 width, UInt16 height);
    [DllImport("libcpp_raytracing_engine", CallingConvention = CallingConvention.Cdecl)]

    public static extern void FreeRenderer(nint renderer);
    [DllImport("libcpp_raytracing_engine", CallingConvention = CallingConvention.Cdecl)]

    public static extern void Raytrace(byte* data, nint renderer, nint scene);

}