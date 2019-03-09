using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for audio managers on the RV and enemy vehicles
/// </summary>
public class CarAudio : AudioManager
{

    /// <summary>
    /// The honk sound effect for this vehicle
    /// </summary>
    public AudioClip honk;

    /// <summary>
    /// Honks the horn (Plays the current honk sound as a one shot).
    /// </summary>
    public void Honk()
    {
        OneShot(honk);
    }
}
