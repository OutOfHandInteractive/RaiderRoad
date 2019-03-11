using UnityEngine;
using System.Collections;

/// <summary>
/// This is a behaviour that destroys the object that contains it when the audiosource completes playing.
/// Relies on timing and assumes that the source plays on awake.
/// </summary>
public class SelfDestructAudio : MonoBehaviour
{

    private float countdownTime;
    // Use this for initialization
    void Start()
    {
        countdownTime = GetComponent<AudioSource>().clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        countdownTime -= Time.deltaTime;
        if(countdownTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
