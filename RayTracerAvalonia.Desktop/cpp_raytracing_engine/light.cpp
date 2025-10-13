#include "light.hpp"

auto Light::illuminate(Color color, Vector3f point,
                       float brightness) const noexcept -> Color {
  return color * brightness;
}
