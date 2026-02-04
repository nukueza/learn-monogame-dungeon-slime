using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Graphics;

public class Animation
{
  // the texture regions that make up the frames of this animation
  // the order of the regions within the collection are the order that the frames should be displayed in.
  public List<TextureRegion> Frames { get; private set; }

  // the amount of time to delay between each frame before moving to teh next frame for this animation
  public TimeSpan Delay { get; set; }

  // creates a new animation
  public Animation()
  {
    Frames = new List<TextureRegion>();
    Delay = TimeSpan.FromMilliseconds(100);
  }

  // creates a new animation with the specified frames and delay
  public Animation(List<TextureRegion> frames, TimeSpan delay)
  {
    Frames = frames;
    Delay = delay;
  }
}

