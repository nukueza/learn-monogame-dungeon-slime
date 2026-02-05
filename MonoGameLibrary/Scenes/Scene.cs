using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary.Scenes;

public abstract class Scene : IDisposable
{
  // asset loaded through this
  protected ContentManager Content { get; }

  public bool IsDisposed { get; private set; }

  // create a new scene instance
  public Scene()
  {
    // create a content manager for the scene
    Content = new ContentManager(Core.Content.ServiceProvider);

    // set the root directory for the content to the same as the root directory
    // for the game's content.
    Content.RootDirectory = Core.Content.RootDirectory;
  }

  // finalizer, called when object is cleaned up by garbage collector
  ~Scene() => Dispose(false);

  // Initialize the scene
  public virtual void Initialize()
  {
    LoadContent();
  }

  // override to provide logic to load content for the scene
  public virtual void LoadContent() { }

  // unload scene-specific content
  public virtual void UnloadContent()
  {
    Content.Unload();
  }

  // updates this scene
  public virtual void Update(GameTime gameTime) { } 

  // draws this scene
  public virtual void Draw(GameTime gameTime) { }

  // IDisposable implementation
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (IsDisposed)
    {
      return;
    }

    if (disposing)
    {
      UnloadContent();
      Content.Dispose();
    }

    IsDisposed = true;
  }

}

