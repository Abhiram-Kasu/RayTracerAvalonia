cmake_minimum_required(VERSION 3.15)

set(CMAKE_C_COMPILER "clang")
set(CMAKE_CXX_COMPILER "clang++")

project(cpp_raytracing_engine)

# Export compile commands for clangd/LSP
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

# Specify the source files for your library
set(SOURCES
    engine.cpp
    renderer.cpp
    scene.cpp
    camera.cpp
    color.cpp
    ray.cpp
    shape.cpp
    light.cpp
    box.cpp
    plane.cpp
    sphere.cpp
)

# Find Eigen3
find_package(Eigen3 QUIET)
if(NOT Eigen3_FOUND)
    # Fallback to common install locations
    find_path(EIGEN3_INCLUDE_DIR Eigen/Core
        PATHS
        /usr/local/include/eigen
        /usr/local/include/eigen3
        /usr/local/include
        /opt/homebrew/include/eigen
        /opt/homebrew/include/eigen3
        /opt/homebrew/include
    )
    if(EIGEN3_INCLUDE_DIR)
        message(STATUS "Found Eigen3: ${EIGEN3_INCLUDE_DIR}")
    endif()
endif()

# Create shared library
add_library(cpp_raytracing_engine SHARED ${SOURCES})
target_include_directories(cpp_raytracing_engine PUBLIC
    ${CMAKE_CURRENT_SOURCE_DIR}/include
)

# Add Eigen include directory if found
if(EIGEN3_INCLUDE_DIR)
    target_include_directories(cpp_raytracing_engine PUBLIC ${EIGEN3_INCLUDE_DIR})
else()
    # Fallback include paths
    target_include_directories(cpp_raytracing_engine PUBLIC
        /usr/local/include/eigen
        /usr/local/include
    )
endif()

# Link Eigen3 if found
if(TARGET Eigen3::Eigen)
    target_link_libraries(cpp_raytracing_engine PUBLIC Eigen3::Eigen)
endif()

# (Optional) Specify the C++ standard
target_compile_features(cpp_raytracing_engine PUBLIC cxx_std_23)

# Manually add OpenMP support for Homebrew LLVM/Clang on macOS
target_compile_options(cpp_raytracing_engine PRIVATE -Xpreprocessor -fopenmp -I/opt/homebrew/opt/libomp/include -O3 -ffast-math)
target_link_libraries(cpp_raytracing_engine PRIVATE /opt/homebrew/opt/libomp/lib/libomp.dylib)
