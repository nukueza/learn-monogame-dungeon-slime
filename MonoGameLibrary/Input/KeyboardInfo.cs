using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class KeyboardInfo
{
  // gets the state of keyboard input during the previous update cycle
  public KeyboardState PreviousState { get; private set; }

  // gets the state of keyboard input during the current input cycle
  public KeyboardState CurrentState { get; private set; }

  // creates a new keyboard info instance
  public KeyboardInfo()
  {
    PreviousState = new KeyboardState();
    CurrentState = Keyboard.GetState();
  }

  // updates the state information about keyboard input.
  public void Update()
  {
    PreviousState = CurrentState;
    CurrentState = Keyboard.GetState();
  }

  // returns a value that indicates if the specified key is currently down
  public bool IsKeyDown(Keys key)
  {
    return CurrentState.IsKeyDown(key);
  }

  // returns a value that indicates if the specified key is currently up
  public bool IsKeyUp(Keys key)
  {
    return CurrentState.IsKeyUp(key);
  }

  public bool WasKeyJustPressed(Keys key)
  {
    return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
  }

  // returns a value that indicates if the specified key was just pressed on the current frame
  public bool WasKeyPressed(Keys key)
  {
    return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
  }

  // returns a value that indicates if the specified key was just released on the current frame
  public bool wasKeyReleased(Keys key)
  {
    return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
  }
}

