#pragma once

#include "Eigen/Core"
#include "shape.hpp"

using Eigen::Vector3f;

struct Plane : public Shape {
  Vector3f normal;
  float distance;
  Color color;

  Plane(Vector3f normal, float distance, Color color)
      : normal(normal.normalized()), distance(distance), color(color) {}

  virtual auto getColor() const -> Color override;
  virtual auto intersect(const Ray &ray) const noexcept
      -> std::vector<float> override;
  virtual auto getNormalAt(Vector3f point) const noexcept -> Vector3f override;
};
