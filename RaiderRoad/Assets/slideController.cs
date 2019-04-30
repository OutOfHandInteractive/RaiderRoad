using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class slideController : MonoBehaviour
{
    private VideoPlayer vp;
    private float videoTimer;
    [SerializeField] private float timeForClipOnscreen;

    private int clipIndex = 0;
    [SerializeField] private VideoClip[] myClipsArr; //all clips in order they will play
    [SerializeField] private bool[] loopOrNotArr; //bool of whether clip in sequence is a transition or a looping

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponentInChildren<VideoPlayer>();
        clipIndex = 0;
        ToNextClip();
    }

    // Update is called once per frame
    void Update()
    {
        videoTimer -= Time.deltaTime;
        if (videoTimer <= 0f)
        {
            ToNextClip();
        }
    }

    //progressive enhancement: hit projector to skip to next slide. Requires a check to it doesn't just skip the transition
    private void ToNextClip()
    {
        vp.clip = myClipsArr[clipIndex];
        vp.Play();
        if (loopOrNotArr[clipIndex]) {
            vp.isLooping = true;
            videoTimer = timeForClipOnscreen - (timeForClipOnscreen % (vp.frameCount / vp.frameRate));
        } else {
            vp.isLooping = false;
            videoTimer = vp.frameCount / vp.frameRate;
        }
        clipIndex++;
        if(clipIndex >= myClipsArr.Length)
        {
            clipIndex = 0;
        }
    }
}
