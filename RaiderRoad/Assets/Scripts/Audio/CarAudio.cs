using UnityEngine;
using System.Collections;

public class CarAudio : AudioManager
{

    public AudioClip honk;

    public void Honk()
    {
        OneShot(honk);
    }
}
