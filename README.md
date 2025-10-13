# RayTracerAvalonia

A high-performance ray tracer built with Avalonia UI, showcasing the power of C# optimization and C++/C# interop for maximum performance. This project demonstrates how to build a real-time ray tracing engine with both pure C# and native C++ implementations.

![Ray Tracer Demo](assets/demo.png)

## Overview

RayTracerAvalonia is a cross-platform ray tracing application that renders 3D scenes in real-time. The project started as a pure C# implementation and evolved to include a high-performance C++ rendering engine, demonstrating the power of native interop when maximum performance is required.

### Key Features

- **Dual Rendering Engines**: Switch between C# and C++ implementations with a single button click
- **Real-Time Performance**: Optimized for speed with parallel processing and aggressive optimizations
- **Interactive Camera Controls**: Adjust camera position (X, Y, Z) and see results immediately
- **Cross-Platform**: Built with Avalonia UI, runs on Windows, macOS, and Linux
- **Performance Metrics**: Real-time display of render times, including average and minimum times

## Architecture

### C# Ray Tracer

The pure C# implementation leverages several optimization techniques:

- **Parallel Rendering**: Uses `Parallel.For` to distribute pixel calculations across multiple CPU cores
- **Struct-Based Design**: Utilizes value types (`struct`) for `Camera`, `Ray`, `Color`, etc. to minimize heap allocations
- **Aggressive Inlining**: Strategic use of `[MethodImpl(MethodImplOptions.AggressiveInlining)]` for hot paths
- **SIMD-Ready**: Uses `System.Numerics.Vector3` for efficient vector mathematics

**Core Components:**
```
RayTracerAvalonia/RayTracing/
├── Renderer.cs          # Parallel C# renderer
├── Camera.cs            # Camera with ray generation
├── Ray.cs               # Ray-scene intersection logic
├── Scene.cs             # Scene container
├── Shapes/              # Geometric primitives (Sphere, Box, Plane)
├── Light.cs             # Lighting calculations
└── Color.cs             # Color representation
```

### C++ Ray Tracing Engine

When pure C# performance wasn't enough, the project was extended with a native C++ engine that provides significant speedup through:

- **OpenMP Parallelization**: Multi-threaded rendering with `#pragma omp parallel for`
- **Eigen Library**: High-performance linear algebra with SIMD optimizations
- **C++23 Features**: Modern C++ for better performance and expressiveness
- **Aggressive Compiler Optimizations**: Built with `-O3 -ffast-math` flags

**C++ Engine Structure:**
```
RayTracerAvalonia.Desktop/cpp_raytracing_engine/
├── include/
│   ├── renderer.hpp     # C++ renderer interface
│   ├── camera.hpp       # Camera implementation
│   ├── ray.hpp          # Ray tracing logic
│   ├── scene.hpp        # Scene structure
│   ├── shape.hpp        # Shape base class
│   └── ...
├── engine.cpp           # C API exports
├── renderer.cpp         # Rendering implementation
└── CMakeLists.txt       # Build configuration
```

## C++/C# Interop

The project demonstrates seamless interop between C# and C++ using P/Invoke (Platform Invoke). This allows the C# application to call native C++ functions with minimal overhead.

### How It Works

#### 1. C++ Exports C API
The C++ library exports a C-style API that's compatible with P/Invoke:

```cpp
extern "C" {
    EXPORT void* CreateRenderer(uint16_t width, uint16_t height);
    EXPORT void* GetScene();
    EXPORT void SetCamera(void* scene, float x, float y, float z);
    EXPORT void Raytrace(uint8_t* data, void* renderer, void* scene);
    EXPORT void FreeRenderer(void* renderer);
    EXPORT void FreeScene(void* scene);
}
```

#### 2. C# Declares Imports
The C# code uses `DllImport` to declare the native functions:

```csharp
public static unsafe class CppInterop
{
    [DllImport("libcpp_raytracing_engine", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint CreateRenderer(UInt16 width, UInt16 height);

    [DllImport("libcpp_raytracing_engine", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Raytrace(byte* data, nint renderer, nint scene);
    
    // ... other imports
}
```

#### 3. Shared Memory for Image Data
Instead of copying pixel data back and forth, both engines write directly to a shared byte array:

```csharp
// C# allocates pinned memory
pixels = GC.AllocateArray<byte>(_width * _height * 4, true);
unsafe {
    fixed (byte* ptr = &pixels[0]) {
        pixels_ptr = ptr;
        // Pass pointer to C++
        CppInterop.Raytrace(pixels_ptr, _cppRenderer, _cppScene);
    }
}
```

This approach provides:
- **Zero-Copy Performance**: Image data is written once, directly to display memory
- **Type Safety**: C# manages memory lifecycle
- **Native Speed**: C++ operates on raw pointers for maximum performance

### Performance Comparison

The C++ implementation typically achieves **2-5x faster** render times compared to the optimized C# version:

| Implementation | Typical Render Time (1280x720) |
|---------------|-------------------------------|
| C# (Parallel) | ~80-150ms                     |
| C++ (OpenMP)  | ~25-50ms                      |

*Performance varies based on scene complexity and CPU capabilities*

## Building and Running

### Prerequisites

- .NET 8.0 SDK
- CMake 3.15 or higher
- C++ compiler with C++23 support (Clang recommended)
- Eigen3 library
- OpenMP support

### macOS Build

```bash
cd RayTracerAvalonia.Desktop

# Build and run everything
./run.sh
```

The `run.sh` script:
1. Publishes the .NET application
2. Builds the C++ library with CMake
3. Copies the native library to the publish directory
4. Launches the application

### Manual Build

#### C++ Library
```bash
cd RayTracerAvalonia.Desktop/cpp_raytracing_engine
mkdir -p build && cd build
cmake ..
make
# Copy resulting libcpp_raytracing_engine.dylib (macOS) or .so (Linux) to publish folder
```

#### .NET Application
```bash
dotnet publish -c Release
cd bin/Release/net8.0/[runtime]/publish
./RayTracerAvalonia.Desktop
```

## Usage

1. **Launch the application** - A window will open showing the ray-traced scene
2. **Choose Rendering Engine**:
   - Click "Render C#" for pure C# implementation
   - Click "Render C++" for native C++ engine
3. **Adjust Camera**: Use the X, Y, Z numeric controls to move the camera position
4. **Monitor Performance**: Watch real-time render times displayed in the UI

## Current Scene

The default scene includes:
- Multiple colored spheres (Magenta, Yellow, Green, Blue, Cyan)
- A red box
- A white ground plane
- Dynamic lighting with shadow casting

## Future Plans

### Reflections
Adding realistic reflections to make surfaces mirror their environment:
- Recursive ray tracing for mirror-like surfaces
- Configurable reflection depth limit
- Fresnel effects for realistic reflection intensity

### Advanced Materials
Implementing a proper material system:
- **Diffuse Materials**: Matte surfaces with Lambertian reflection
- **Specular Materials**: Shiny surfaces with controllable shininess
- **Metallic Materials**: Realistic metal appearance
- **Dielectric Materials**: Glass, water, and other transparent materials
- **Texture Mapping**: Apply images to surfaces
- **Normal Mapping**: Add surface detail without geometry

### Additional Features
- Refraction for transparent objects
- Area lights for soft shadows
- Ambient occlusion for better depth perception
- Anti-aliasing for smoother edges

## Project Structure

```
RayTracerAvalonia/
├── RayTracerAvalonia/              # Core C# ray tracing library
│   ├── RayTracing/                 # Ray tracing engine
│   ├── ViewModels/                 # Avalonia ViewModels
│   └── Views/                      # Avalonia UI Views
├── RayTracerAvalonia.Desktop/      # Desktop application
│   ├── cpp_raytracing_engine/      # Native C++ engine
│   └── run.sh                      # Build and run script
└── RayTracerAvalonia.Browser/      # ⚠️ DEPRECATED - Not maintained
```

**Note:** The `RayTracerAvalonia.Browser` folder contains an experimental WebAssembly port that is no longer maintained. Focus has shifted to the high-performance desktop implementation with native C++ acceleration.

## Technical Deep Dive

### Why C# Wasn't Enough

The initial C# implementation was already quite optimized:
- Parallel processing across all CPU cores
- Value types to avoid GC pressure
- Aggressive inlining of hot paths
- Efficient vector math with System.Numerics

However, when pushing for real-time performance at high resolutions (5K+), even optimized C# couldn't quite reach the frame rates needed. The overhead of:
- Managed runtime bounds checking
- JIT compilation limitations
- GC pauses (even with pinned arrays)

...meant that native code was necessary for the last 2-5x performance improvement.

### Why the C++ Port Was Successful

The C++ implementation achieves superior performance through:

1. **SIMD-Optimized Vector Math**: Eigen library with compiler-level vectorization
2. **Zero Runtime Overhead**: No bounds checking, no GC, direct memory access
3. **Better Optimization**: Clang's `-O3 -ffast-math` enables aggressive optimizations unsafe in managed code
4. **OpenMP**: More efficient parallelization than .NET's Task Parallel Library for this workload

The key was maintaining the same algorithm structure, ensuring the port was straightforward and correct.

## Performance Tips

### For C# Rendering
- Ensure Release build configuration
- Consider NativeAOT for even better startup and sustained performance
- High core count CPUs benefit most from parallel rendering

### For C++ Rendering
- Build with Release configuration (`-O3`)
- Ensure Eigen is found by CMake for SIMD optimizations
- OpenMP thread count scales with CPU cores

## Contributing

Contributions are welcome! Areas of interest:
- Additional shape primitives (cylinders, cones, tori)
- Material system implementation
- BVH (Bounding Volume Hierarchy) for faster ray-scene intersections
- Path tracing for global illumination
- GPU acceleration with CUDA or Vulkan compute shaders

## License

[Add your license here]

## Acknowledgments

- Built with [Avalonia UI](https://avaloniaui.net/) for cross-platform desktop apps
- Uses [Eigen](https://eigen.tuxfamily.org/) for high-performance linear algebra in C++
- Inspired by classic ray tracing literature and modern real-time rendering techniques
