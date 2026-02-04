using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input
{
  public class MouseInfo
  {
    // the state of mouse input during the previous update cycle
    public MouseState PreviousState { get; private set; }

    // the state of mouse input during the current update cycle
    public MouseState CurrentState { get; private set; }

    // get or set the current position of the mouse cursor
    public Point Position
    {
      get => CurrentState.Position;
      set => SetPosition(value.X, value.Y);
    }

    // gets the difference in the mouse cursor position between the current and previous update cycle
    public Point PositionDelta => CurrentState.Position - PreviousState.Position;

    // gets the difference in the mouse cursor X position between the current and previous update cycle
    public int XDelta => CurrentState.X - PreviousState.X;

    //gets the difference in the mouse cursor Y position between the current and previous update cycle
    public int YDelta => CurrentState.Y - PreviousState.Y;

    // gets a value that indicates if the mouse cursor moved between the preivious and current update cycle
    public bool HasMoved => PositionDelta != Point.Zero;

    // gets the cumulative value of the mouse scroll wheel
    public int ScrollWheel => CurrentState.ScrollWheelValue;

    // gets the value of the scroll wheel between the previous and current update cycle
    public int ScrollWheelDelta => CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue;

    // creates a new mouse info
    public MouseInfo()
    {
      PreviousState = new MouseState();
      CurrentState = Mouse.GetState();
    }

    // updates the state information about mouse input
    public void Update()
    {
      PreviousState = CurrentState;
      CurrentState = Mouse.GetState();
    }

    // return a value that indicates wheter the specified mouse button is currently down
    public bool IsButtonDown(MouseButton button)
    {
      switch(button)
      {
        case MouseButton.Left:
          return CurrentState.LeftButton == ButtonState.Pressed;
        case MouseButton.Middle:
          return CurrentState.MiddleButton == ButtonState.Pressed;
        case MouseButton.Right:
          return CurrentState.RightButton == ButtonState.Pressed;
        case MouseButton.XButton1:
          return CurrentState.XButton1 == ButtonState.Pressed;
        case MouseButton.XButton2:
          return CurrentState.XButton2 == ButtonState.Pressed;
        default:
          return false;
      }
    }

    // return a value that indicates wheter the specified mouse button is currently up
    public bool IsButtonUp(MouseButton button)
    {
      switch (button)
      {
        case MouseButton.Left:
          return CurrentState.LeftButton == ButtonState.Released;
        case MouseButton.Middle:
          return CurrentState.MiddleButton == ButtonState.Released;
        case MouseButton.Right:
          return CurrentState.RightButton == ButtonState.Released;
        case MouseButton.XButton1:
          return CurrentState.XButton1 == ButtonState.Released;
        case MouseButton.XButton2:
          return CurrentState.XButton2 == ButtonState.Released;
        default:
          return false;
      }
    }

    // returns a value that indicates whether the specified mouse button was pressed during the current update cycle
    public bool wasButtonPressed(MouseButton button)
    {
      switch (button)
      {
        case MouseButton.Left:
          return CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released;
        case MouseButton.Middle:
          return CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released;
        case MouseButton.Right:
          return CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released;
        case MouseButton.XButton1:
          return CurrentState.XButton1 == ButtonState.Pressed && PreviousState.XButton1 == ButtonState.Released;
        case MouseButton.XButton2:
          return CurrentState.XButton2 == ButtonState.Pressed && PreviousState.XButton2 == ButtonState.Released;
        default:
          return false;
      }
    }

    // return a value that indicates whether the specified mouse button was released during the current update cycle
    public bool WasButtonJustReleased(MouseButton button)
    {
      switch (button)
      {
        case MouseButton.Left:
          return CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed;
        case MouseButton.Middle:
          return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed;
        case MouseButton.Right:
          return CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed;
        case MouseButton.XButton1:
          return CurrentState.XButton1 == ButtonState.Released && PreviousState.XButton1 == ButtonState.Pressed;
        case MouseButton.XButton2:
          return CurrentState.XButton2 == ButtonState.Released && PreviousState.XButton2 == ButtonState.Pressed;
        default:
          return false;
      }
    }

    // sets the position of the mouse cursor
    public void SetPosition(int x, int y)
    {
      Mouse.SetPosition(x, y);
      CurrentState = new MouseState(
          x,
          y,
          CurrentState.ScrollWheelValue,
          CurrentState.LeftButton,
          CurrentState.MiddleButton,
          CurrentState.RightButton,
          CurrentState.XButton1,
          CurrentState.XButton2
      );
    }
  }
}
