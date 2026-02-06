using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

// Represents a rectangular region within a texture
public class TextureRegion
{
  // gets or sets the source texture this texture reigon is part of
  public Texture2D Texture { get; set; }

  // gets or sets the source rectangle within the texture
  public Rectangle SourceRectangle { get; set; }

  // gets the width of the texture region
  public int Width => SourceRectangle.Width;

  // gets the height of the texture region
  public int Height => SourceRectangle.Height;

  // gets the top normalized texture coordinate of this region
  public float TopTextureCoordinate => SourceRectangle.Top / (float) Texture.Height;

  // gets the bottom normalized texture coordinate of this region
  public float BottomTextureCoordinate => SourceRectangle.Bottom / (float) Texture.Height;

  // gets the left normalized texture coordinate of this region
  public float LeftTextureCoordinate => SourceRectangle.Left / (float) Texture.Width;

  // gets the right normalized texture coordinate of this region
  public float RightTextureCoordinate => SourceRectangle.Right / (float) Texture.Width;

  // creates a new texture region
  public TextureRegion() { }

  // creates a new texture region using the specified source rectangle
  public TextureRegion(Texture2D texture, int x, int y, int width, int height)
  {
    Texture = texture;
    SourceRectangle = new Rectangle(x, y, width, height);
  }

  // submit this texture region for drawing in teh current batch
  public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
  {
    Draw(spriteBatch, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
  }

  // submit this texture region for drawing in the current batch
  public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color,
    float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
  {
    Draw(
      spriteBatch,
      position,
      color,
      rotation,
      origin,
      new Vector2(scale, scale),
      effects,
      layerDepth
    );
  }

  // submit this texture region for drawing in the current batch
  public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color,
    float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
  {
    spriteBatch.Draw(
      texture: Texture,
      position: position,
      sourceRectangle: SourceRectangle,
      color: color,
      rotation: rotation,
      origin: origin,
      scale: scale,
      effects: effects,
      layerDepth: layerDepth
    );
  }
}

