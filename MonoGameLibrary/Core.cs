using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary;

public class Core : Game
{
  // singleton instance
  internal static Core s_instance;

  // gets a reference to the singleton instance
  public static Core Instance => s_instance;

  // Gets the graphics device manager to control the presentation of graphics
  public static GraphicsDeviceManager Graphics { get; private set; }

  // Gets the graphics device used to create graphical resources and perform primitive rendering
  public static new GraphicsDevice GraphicsDevice { get; private set; }

  // Gets the sprite batch used for all 2D rendering
  public static SpriteBatch SpriteBatch { get; private set; }

  // Gets the content manager used to load game assets
  public static new ContentManager Content { get; private set; }

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
  }

  protected override void Initialize()
  {
    base.Initialize();

    // set teh core's graphics device to a reference to the base game's graphics device
    GraphicsDevice = base.GraphicsDevice;

    // Create the sprite batch instance
    SpriteBatch = new SpriteBatch(GraphicsDevice);
  }
}

