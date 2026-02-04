using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public class AnimatedSprite : Sprite
{
  private int _currentFrame;
  private TimeSpan _elapsed;
  private Animation _animation;

  // gets or sets the animation for this animated sprite
  public Animation Animation
  {
    get => _animation;
    set
    {
      _animation = value;
      Region = _animation.Frames[0];
    }
  }

  // creates a new animated sprite
  public AnimatedSprite() { }

  // creates a new animated sprite using the specified animation frames and delay
  public AnimatedSprite(Animation animation)
  {
    Animation = animation;
  }

  // updates this animated sprite
  public void Update(GameTime gameTime)
  {
    _elapsed += gameTime.ElapsedGameTime;

    if (_elapsed >= Animation.Delay)
    {
      _elapsed -= _animation.Delay;
      _currentFrame++;

      if (_currentFrame >= _animation.Frames.Count)
      {
        _currentFrame = 0;
      }

      Region = _animation.Frames[_currentFrame];
    }
  }

}

