namespace Ainomis.Shared {
  using System;

  using Microsoft.Xna.Framework;

  public struct Size : IEquatable<Size> {
    public Size(uint width = 0, uint height = 0) {
      Width = width;
      Height = height;
    }

    public uint Width { get; set; }

    public uint Height { get; set; }

    public uint Area => this.Width * this.Height;

    public static implicit operator Vector2(Size size) {
      return new Vector2(size.Width, size.Height);
    }

    public bool Equals(Size other) {
      return this.Width == other.Width && this.Height == other.Height;
    }
  }
}
