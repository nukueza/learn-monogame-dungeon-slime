using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using System;
using System.Collections.Generic;

namespace DungeonSlime.Scenes;

public class GameScene : Scene
{
  // Defines the slime animated sprite.
  private AnimatedSprite _slime;

  // Defines the bat animated sprite.
  private AnimatedSprite _bat;

  // Tracks the position of the slime.
  private Vector2 _slimePosition;

  // Speed multiplier when moving.
  private const float MOVEMENT_SPEED = 5.0f;

  // Tracks the position of the bat.
  private Vector2 _batPosition;

  // Tracks the velocity of the bat.
  private Vector2 _batVelocity;

  // Defines the tilemap to draw.
  private Tilemap _tilemap;

  // Defines the bounds of the room that the slime and bat are contained within.
  private Rectangle _roomBounds;

  // The sound effect to play when the bat bounces off the edge of the screen.
  private SoundEffect _bounceSoundEffect;

  // The sound effect to play when the slime eats a bat.
  private SoundEffect _collectSoundEffect;

  // The SpriteFont Description used to draw text
  private SpriteFont _font;

  // Tracks the players score.
  private int _score;

  // Defines the position to draw the score text at.
  private Vector2 _scoreTextPosition;

  // Defines the origin used when drawing the score text.
  private Vector2 _scoreTextOrigin;

  public override void Initialize()
  {
    // TODO: Add your initialization logic here
    base.Initialize();

    Core.ExitOnEscape = false;

    Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

    _roomBounds = new Rectangle(
      (int)_tilemap.TileWidth,
      (int)_tilemap.TileHeight,
      screenBounds.Width - (int)_tilemap.TileWidth * 2,
      screenBounds.Height - (int)_tilemap.TileHeight * 2
    );

    int centerRow = _tilemap.Rows / 2;
    int centerColumn = _tilemap.Columns / 2;
    _slimePosition = new Vector2(centerColumn * _tilemap.TileWidth, centerRow * _tilemap.TileHeight);

    _batPosition = new Vector2(_roomBounds.Left, _roomBounds.Top);

    _scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileHeight * 0.5f);

    float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
    _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);

    _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");
    _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");
    _font = Content.Load<SpriteFont>("fonts/04B_30");

    AssignRandomBatVelocity();
  }

  public override void LoadContent()
  {
    // create texture atlas from the xml configuration file
    TextureAtlas atlas = TextureAtlas.FromFile(
      Content,
      "images/atlas-definition.xml"
    );

    _slime = atlas.CreateAnimatedSprite("slime-animation");
    _slime.Scale = new Vector2(4.0f, 4.0f);

    _bat = atlas.CreateAnimatedSprite("bat-animation");
    _bat.Scale = new Vector2(4.0f, 4.0f);

    _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
    _tilemap.Scale = new Vector2(4.0f, 4.0f);

    // load the bounce sound effect
    _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");
    _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

    // load font
    _font = Content.Load<SpriteFont>("fonts/04B_30");
  }

  public override void Update(GameTime gameTime)
  {
    _slime.Update(gameTime);
    _bat.Update(gameTime);

    CheckKeyboardInput();
    CheckGamePadInput();

    // create a bounding rectangle for the screen
    //Rectangle screenBounds = new Rectangle(
    //  0,
    //  0,
    //  GraphicsDevice.PresentationParameters.BackBufferWidth,
    //  GraphicsDevice.PresentationParameters.BackBufferHeight
    //);

    // creating a bounding circle for the slime
    Circle slimeBounds = new Circle(
      (int)(_slimePosition.X + (_slime.Width * 0.5f)),
      (int)(_slimePosition.Y + (_slime.Height * 0.5f)),
      (int)(_slime.Width * 0.5f)
    );

    // the bat are move like a dvd
    if (slimeBounds.Left < _roomBounds.Left)
    {
      _slimePosition.X = _roomBounds.Left;
    }
    else if (slimeBounds.Right > _roomBounds.Right)
    {
      _slimePosition.X = _roomBounds.Right - _slime.Width;
    }

    if (slimeBounds.Top < _roomBounds.Top)
    {
      _slimePosition.Y = _roomBounds.Top;
    }
    else if (slimeBounds.Bottom > _roomBounds.Bottom)
    {
      _slimePosition.Y = _roomBounds.Bottom - _slime.Height;
    }

    // calculate the new position of the bat based on the velocity
    Vector2 newBatPosition = _batPosition + _batVelocity;

    // create bounding circle for the bat
    Circle batBounds = new Circle(
      (int)(newBatPosition.X + (_bat.Width * 0.5f)),
      (int)(newBatPosition.Y + (_bat.Height * 0.5f)),
      (int)(_bat.Width * 0.5f)
    );

    Vector2 normal = Vector2.Zero;
    if (batBounds.Left < _roomBounds.Left)
    {
      normal.X = Vector2.UnitX.X;
      newBatPosition.X = _roomBounds.Left;
    }
    else if (batBounds.Right > _roomBounds.Right)
    {
      normal.X = -Vector2.UnitX.X;
      newBatPosition.X = _roomBounds.Right - _bat.Width;
    }

    if (batBounds.Top < _roomBounds.Top)
    {
      normal.Y = Vector2.UnitY.Y;
      newBatPosition.Y = _roomBounds.Top;
    }
    else if (batBounds.Bottom > _roomBounds.Bottom)
    {
      normal.Y = -Vector2.UnitY.Y;
      newBatPosition.Y = _roomBounds.Bottom - _bat.Height;
    }

    if (normal != Vector2.Zero)
    {
      normal.Normalize();
      _batVelocity = Vector2.Reflect(_batVelocity, normal);

      Core.Audio.PlaySoundEffect(_bounceSoundEffect);
    }

    _batPosition = newBatPosition;

    if (slimeBounds.Intersects(batBounds))
    {
      // Divide the width  and height of the screen into equal columns and
      // rows based on the width and height of the bat.
      //int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
      //int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

      // Choose a random row and column based on the total number of each
      int column = Random.Shared.Next(1, _tilemap.Columns - 1);
      int row = Random.Shared.Next(1, _tilemap.Rows - 1);

      // Change the bat position by setting the x and y values equal to
      // the column and row multiplied by the width and height.
      _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

      // Assign a new random velocity to the bat
      AssignRandomBatVelocity();

      Core.Audio.PlaySoundEffect(_collectSoundEffect);

      _score += 100;
    }

    base.Update(gameTime);
  }

  private void AssignRandomBatVelocity()
  {
    // Generate a random angle.
    float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

    // Convert angle to a direction vector.
    float x = (float)Math.Cos(angle);
    float y = (float)Math.Sin(angle);
    Vector2 direction = new Vector2(x, y);

    // Multiply the direction vector by the movement speed.
    _batVelocity = direction * MOVEMENT_SPEED;
  }

  private void CheckKeyboardInput()
  {
    // get the state of keyboard input
    KeyboardState keyboardState = Keyboard.GetState();
    Vector2 newDirection = Vector2.Zero;

    // if the space key is held down, the movement speed increase
    float speed = MOVEMENT_SPEED;
    if (Core.Input.Keyboard.IsKeyDown(Keys.Space))
    {
      speed *= 1.5f;
    }

    // move the slime based on keyboard input
    if (Core.Input.Keyboard.IsKeyDown(Keys.W) || Core.Input.Keyboard.IsKeyDown(Keys.Up))
    {
      _slimePosition.Y -= speed;
      newDirection = -Vector2.UnitY;
    }
    if (Core.Input.Keyboard.IsKeyDown(Keys.S) || Core.Input.Keyboard.IsKeyDown(Keys.Down))
    {
      _slimePosition.Y += speed;
      newDirection = Vector2.UnitY;
    }
    if (Core.Input.Keyboard.IsKeyDown(Keys.A) || Core.Input.Keyboard.IsKeyDown(Keys.Left))
    {
      _slimePosition.X -= speed;
      newDirection = -Vector2.UnitX;
    }
    if (Core.Input.Keyboard.IsKeyDown(Keys.D) || Core.Input.Keyboard.IsKeyDown(Keys.Right))
    {
      _slimePosition.X += speed;
      newDirection = Vector2.UnitX;
    }

    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.M))
    {
      Core.Audio.ToggleMute();
    }

    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.OemPlus))
    {
      Core.Audio.SongVolume += 0.1f;
      Core.Audio.SoundEffectVolume += 0.1f;
    }

    // if the button was pressed, decrease the volume
    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.OemMinus))
    {
      Core.Audio.SongVolume -= 0.1f;
      Core.Audio.SoundEffectVolume -= 0.1f;
    }
  }

  private void CheckGamePadInput()
  {
    GamePadInfo gamePadOne = Core.Input.GamePads[(int)PlayerIndex.One];

    float speed = MOVEMENT_SPEED;
    if (gamePadOne.IsButtonDown(Buttons.A))
    {
      speed *= 1.5f;
      gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(1));
    }
    else
    {
      gamePadOne.StopVibration();
    }

    if (gamePadOne.LeftThumbStick != Vector2.Zero)
    {
      _slimePosition.X += gamePadOne.LeftThumbStick.X * speed;
      _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * speed;
    }
    else
    {
      if (gamePadOne.IsButtonDown(Buttons.DPadUp))
      {
        _slimePosition.Y -= speed;
      }

      if (gamePadOne.IsButtonDown(Buttons.DPadDown))
      {
        _slimePosition.Y += speed;
      }

      if (gamePadOne.IsButtonDown(Buttons.DPadLeft))
      {
        _slimePosition.X -= speed;
      }

      if (gamePadOne.IsButtonDown(Buttons.DPadRight))
      {
        _slimePosition.X += speed;
      }
    }
  }

  public override void Draw(GameTime gameTime)
  {
    Core.GraphicsDevice.Clear(Color.CornflowerBlue);

    Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

    _tilemap.Draw(Core.SpriteBatch);

    _slime.Draw(
      Core.SpriteBatch,
      _slimePosition
     );

    _bat.Draw(
      Core.SpriteBatch,
      _batPosition
     );

    Core.SpriteBatch.DrawString(
      _font,
      $"Score: {_score}",
      _scoreTextPosition,
      Color.White,
      0.0f,
      _scoreTextOrigin,
      1.0f,
      SpriteEffects.None,
      0.0f
    );

    Core.SpriteBatch.End();
  }

}
