#include "include/camera.hpp"
#include "include/color.hpp"
#include "include/ray.hpp"
#include "include/scene.hpp"
#include <Eigen/Core>
#include <Eigen/Geometry>

using Eigen::Vector3f;

Color Camera::trace(const Scene &scene, float x, float y) const {
  auto xRay = right * x;
  auto yRay = -up * y;
  auto rayDir = direction + xRay + yRay;
  Ray ray(location, rayDir);
  return ray.trace(scene);
}
