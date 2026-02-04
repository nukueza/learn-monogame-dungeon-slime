using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System.Collections.Generic;
using MonoGameLibrary.Input;

namespace DungeonSlime
{
  public class MainGame : Core
  {
    private AnimatedSprite _slime;
    private AnimatedSprite _bat;

    private Vector2 _slimePosition;
    private const float MOVEMENT_SPEED = 5.0f;
    private const int MAX_BUFFER_SIZE = 2;

    private Queue<Vector2> _inputBuffer;
    
    public MainGame() : base("Dungeon Slime", 1280, 720, false)
    {
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here
      base.Initialize();
      _inputBuffer = new Queue<Vector2>(MAX_BUFFER_SIZE);
    }

    protected override void LoadContent()
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

    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      // TODO: Add your update logic here
      _slime.Update(gameTime);
      _bat.Update(gameTime);

      CheckKeyboardInput();
      CheckGamePadInput();

      base.Update(gameTime);
    }

    private void CheckKeyboardInput()
    {
      // get the state of keyboard input
      KeyboardState keyboardState = Keyboard.GetState();
      Vector2 newDirection = Vector2.Zero;

      // if the space key is held down, the movement speed increase
      float speed = MOVEMENT_SPEED;
      if (Input.Keyboard.IsKeyDown(Keys.Space))
      {
        speed *= 1.5f;
      }

      // move the slime based on keyboard input
      if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
      {
        _slimePosition.Y -= speed;
        newDirection = -Vector2.UnitY;
      }
      if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
      {
        _slimePosition.Y += speed;
        newDirection = Vector2.UnitY;
      }
      if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
      {
        _slimePosition.X -= speed;
        newDirection = -Vector2.UnitX;
      }
      if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
      {
        _slimePosition.X += speed;
        newDirection = Vector2.UnitX;
      }

      if (newDirection != Vector2.Zero && _inputBuffer.Count < MAX_BUFFER_SIZE)
      {
        _inputBuffer.Enqueue(newDirection);
      }

      if (_inputBuffer.Count > 0)
      {
        Vector2 bufferedDirection = _inputBuffer.Dequeue();
        _slimePosition += bufferedDirection * speed * 0.5f;
      }
    }

    private void CheckGamePadInput()
    {
      GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];

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

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
      
      _slime.Draw(
        SpriteBatch,
        _slimePosition
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
