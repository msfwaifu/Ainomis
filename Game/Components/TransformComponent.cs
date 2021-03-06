namespace Ainomis.Game.Components {
  using System;

  using Ainomis.Shared.Camera;

  using Artemis.Interface;

  using Microsoft.Xna.Framework;

  internal class TransformComponent : IComponent, IFocusable {
    private Vector2 _position;

    public TransformComponent(Vector2 position, float rotation = 0f) {
      this.Rotation = rotation;
      this.Position = position;
    }

    public TransformComponent(float x = 0f, float y = 0f, float rotation = 0f) :
        this(new Vector2(x, y), rotation) {
    }

    /// <summary>Gets or sets the position.</summary>
    /// <value>The position.</value>
    public ref Vector2 Position => ref _position;

    /// <summary>Gets or sets the rotation.</summary>
    /// <value>The rotation.</value>
    public float Rotation { get; set; }

    /// <summary>Gets the rotation as radians.</summary>
    /// <value>The rotation as radians.</value>
    public float RotationAsRadians => (float)Math.PI * this.Rotation / 180f;

    /// <summary>Gets the distance between this component and another.</summary>
    public float GetDistance(TransformComponent transform) =>
      Vector2.Distance(this.Position, transform.Position);
  }
}
