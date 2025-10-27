#pragma once

#include "color.hpp"
#include "ray.hpp"
#include <tuple>
#include <variant>
#include <vector>

// Forward declaration to avoid circular dependency
struct Scene;

struct Shape {
  using Intersection =
      std::optional<std::variant<float, std::tuple<float, float>>>;

  virtual auto getColor() const -> Color = 0;
  virtual auto intersect(const Ray &ray) const -> Intersection = 0;

  virtual auto getNormalAt(Vector3f point) const noexcept -> Vector3f = 0;

  auto CastsShadowFor(Vector3f point, Vector3f lightVector) const noexcept
      -> bool;

  auto ClosestDistanceAlongRay(Ray ray) const noexcept -> std::optional<float>;

  auto getColorAt(Vector3f point, const Scene &scene) const noexcept -> Color;

  virtual ~Shape() = default;
};
