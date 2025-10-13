#include "include/plane.hpp"
#include <cmath>
#include <limits>

auto Plane::getColor() const -> Color { return color; }

auto Plane::intersect(const Ray &ray) const noexcept -> std::vector<float> {
  float angle = normal.dot(ray.direction);

  // If the dot-product is zero, the ray is parallel to the plane
  // and will never intersect
  if (std::abs(angle) < std::numeric_limits<float>::epsilon()) {
    return {};
  }

  float b = normal.dot(ray.start + normal * (-distance));
  float t = -b / angle;

  return {t};
}

auto Plane::getNormalAt(Vector3f point) const noexcept -> Vector3f {
  // For a plane, the normal is constant everywhere
  return normal;
}
