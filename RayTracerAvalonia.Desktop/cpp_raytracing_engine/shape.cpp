#include "shape.hpp"
#include "scene.hpp"
#include <algorithm>
#include <ranges>
#include <vector>

auto Shape::CastsShadowFor(Vector3f point, Vector3f lightVector) const noexcept
    -> bool {
  auto distanceToLight = lightVector.norm();
  auto ray = Ray(point, lightVector);
  if (auto res = this->ClosestDistanceAlongRay(ray)) {
    return *res <= distanceToLight;
  }
  return false;
}

auto Shape::ClosestDistanceAlongRay(Ray ray) const noexcept
    -> std::optional<float> {
  auto distances = this->intersect(ray);
  if (distances.empty())
    return std::nullopt;

  constexpr float epsilon = 1e-4f;
  std::optional<float> best;
  for (float t : distances) {
    if (t > epsilon && (!best || t < *best)) {
      best = t;
    }
  }
  return best;
}

auto Shape::getColorAt(Vector3f point, const Scene &scene) const noexcept
    -> Color {

  auto normal = this->getNormalAt(point);
  auto color = Color{0, 0, 0};
  auto base = this->getColor();

  for (const auto &light : scene.lights) {
    auto lightVector = light.getPosition() - point;
    auto lightDir = lightVector.normalized();
    auto brightness = lightDir.dot(normal);
    auto castsShadow = false;

    for (const auto &shape : scene.shapes) {
      if (shape.get() != this && shape->CastsShadowFor(point, lightVector)) {
        castsShadow = true;
        break;
      }
    }

    if (castsShadow || brightness <= 0.0f)
      continue;

    // Use the material/base color, not the accumulator
    color += light.illuminate(base, point, brightness);
  }

  return color;
}
