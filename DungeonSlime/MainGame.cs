using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime
{
  public class MainGame : Core
  {
    private TextureRegion _slime;
    private TextureRegion _bat;

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

      // retrieve the slime region from the atlas
      _slime = atlas.GetRegion("slime");

      // retrieve the bat region from the atlas
      _bat = atlas.GetRegion("bat");
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
        Vector2.Zero,
        Color.White,
        0.0f,
        Vector2.One,
        4.0f,
        SpriteEffects.None,
        0.0f
       );

      _bat.Draw(
        SpriteBatch,
        new Vector2(
          _slime.Width * 4.0f + 10,
          0),
        Color.White,
        0.0f,
        Vector2.One,
        4.0f,
        SpriteEffects.None,
        0.0f
       );


      SpriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
