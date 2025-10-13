
#pragma once

#include "color.hpp"
#include <Eigen/Core>
#include <cstdint>

// Forward declaration
struct Scene;

using Eigen::Vector3f;

struct Ray {
  Vector3f start;
  Vector3f direction;

  Ray(const Vector3f &start, const Vector3f &direction)
      : start(start), direction(direction.normalized()) {}

  [[nodiscard]]
  Color trace(const Scene &scene) const;
};
