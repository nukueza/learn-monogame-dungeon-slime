using MonoGameLibrary;
using Microsoft.Xna.Framework.Media;
using DungeonSlime.Scenes;
using Gum.Forms;
using Gum.Forms.Controls;
using MonoGameGum;

namespace DungeonSlime;

public class MainGame : Core
{
  // the background theme song
  private Song _themeSong;

  public MainGame() : base("Dungeon Slime", 1280, 720, false)
  {
  }

  protected override void Initialize()
  {
    // TODO: Add your initialization logic here
    base.Initialize();

    Audio.PlaySong(_themeSong);

    ChangeScene(new TitleScene());

    // initialize gum ui servic3
    InitializeGum();
  }

  protected override void LoadContent()
  {
    // load the background theme music
    _themeSong = Content.Load<Song>("audio/theme");
  }

  private void InitializeGum()
  {
    // initialize the gum service
    GumService.Default.Initialize(this, DefaultVisualsVersion.V3);

    // using gum service to use global content manager
    GumService.Default.ContentLoader.XnaContentManager = Core.Content;

    // register keyboard input for Ui control
    FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);

    // register gampad
    FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads);

    // customize the tab reverse Ui
    FrameworkElement.TabReverseKeyCombos.Add(new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Up });

    FrameworkElement.TabReverseKeyCombos.Add(new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Down });

    // settingup size
    GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;

    GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;

    GumService.Default.Renderer.Camera.Zoom = 4.0f;

  }

}
