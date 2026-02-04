using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime
{
  public class MainGame : Core
  {
    private Texture2D _logo;

    public MainGame() : base("Dungeon Slime", 1280, 720, false)
    {
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here
      base.Initialize();
    }

    protected override void LoadContent()
    {
      // TODO: use this.Content to load your game content here
      _logo = Content.Load<Texture2D>("images/logo");
      base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      Rectangle iconSourceRect = new Rectangle(0, 0, 128, 128);
      Rectangle wordmarkSourceRect = new Rectangle(150, 34, 458, 58);

      SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

      SpriteBatch.Draw(
        texture: _logo,
        position: new Vector2(
          Window.ClientBounds.Width,
          Window.ClientBounds.Height) * 0.5f,
        sourceRectangle: iconSourceRect,
        color: Color.White,
        rotation: 0.0f,
        origin: new Vector2(
          iconSourceRect.Width,
          iconSourceRect.Height) * 0.5f,
        scale: 1.0f,
        effects: SpriteEffects.None,
        layerDepth: 1.0f
       );

      SpriteBatch.Draw(
        texture: _logo,
        position: new Vector2(
          Window.ClientBounds.Width,
          Window.ClientBounds.Height) * 0.5f,
        sourceRectangle: wordmarkSourceRect,
        color: Color.White,
        rotation: 0.0f,
        origin: new Vector2(
          wordmarkSourceRect.Width,
          wordmarkSourceRect.Height) * 0.5f,
        scale: 1.0f,
        effects: SpriteEffects.None,
        layerDepth: 1.0f
       );


      SpriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
