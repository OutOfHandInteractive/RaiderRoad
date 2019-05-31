using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is for audio managers on the RV and enemy vehicles
/// </summary>
public class CarAudio : AudioManager
{

    /// <summary>
    /// The honk sound effect for this vehicle
    /// </summary>
    public List<AudioClip> honks;


    /// <summary>
    /// Honks the horn (Plays the current honk sound as a one shot).
    /// </summary>
    public void Honk()
    {
        RandomOneShot(honks);
    }

}
