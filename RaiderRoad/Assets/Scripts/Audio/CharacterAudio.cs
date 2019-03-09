using UnityEngine;
using System.Collections;

public class CharacterAudio : AudioManager
{
    public AudioClip swing;
    public AudioClip hit;
    [SerializeField] private float hitOffset = 0.1f;

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
