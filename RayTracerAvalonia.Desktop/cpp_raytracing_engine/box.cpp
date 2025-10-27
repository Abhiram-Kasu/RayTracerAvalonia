#include "include/box.hpp"
#include <algorithm>
#include <cmath>
#include <limits>
#include <optional>
#include <tuple>

auto Box::getColor() const -> Color { return color; }

// Fast AABB ray intersection using slab method - much faster than the original
// C# approach
auto Box::intersect(const Ray &ray) const noexcept -> Intersection {

  // Practical epsilons for robustness under -ffast-math
  constexpr float parallel_eps = 1e-6f; // treat near-zero dir as parallel
  constexpr float hit_eps = 1e-4f;      // accept strictly positive hits

  float tmin = 0.0f; // only care about intersections in front of the ray
  float tmax = std::numeric_limits<float>::max();

  // Check intersection with each pair of parallel planes (slab method)
  for (int axis = 0; axis < 3; ++axis) {
    float dir = ray.direction[axis];
    float start = ray.start[axis];
    float lower = lowerCorner[axis];
    float upper = upperCorner[axis];

    if (std::abs(dir) < parallel_eps) {
      // Ray is parallel to the slab - if start is outside the slab, no hit
      if (start < lower || start > upper) {
        return std::nullopt; // No intersection
      }
      // Inside slab; this axis imposes no constraint on t
      continue;
    }

    // Calculate intersection distances with the two planes
    float inv_dir = 1.0f / dir;
    float t1 = (lower - start) * inv_dir;
    float t2 = (upper - start) * inv_dir;

    // Ensure t1 is near and t2 is far
    if (t1 > t2)
      std::swap(t1, t2);

    // Grow the near plane and shrink the far plane
    tmin = std::max(tmin, t1);
    tmax = std::min(tmax, t2);

    // If the interval becomes invalid, no intersection
    if (tmin > tmax || tmax < 0.0f) {
      return std::nullopt; // No intersection
    }
  }

  // Add valid intersections (strictly positive to avoid self-intersection)
  float intersections[2] = {};
  int ptr = 0;

  if (tmin > hit_eps)
    intersections[ptr++] = tmin;
  if (tmax > hit_eps && tmax != tmin)
    intersections[ptr++] = tmax;

  switch (ptr) {
  case 0:
    return std::nullopt;
  case 1:
    return {intersections[0]};
  case 2:
    return std::tuple{intersections[0], intersections[1]};
    default:
      return std::nullopt; // Should not reach here
  }
}

auto Box::getNormalAt(Vector3f pos) const noexcept -> Vector3f {
  constexpr float threshold = 1e-4f;

  // Unrolled loop for maximum performance
  // Check X faces
  if (std::abs(lowerCorner.x() - pos.x()) < threshold)
    return Vector3f(-1.0f, 0.0f, 0.0f);
  if (std::abs(upperCorner.x() - pos.x()) < threshold)
    return Vector3f(1.0f, 0.0f, 0.0f);

  // Check Y faces
  if (std::abs(lowerCorner.y() - pos.y()) < threshold)
    return Vector3f(0.0f, -1.0f, 0.0f);
  if (std::abs(upperCorner.y() - pos.y()) < threshold)
    return Vector3f(0.0f, 1.0f, 0.0f);

  // Check Z faces
  if (std::abs(lowerCorner.z() - pos.z()) < threshold)
    return Vector3f(0.0f, 0.0f, -1.0f);
  if (std::abs(upperCorner.z() - pos.z()) < threshold)
    return Vector3f(0.0f, 0.0f, 1.0f);

  // Point is not on surface - return zero vector as fallback
  // In a production system, you might want to find the closest face
  return Vector3f(0.0f, 0.0f, 0.0f);
}
