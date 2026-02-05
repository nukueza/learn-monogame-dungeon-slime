using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System.Collections.Generic;
using MonoGameLibrary.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using DungeonSlime.Scenes;

namespace DungeonSlime
{
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
    }

    protected override void LoadContent()
    {
      // load the background theme music
      _themeSong = Content.Load<Song>("audio/theme");
    }

  }
}