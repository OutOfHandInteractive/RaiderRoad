using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a helper class for objects with a number of different sounds to play on an AudioSource.
/// The source is found automatically at startup, but can be set later.
/// </summary>
public class AudioManager : MonoBehaviour
{

    protected AudioSource audioSource;

    /// <summary>
    /// Picks up the AudioSource attached to the same object as this component
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        OnStart();
    }

    protected virtual void OnStart()
    {
        //Nothing
    }

    /// <summary>
    /// Helper function to play a one shot clip. If given a null clip this method will return without error and nothing will be played.
    /// </summary>
    /// <param name="clip">The clip to play (may be null)</param>
    /// <param name="volumeScale">The volume to play it at</param>
    protected void OneShot(AudioClip clip, float volumeScale = 1.0f) {
        if(clip != null) {
            audioSource.PlayOneShot(clip, volumeScale);
        }
    }

    protected void RandomOneShot(List<AudioClip> clips, float volumeScale = 1.0f)
    {
        OneShot(clips[Random.Range(0, clips.Count)], volumeScale);
    }

    /// <summary>
    /// Set the AudioSource this manager will use.
    /// </summary>
    /// <param name="source">The audio source to use</param>
    public void SetSource(AudioSource source)
    {
        audioSource = source;
    }
}
