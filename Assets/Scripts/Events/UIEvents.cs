using System;
using UnityEngine;

public static class UIEvents
{
    public static Action<AudioClip> PlayBackgroundMusic;
    public static Action<AudioClip, float> PlaySFX;

    public static void RaisePlayBackgroundMusic(AudioClip clip)
    {
        PlayBackgroundMusic?.Invoke(clip);
    }
    
    public static void RaisePlaySFX(AudioClip clip, float volume)
    {
        PlaySFX?.Invoke(clip, volume);
    }
}
