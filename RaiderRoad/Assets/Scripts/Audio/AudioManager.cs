using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    protected AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void OneShot(AudioClip clip, float volumeScale = 1.0f)
    {
        if(clip != null)
        {
            audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}
