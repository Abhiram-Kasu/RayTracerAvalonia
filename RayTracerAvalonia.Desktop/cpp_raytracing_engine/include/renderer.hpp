#pragma once

#include "scene.hpp"
#include <cstdint>

class Renderer {
public:
  Renderer(uint16_t width, uint16_t height);

  // Renders the scene into the provided data buffer.
  // The buffer is expected to be of size (width * height * 4) for RGBA.
  void Render(uint8_t *data, const Scene *scene) const noexcept;

  uint16_t getWidth() const noexcept { return canvasWidth; }
  uint16_t getHeight() const noexcept { return canvasHeight; }

private:
  uint16_t canvasHeight;
  uint16_t canvasWidth;
};
