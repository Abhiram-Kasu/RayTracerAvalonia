
#pragma once

#include "color.hpp"
#include <Eigen/Core>
#include <Eigen/Geometry>

// Forward declarations to avoid circular dependencies
struct Scene;

using Eigen::Vector3f;

struct Camera {
  Vector3f location, lookAt;
  float Width, Height;
  Vector3f direction, right, up;

  constexpr explicit Camera(Vector3f location, Vector3f lookAt,
                            float width = 4.0f, float height = 9.0f / 4.0f)
      : location(location), lookAt(lookAt), Width(width), Height(height),
        direction((lookAt - location).normalized()),
        right(direction.cross(Vector3f(0, 1, 0)).normalized()),
        up(right.cross(direction).normalized()) {}

  Color trace(const Scene &scene, float x, float y) const;
};
