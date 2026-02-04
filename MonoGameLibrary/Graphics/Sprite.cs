using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameLibrary.Graphics;

public class Sprite
{
  // gets or set the source texture region represented by this sprite
  public TextureRegion Region { get; private set; }

  // get or sets teh color mask to apply when rendering the sprite
  public Color Color { get; set; } = Color.White;

  // gets or sets the amount of rotation to apply when rendering the sprite
  public float Rotation { get; set; } = 0.0f;

  // get or sets the scale factor to apply when rendering the sprite
  public Vector2 Scale { get; set; } = Vector2.One;

  // gets or sets the origin point to use when rendering the sprite
  public Vector2 Origin { get; set; } = Vector2.Zero;

  // gets or sets the layer depth to use when rendering the sprite
  public float LayerDepth { get; set; } = 0.0f;

  // gets or sets the effects to use when rendering the sprite
  public SpriteEffects Effects { get; set; } = SpriteEffects.None;

  // gets the width of the sprite
  public float Width => Region.Width * Scale.X;

  // gets the height of the sprite
  public float Height => Region.Height * Scale.Y;

  // creates a new sprite
  public Sprite() { }

  // Creates a new sprite using the speciifed texture region
  public Sprite(TextureRegion region)
  {
    Region = region; 
  }

  public void CenterOrigin()
  {
    Origin = new Vector2(Region.Width, Region.Height) * 0.5f;
  }

  // submit this sprite for drawing to the current batch
  public void Draw(
    SpriteBatch spriteBatch,
    Vector2 position
  )
  {
    Region.Draw(
      spriteBatch,
      position,
      Color,
      Rotation,
      Origin,
      Scale,
      Effects,
      LayerDepth
    );
  }
}

