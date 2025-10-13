#include "include/sphere.hpp"
#include <cmath>
#include <limits>

auto Sphere::getColor() const -> Color { return color; }

auto Sphere::intersect(const Ray &ray) const noexcept -> std::vector<float> {
  Vector3f os = ray.start - center;
  float b = 2.0f * os.dot(ray.direction);
  float c = os.squaredNorm() - radius * radius;
  float disc = b * b - 4.0f * c;

  if (disc < 0) {
    // No intersection
    return {};
  } else if (disc == 0) {
    // Tangent intersection
    return {-b / 2.0f};
  } else {
    // Two intersections
    float root = std::sqrt(disc);
    float t1 = (-b - root) / 2.0f;
    float t2 = (-b + root) / 2.0f;
    return {t1, t2};
  }
}

auto Sphere::getNormalAt(Vector3f point) const noexcept -> Vector3f {
  // Calculate the normal vector at a point on the sphere
  Vector3f normal = point - center;
  return normal.normalized();
}
