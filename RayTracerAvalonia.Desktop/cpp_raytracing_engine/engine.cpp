#include "include/engine.hpp"
#include "box.hpp"
#include "include/renderer.hpp"
#include "include/scene.hpp"
#include "plane.hpp"
#include "sphere.hpp"
#include <iterator>
#include <memory>
#include <print>

const static auto scene = []() {
  auto backgroundColor = Color(0, 0, 0);
  auto light = Light({-30, 25, -12}, Color(255, 255, 255));
  /*
   * new Shapes.Plane(Vector3.UnitY, 0, new Appearance(Color.White)),
               new Box(new(-2, 0, -2), new(2, 4, 2), new
   Appearance(Color.Red)), new Sphere(new(6, 2, 0), 2, new
   Appearance(Color.Magenta)), new Sphere(new(6, 1, -4), 1, new
   Appearance(Color.Yellow)), new Sphere(new(-2, 2, 4), 2, new
   Appearance(Color.Green)), new Sphere(new(-4, 4, 10), 4, new
   Appearance(Color.Blue)), new Sphere(new(-3.2f, 1, -1), 1, new
   Appearance(Color.Cyan)),
   */
  std::vector<std::unique_ptr<Shape>> shapes;

  shapes.push_back(
      std::make_unique<Plane>(Vector3f{0, 1, 0}, 0, Color(255, 255, 255)));
  shapes.push_back(std::make_unique<Box>(Vector3f(-2, 0, -2), Vector3f(2, 4, 2),
                                         Color(255, 0, 0)));
  shapes.push_back(
      std::make_unique<Sphere>(Vector3f(6, 2, 0), 2, Color(255, 0, 255)));
  shapes.push_back(
      std::make_unique<Sphere>(Vector3f(6, 1, -4), 1, Color(255, 255, 0)));
  shapes.push_back(
      std::make_unique<Sphere>(Vector3f(-2, 2, 4), 2, Color(0, 255, 0)));
  shapes.push_back(
      std::make_unique<Sphere>(Vector3f(-4, 4, 10), 4, Color(0, 0, 255)));
  shapes.push_back(
      std::make_unique<Sphere>(Vector3f(-3.2f, 1, -1), 1, Color(0, 255, 255)));

  return new Scene(
      std::make_unique<Camera>(Vector3f{-10, 10, -20}, Vector3f{0, 4, 0}),
      std::vector<Light>{light}, std::move(shapes));
}();

extern "C" {
void SetCamera(void *scene, float x, float y, float z) {
  auto r_scene = reinterpret_cast<Scene *>(scene);
  r_scene->camera->location = Vector3f{x, y, z};
}

void *GetScene() { return scene; }

void FreeScene(void *scene) { delete reinterpret_cast<Scene *>(scene); }

void *CreateRenderer(uint16_t width, uint16_t height) {

  return new Renderer(width, height);
}

void FreeRenderer(void *renderer) {
  delete reinterpret_cast<Renderer *>(renderer);
}

// Stub for raytrace API. You may want to update this to match your actual
// logic.
void Raytrace(uint8_t *input, void *renderer, void *scene) {

  auto r_renderer = reinterpret_cast<Renderer *>(renderer);
  auto r_scene = reinterpret_cast<Scene *>(scene);


  r_renderer->Render(input, r_scene);
}
}
