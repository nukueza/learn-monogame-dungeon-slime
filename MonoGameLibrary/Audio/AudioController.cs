using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGameLibrary.Audio;

public class AudioController : IDisposable
{
  // tracks sound effect
  private readonly List<SoundEffectInstance> _activeSoundEffectInstances;

  // tracks the volume for song playback when mutting and unmuttin
  private float _previousSongVolume;

  // tracks the volume for sound effect playback
  private float _previousSoundEffectVolume;

  // gets a value that indicates if audio is muted
  public bool IsMuted { get; private set; }

  // get or set global volume song
  public float SongVolume
  {
    get
    {
      if (IsMuted)
      {
        return 0.0f;
      }
      return MediaPlayer.Volume;
    }
    set
    {
      if (IsMuted)
      {
        return;
      }
      MediaPlayer.Volume = Math.Clamp(value, 0.0f, 1.0f);
    }
  }

  // get or set global volume for sound effects
  public float SoundEffectVolume
  {
    get
    {
      if (IsMuted)
      {
        return 0.0f;
      }
      return SoundEffect.MasterVolume;
    }
    set
    {
      if (IsMuted)
      {
        return;
      }
      SoundEffect.MasterVolume = Math.Clamp(value, 0.0f, 1.0f);
    }
  }

  // gets a value that indicates if this audio controller has been disposed
  public bool IsDisposed { get; private set; }

  // create a new audio controller instance
  public AudioController()
  {
    _activeSoundEffectInstances = new List<SoundEffectInstance>();
  }

  // Finalizer called when object is collected by the garbage collector
  ~AudioController() => Dispose(false);

  // updates this audio controller
  public void Update()
  {
    for (int i = _activeSoundEffectInstances.Count - 1; i >= 0; i--)
    {
      SoundEffectInstance instance = _activeSoundEffectInstances[i];

      if (instance.State == SoundState.Stopped)
      {
        if (!instance.IsDisposed)
        {
          instance.Dispose();
        }
        _activeSoundEffectInstances.RemoveAt(i);
      }
    }
  }

  // play the given sound effect
  public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect)
  {
    return PlaySoundEffect(soundEffect, 1.0f, 0.0f, 0.0f, false);
  }

  // plays the given sound effect with the specified properties
  public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect, float volume, float pitch, float pan, bool isLooped)
  {
    // create an instance from the sound effect given
    SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

    // Apply volume, pitch, pan, and loop values
    soundEffectInstance.Volume = volume;
    soundEffectInstance.Pitch = pitch;
    soundEffectInstance.Pan = pan;
    soundEffectInstance.IsLooped = isLooped;

    // tell the instance to play
    soundEffectInstance.Play();

    // add it to the active instances for tracking
    _activeSoundEffectInstances.Add(soundEffectInstance);

    return soundEffectInstance;
  }

  // plays the given song.
  public void PlaySong(Song song, bool isRepeating = true)
  {
    // check if the media player is already playing, if so, stop it
    // if we do not stop it, this could cause issues on some platforms
    if (MediaPlayer.State == MediaState.Playing)
    {
      MediaPlayer.Stop();
    }
    MediaPlayer.Play(song);
    MediaPlayer.IsRepeating = isRepeating;
  }

  // pauses all audio
  public void PauseAudio()
  {
    // pause any active song playing
    MediaPlayer.Pause();

    // pause any active sound effects
    foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
    {
      soundEffectInstance.Pause();
    }
  }

  public void MuteAudio()
  {
    // store the volume so they can be restored during ResumeAudio
    _previousSongVolume = MediaPlayer.Volume;
    _previousSoundEffectVolume = SoundEffect.MasterVolume;

    // set all volumes to 0
    MediaPlayer.Volume = 0.0f;
    SoundEffect.MasterVolume = 0.0f;

    IsMuted = true;
  }

  public void UnmutedAudio()
  {
    // Restore the previous volume values
    MediaPlayer.Volume = _previousSongVolume;
    SoundEffect.MasterVolume = _previousSoundEffectVolume;

    IsMuted = false;
  }

  public void ToggleMute()
  {
    if (IsMuted)
    {
      UnmutedAudio();
    }
    else
    {
      MuteAudio();
    }
  }

  // clean up
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected void Dispose(bool disposing)
  {
    if (IsDisposed)
    {
      return;
    }

    if (disposing)
    {
      foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
      {
        soundEffectInstance.Dispose();
      }
      _activeSoundEffectInstances.Clear();
    }

    IsDisposed = true;
  }
}
