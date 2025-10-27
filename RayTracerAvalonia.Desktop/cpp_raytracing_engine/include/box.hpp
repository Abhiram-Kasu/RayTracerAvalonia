#include "Eigen/Core"
#include "shape.hpp"
#include <array>

struct Box : public Shape {
  Vector3f lowerCorner, upperCorner;
  std::array<Vector3f, 2> vertices = {lowerCorner, upperCorner};

  Color color;

  Box(Vector3f corner1, Vector3f corner2, Color color) : color(color) {
    lowerCorner = Vector3f{std::min(corner1.x(), corner2.x()),
                           std::min(corner1.y(), corner2.y()),
                           std::min(corner1.z(), corner2.z())};
    upperCorner = Vector3f{std::max(corner1.x(), corner2.x()),
                           std::max(corner1.y(), corner2.y()),
                           std::max(corner1.z(), corner2.z())};
  }

  virtual auto getColor() const -> Color;
  virtual auto intersect(const Ray &ray) const noexcept -> Intersection;

  virtual auto getNormalAt(Vector3f point) const noexcept -> Vector3f;
};
