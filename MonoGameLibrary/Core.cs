using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Scenes;

namespace MonoGameLibrary;

public class Core : Game
{
  // singleton instance
  internal static Core s_instance;

  // gets a reference to the singleton instance
  public static Core Instance => s_instance;

  // The Scene that is currently active
  private static Scene s_activeScene;

  // The next scene to switch to, if there is one.
  private static Scene s_nextScene;

  // Gets the graphics device manager to control the presentation of graphics
  public static GraphicsDeviceManager Graphics { get; private set; }

  // Gets the graphics device used to create graphical resources and perform primitive rendering
  public static new GraphicsDevice GraphicsDevice { get; private set; }

  // Gets the sprite batch used for all 2D rendering
  public static SpriteBatch SpriteBatch { get; private set; }

  // Gets the content manager used to load game assets
  public static new ContentManager Content { get; private set; }

  // gets reference to the input management system
  public static InputManager Input { get; private set; }

  // get or set a value that indicates  if the game should exit
  public static bool ExitOnEscape { get; set; }

  // get a reference to the audio control system
  public static AudioController Audio {  get; private set; }

  // Create a new Core instance
  public Core(string title, int width, int height, bool fullScreen)
  {
    // ensure that multiple cores are not created
    if (s_instance != null)
      throw new InvalidOperationException("Only one instance of Core can be created.");

    // store reference to engine for global memeber access
    s_instance = this;

    // create graphics device manager
    Graphics = new GraphicsDeviceManager(this);

    // set the graphics defaults
    Graphics.PreferredBackBufferWidth = width;
    Graphics.PreferredBackBufferHeight = height;
    Graphics.IsFullScreen = fullScreen;

    // apply the graphic presentation changes
    Graphics.ApplyChanges();

    // set the window title
    Window.Title = title;

    // set the core's content manager to a reference to the base game's content manager
    Content = base.Content;

    // set the root directory for content.
    Content.RootDirectory = "Content";

    // Mouse is visible by default
    IsMouseVisible = true;

    // exit on escape is true by default
    ExitOnEscape = true;
  }

  protected override void Initialize()
  {
    base.Initialize();

    // set teh core's graphics device to a reference to the base game's graphics device
    GraphicsDevice = base.GraphicsDevice;

    // Create the sprite batch instance
    SpriteBatch = new SpriteBatch(GraphicsDevice);

    // create a new input manager
    Input = new InputManager();

    // create a new audio controller
    Audio = new AudioController();
  }

  // clean up things
  protected override void UnloadContent()
  {
    // dispose of the audio controller
    Audio.Dispose();

    base.UnloadContent();
  }

  protected override void Update(GameTime gameTime)
  {
    // update the input manager
    Input.Update(gameTime);

    // update the audio controller
    Audio.Update();

    if (ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape))
    {
      Exit();
    }

    // if there is a next scene waiting to be switch to, then transition
    // to the next scene
    if (s_nextScene != null)
    {
      TransitionScene();
    }

    // if there is an active scene, update it
    if (s_activeScene != null)
    {
      s_activeScene.Update(gameTime);
    }

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime)
  {
    // if there is an active scene, draw it
    if (s_activeScene != null)
    {
      s_activeScene.Draw(gameTime);
    }
    base.Draw(gameTime);
  }

  public static void ChangeScene(Scene next)
  {
    // Only set the next scene value if it is not the same
    // instance as the currently active scene
    if (s_activeScene != next)
    {
      s_nextScene = next;
    }
  }

  private static void TransitionScene()
  {
    if (s_activeScene != null)
    {
      s_activeScene.Dispose();
    }

    // force the gc to collect to ensure mem is cleared
    GC.Collect();

    // change the scene
    s_activeScene = s_nextScene;

    // null out
    s_nextScene = null;

    // if not null
    if (s_activeScene != null)
    {
      s_activeScene.Initialize(); 
    }
  }
}

