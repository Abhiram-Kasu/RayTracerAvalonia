#!/bin/bash


dotnet publish

# Step 2: Enter the C++ project directory
cd cpp_raytracing_engine/

# Step 3: Create build directory if it doesn't exist
mkdir -p build

# Step 4: Enter build directory
cd build

# Step 5: Run cmake to generate build files
cmake ..

# Step 6: Build the project with make
make

# Step 7: Copy the resulting binary (edit ./mybinary and /path/to/folder as needed)
cp libcpp_raytracing_engine.dylib ../../bin/Release/net8.0/osx-arm64/publish

cd ../../

./bin/Release/net8.0/osx-arm64/publish/RayTracerAvalonia.Desktop
