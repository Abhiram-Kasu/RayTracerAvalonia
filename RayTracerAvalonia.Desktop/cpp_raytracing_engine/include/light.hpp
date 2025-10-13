#pragma once

#include "color.hpp"
#include <Eigen/Core>

using Eigen::Vector3f;
struct Light {
public:
  constexpr Light(Vector3f position, Color color)
      : position(position), color(color) {}

  constexpr auto getPosition() const { return position; }
  constexpr auto getColor() const { return color; }

  //     public Color Illuminate(IMaterial material, Vector3 point, float
  //     brightness) => material.GetColorAt(point) * Color * brightness;

  auto illuminate(Color color, Vector3f point, float brightness) const noexcept
      -> Color;

private:
  Vector3f position;
  Color color;
};
