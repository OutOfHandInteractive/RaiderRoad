using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles sounds common to players and enemies
/// </summary>
public class CharacterAudio : AudioManager
{
    /// <summary>
    /// The sound of swinging their weapon
    /// </summary>
    public AudioClip swing;

    /// <summary>
    /// The sound their weapon makes when it hits
    /// </summary>
    public AudioClip hit;

    /// <summary>
    /// The offset between the swing and the hit in seconds.
    /// </summary>
    [SerializeField] private float hitOffset = 0.1f;

    /// <summary>
    /// Plays the swing effect as a one shot then the hit sound if the flag is true.
    /// </summary>
    /// <param name="andHit">true if the hit sound should happen, false otherwise</param>
    public void Swing(bool andHit)
    {
        OneShot(swing);
        if(andHit && hit != null)
        {
            audioSource.clip = hit;
            audioSource.PlayDelayed(hitOffset);
        }
    }
}
