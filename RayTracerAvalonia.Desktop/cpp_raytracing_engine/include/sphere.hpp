#pragma once

#include "Eigen/Core"
#include "shape.hpp"

using Eigen::Vector3f;

struct Sphere : public Shape {
  Vector3f center;
  float radius;
  Color color;

  Sphere(Vector3f center, float radius, Color color)
      : center(center), radius(radius), color(color) {}

  virtual auto getColor() const -> Color override;
  virtual auto intersect(const Ray &ray) const noexcept
      -> std::vector<float> override;
  virtual auto getNormalAt(Vector3f point) const noexcept -> Vector3f override;
};
