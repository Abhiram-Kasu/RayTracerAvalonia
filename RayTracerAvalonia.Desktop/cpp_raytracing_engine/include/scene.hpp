#pragma once

#include "camera.hpp"
#include "color.hpp"
#include "light.hpp"
#include "shape.hpp"
#include <cstdint>
#include <iostream>
#include <memory>
#include <vector>

// The Scene struct represents the entire scene to be rendered.
// It contains the camera and background color, and provides a trace method
// to compute the color at a given pixel coordinate.
struct Scene {
  Color background;
  std::unique_ptr<Camera> camera;
  std::vector<Light> lights;
  std::vector<std::unique_ptr<Shape>> shapes;

  // Constructor
  Scene(std::unique_ptr<Camera> &&cam, std::vector<Light> &&lights,
        std::vector<std::unique_ptr<Shape>> &&shapes)
      : background{0, 0, 0, 255}, camera(std::move(cam)),
        lights(std::move(lights)), shapes(std::move(shapes)) {}

  // Trace a ray at pixel (x, y) and return the resulting color.
  // This should be implemented to perform actual ray tracing logic.
  [[nodiscard]]
  auto Trace(float x, float y) const -> Color;
};

// ostream operator for Scene
inline std::ostream &operator<<(std::ostream &os, const Scene &scene) {
  os << "Scene {\n";
  os << "  Background: RGBA(" << static_cast<int>(scene.background.r) << ", "
     << static_cast<int>(scene.background.g) << ", "
     << static_cast<int>(scene.background.b) << ", "
     << static_cast<int>(scene.background.a) << ")\n";

  if (scene.camera) {
    os << "  Camera: Location(" << scene.camera->location.x() << ", "
       << scene.camera->location.y() << ", " << scene.camera->location.z()
       << "), "
       << "LookAt(" << scene.camera->lookAt.x() << ", "
       << scene.camera->lookAt.y() << ", " << scene.camera->lookAt.z() << ")\n";
  } else {
    os << "  Camera: null\n";
  }

  os << "  Lights: " << scene.lights.size() << " light(s)\n";
  for (size_t i = 0; i < scene.lights.size(); ++i) {
    const auto &light = scene.lights[i];
    os << "    Light[" << i << "]: Position(" << light.getPosition().x() << ", "
       << light.getPosition().y() << ", " << light.getPosition().z() << "), "
       << "Color RGBA(" << static_cast<int>(light.getColor().r) << ", "
       << static_cast<int>(light.getColor().g) << ", "
       << static_cast<int>(light.getColor().b) << ", "
       << static_cast<int>(light.getColor().a) << ")\n";
  }

  os << "  Shapes: " << scene.shapes.size() << " shape(s)\n";
  for (size_t i = 0; i < scene.shapes.size(); ++i) {
    if (scene.shapes[i]) {
      const auto &shape = scene.shapes[i];
      const auto color = shape->getColor();
      os << "    Shape[" << i << "]: Color RGBA(" << static_cast<int>(color.r)
         << ", " << static_cast<int>(color.g) << ", "
         << static_cast<int>(color.b) << ", " << static_cast<int>(color.a)
         << ")\n";
    } else {
      os << "    Shape[" << i << "]: null\n";
    }
  }

  os << "}";
  return os;
}
