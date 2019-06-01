using UnityEngine;
using System.Collections;

public class IntroLooper : AudioManager
{
    public AudioClip intro;
    public AudioClip loop;

    protected override void OnStart()
    {
        base.OnStart();
        audioSource.clip = loop;
        audioSource.loop = true;
        OneShot(intro);
        audioSource.PlayDelayed(intro.length);
    }
}
