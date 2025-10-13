#include "include/scene.hpp"
#include "include/camera.hpp"
#include "include/color.hpp"

// The Scene struct implementation.
// The Trace method is a stub and should be implemented with actual ray tracing
// logic.

Color Scene::Trace(float x, float y) const {
  // TODO: Implement ray tracing logic here.

  return camera->trace(*this, x, y);
}
