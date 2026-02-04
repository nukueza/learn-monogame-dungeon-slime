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

    private Vector2 _batPosition;
    private Vector2 _batVelocity;
    
    public MainGame() : base("Dungeon Slime", 1280, 720, false)
    {
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here
      base.Initialize();
      _inputBuffer = new Queue<Vector2>(MAX_BUFFER_SIZE);
      _batPosition = new Vector2(_slime.Width + 10, 0);
      AssignRandomBatVelocity();
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

      // create a bounding rectangle for the screen
      Rectangle screenBounds = new Rectangle(
        0,
        0,
        GraphicsDevice.PresentationParameters.BackBufferWidth,
        GraphicsDevice.PresentationParameters.BackBufferHeight
      );

      // creating a bounding circle for the slime
      Circle slimeBounds = new Circle(
        (int)(_slimePosition.X + (_slime.Width * 0.5f)),
        (int)(_slimePosition.Y + (_slime.Height * 0.5f)),
        (int)(_slime.Width * 0.5f)
      );

      // the bat are move like a dvd
      if (slimeBounds.Left < screenBounds.Left)
      {
        _slimePosition.X = screenBounds.Left;
      }
      else if (slimeBounds.Right > screenBounds.Right)
      {
        _slimePosition.X = screenBounds.Right - _slime.Width;
      }

      if (slimeBounds.Top < screenBounds.Top)
      {
        _slimePosition.Y = screenBounds.Top;
      }
      else if (slimeBounds.Bottom > screenBounds.Bottom)
      {
        _slimePosition.Y = screenBounds.Bottom - _slime.Height;
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
      if (batBounds.Left < screenBounds.Left)
      {
        normal.X = Vector2.UnitX.X;
        newBatPosition.X = screenBounds.Left;
      }
      else if (batBounds.Right > screenBounds.Right)
      {
        normal.X = -Vector2.UnitX.X;
        newBatPosition.X = screenBounds.Right - _bat.Width;
      }

      if (batBounds.Top < screenBounds.Top)
      {
        normal.Y = Vector2.UnitY.Y;
        newBatPosition.Y = screenBounds.Top;
      }
      else if (batBounds.Bottom > screenBounds.Bottom)
      {
        normal.Y = -Vector2.UnitY.Y;
        newBatPosition.Y = screenBounds.Bottom - _bat.Height;
      }

      if (normal != Vector2.Zero)
      {
        normal.Normalize();
        _batVelocity = Vector2.Reflect(_batVelocity, normal);
      }

      _batPosition = newBatPosition;

      if (slimeBounds.Intersects(batBounds))
      {
        // Divide the width  and height of the screen into equal columns and
        // rows based on the width and height of the bat.
        int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
        int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

        // Choose a random row and column based on the total number of each
        int column = Random.Shared.Next(0, totalColumns);
        int row = Random.Shared.Next(0, totalRows);

        // Change the bat position by setting the x and y values equal to
        // the column and row multiplied by the width and height.
        _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

        // Assign a new random velocity to the bat
        AssignRandomBatVelocity();
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
        _batPosition
       );


      SpriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
