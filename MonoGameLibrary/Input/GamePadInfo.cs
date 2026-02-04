using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class GamePadInfo
{
  private TimeSpan _vibrationTimeRemaining = TimeSpan.Zero;

  // get the index of the player this game pad is for.
  public PlayerIndex PlayerIndex { get; }

  // gets the state of input for this gamepad during the previous update cycle
  public GamePadState PreviousState { get; private set; }

  // gets the state of input for this gamepad during the current update cycle
  public GamePadState CurrentState { get; private set; }

  // gets a value that indicates if this gamepad is currently connected
  public bool IsConnected => CurrentState.IsConnected;

  // gets the value of the left thumbstick of this gamepad
  public Vector2 LeftThumbStick => CurrentState.ThumbSticks.Left;

  // gets the value of the left trigger of this gamepad
  public float LeftTrigger => CurrentState.Triggers.Left;

  // getst the value of the right trigger of this gamepad
  public float RightTrigger => CurrentState.Triggers.Right;

  // creates a new gamepad info for the gamepad connected at the specified player index
  public GamePadInfo(PlayerIndex playerIndex)
  {
    PlayerIndex = playerIndex;
    PreviousState = new GamePadState();
    CurrentState = GamePad.GetState(playerIndex);
  }

  // Updates the state information for this gamepad input
  public void Update(GameTime gameTime)
  {
    PreviousState = CurrentState;
    CurrentState = GamePad.GetState(PlayerIndex);

    if (_vibrationTimeRemaining > TimeSpan.Zero)
    {
      _vibrationTimeRemaining -= gameTime.ElapsedGameTime;
      if (_vibrationTimeRemaining <= TimeSpan.Zero)
      {
        StopVibration();
      }
    }
  }

  // returns a value that indicates wheter the specified gamepad button is current down
  public bool IsButtonDown(Buttons button)
  {
    return CurrentState.IsButtonDown(button);
  }

  public bool IsButtonUp(Buttons button)
  {
    return CurrentState.IsButtonUp(button);
  }

  public bool WasButtonJustPressed(Buttons button)
  {
    return CurrentState.IsButtonDown(button) && PreviousState.IsButtonUp(button);
  }

  public bool WasButtonJustReleased(Buttons button)
  {
    return CurrentState.IsButtonUp(button) && PreviousState.IsButtonDown(button);
  }

  // set the vibration
  public void SetVibration(float strength, TimeSpan time)
  {
    _vibrationTimeRemaining = time;
    GamePad.SetVibration(PlayerIndex, strength, strength);
  }

  public void StopVibration()
  {
    GamePad.SetVibration(PlayerIndex, 0.0f, 0.0f);
  }

}
