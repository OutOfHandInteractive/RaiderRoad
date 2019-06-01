using UnityEngine;
using System.Collections.Generic;

public class SkidAudio : AudioManager
{
    public float skidVolume = 1.0f;
    public List<AudioClip> skidSounds;
    public float skidCooldown;

    [SerializeField] private float cooldown = 0;

    void Update()
    {
        cooldown = Mathf.Max(0f, cooldown - Time.deltaTime);
    }

    /// <summary>
    /// Plays a random skid mark sound
    /// </summary>
    public void Skid()
    {
        if(cooldown <= 0)
        {
            RandomOneShot(skidSounds, skidVolume);
            cooldown = skidCooldown;
        }
    }
}
