using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime
{
  public class MainGame : Core
  {
    private Sprite _slime;
    private Sprite _bat;
    
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

      // create texture atlas from the xml configuration file
      TextureAtlas atlas = TextureAtlas.FromFile(
        Content,
        "images/atlas-definition.xml"
      );

      _slime = atlas.CreateSprite("slime");
      _slime.Scale = new Vector2(4.0f, 4.0f);

      _bat = atlas.CreateSprite("bat");
      _bat.Scale = new Vector2(4.0f, 4.0f);

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

      SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
      
      _slime.Draw(
        SpriteBatch,
        Vector2.Zero
       );

      _bat.Draw(
        SpriteBatch,
        new Vector2(
          _slime.Width + 10,
          0)
       );


      SpriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
