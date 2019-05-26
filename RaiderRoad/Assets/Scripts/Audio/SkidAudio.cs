using UnityEngine;
using System.Collections.Generic;

public class SkidAudio : AudioManager
{
    public float skidVolume = 1.0f;
    public List<AudioClip> skidSounds;

    /// <summary>
    /// Plays a random skid mark sound
    /// </summary>
    public void Skid()
    {
        RandomOneShot(skidSounds, skidVolume);
    }
}
