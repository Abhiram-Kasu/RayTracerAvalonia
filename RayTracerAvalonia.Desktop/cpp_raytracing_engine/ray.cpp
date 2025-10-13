#include "include/ray.hpp"
#include "include/color.hpp"
#include "include/scene.hpp"
#include <Eigen/Core>
#include <memory>

using Eigen::Vector3f;

// Implementation of Ray methods

Color Ray::trace(const Scene &scene) const {
  // TODO: Implement intersection logic and shading
  // For now, return background color as a placeholder
  auto minDistance = std::numeric_limits<float>::infinity();
  Shape *closestShape = nullptr;
  Vector3f point;

  for (const auto &shape : scene.shapes) {

    auto distance = shape->ClosestDistanceAlongRay(*this);
    if (!distance.has_value())
      continue;
    if (*distance < minDistance) {

      minDistance = *distance;
      closestShape = shape.get();
      point = this->start + (this->direction * minDistance);
    }
  }

  return closestShape != nullptr ? closestShape->getColorAt(point, scene)
                                 : scene.background;
}
