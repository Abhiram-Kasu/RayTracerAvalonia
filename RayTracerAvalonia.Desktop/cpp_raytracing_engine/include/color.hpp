#pragma once

#include <algorithm>
#include <cstdint>

// Represents an RGBA color in 8-bit channels.
struct Color {
  uint8_t r = 0;
  uint8_t g = 0;
  uint8_t b = 0;
  uint8_t a = 255;

  constexpr Color() = default;
  constexpr Color(uint8_t red, uint8_t green, uint8_t blue, uint8_t alpha = 255)
      : r(red), g(green), b(blue), a(alpha) {}

  // Equality comparison (C++20 defaulted operator)
  bool operator==(const Color &) const = default;

  Color &operator*(const float &scale) {
    // TODO vectorize
    r = static_cast<uint8_t>(std::clamp(r * scale, 0.0f, 255.0f));
    g = static_cast<uint8_t>(std::clamp(g * scale, 0.0f, 255.0f));
    b = static_cast<uint8_t>(std::clamp(b * scale, 0.0f, 255.0f));
    a = static_cast<uint8_t>(std::clamp(a * scale, 0.0f, 255.0f));
    return *this;
  }

  Color &operator+(const Color &other) noexcept {
    r = static_cast<uint8_t>(
        std::clamp(static_cast<int>(r) + static_cast<int>(other.r), 0, 255));
    g = static_cast<uint8_t>(
        std::clamp(static_cast<int>(g) + static_cast<int>(other.g), 0, 255));
    b = static_cast<uint8_t>(
        std::clamp(static_cast<int>(b) + static_cast<int>(other.b), 0, 255));
    a = static_cast<uint8_t>(
        std::clamp(static_cast<int>(a) + static_cast<int>(other.a), 0, 255));
    return *this;
  }

  Color &operator+=(const Color &other) noexcept { return *this + other; }
};
