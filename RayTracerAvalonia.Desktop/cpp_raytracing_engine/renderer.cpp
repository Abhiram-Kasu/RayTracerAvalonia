#include "include/renderer.hpp"
#include "include/scene.hpp"
#include <algorithm>
#include <cstdint>
#include <print>

// Constructor implementation
Renderer::Renderer(uint16_t width, uint16_t height)
    : canvasHeight(height), canvasWidth(width) {}

// Render method implementation
void Renderer::Render(uint8_t *data, const Scene *scene) const noexcept {
  // Parallelize using OpenMP if available (C++23 std::execution can be used for
  // more advanced parallelism)

#pragma omp parallel for
  for (uint16_t h = 0; h < canvasHeight; ++h) {

    for (uint16_t w = 0; w < canvasWidth; ++w) {
      auto x = (static_cast<float>(w) / canvasWidth) - 0.5f;
      auto y = (static_cast<float>(h) / canvasHeight) - 0.5f;

      auto color = scene->Trace(x, y);
      auto mw = static_cast<uint16_t>(canvasWidth - 1 - w);

      auto index = (h * canvasWidth + mw) * 4;

      data[index] = color.b;
      data[index + 1] = color.g;
      data[index + 2] = color.r;
      data[index + 3] = color.a;
    }
  }
}
