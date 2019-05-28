using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class slideController : MonoBehaviour
{
    [SerializeField] private RenderTexture myRenderText;
    [SerializeField] private VideoPlayer vp1;
    [SerializeField] private VideoPlayer vp2;
    private bool CurrIs1 = true;
    //two video players, one prepares their clip while the other plays their clip (no waiting for load)
    private float videoTimer;
    [SerializeField] private float timeForClipOnscreen;

    private int clipIndex = 0;
    [SerializeField] private VideoClip[] myClipsArr; //all clips in order they will play
    [SerializeField] private bool[] loopOrNotArr; //bool of whether clip in sequence is a transition or a looping

    // Start is called before the first frame update
    void Start()
    {
        //vp = GetComponentInChildren<VideoPlayer>();
        clipIndex = 0;

        //set both clips to something at start
        vp1.clip = myClipsArr[clipIndex];
        vp2.clip = myClipsArr[clipIndex];

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
        VideoPlayer vpCurr;
        VideoPlayer vpNext;
        //assign which player is current vs next based on CurrIs1
        if (CurrIs1) {
            vpCurr = vp1;
            vpNext = vp2;
        } else {
            vpCurr = vp2;
            vpNext = vp1;
        }
        CurrIs1 = !CurrIs1; //flip bool

        vpCurr.targetTexture = myRenderText;
        vpCurr.Play();
        if (loopOrNotArr[clipIndex]) {
            vpCurr.isLooping = true;
            videoTimer = timeForClipOnscreen - (timeForClipOnscreen % (vpCurr.frameCount / vpCurr.frameRate));
        } else {
            vpCurr.isLooping = false;
            videoTimer = vpCurr.frameCount / vpCurr.frameRate;
        }
        clipIndex++;
        if(clipIndex >= myClipsArr.Length)
        {
            clipIndex = 0;
        }

        //queue next clip
        vpNext.clip = myClipsArr[clipIndex];
        vpNext.Prepare();
    }

    // Script for hitting projector to skip slides
    public void SkipToNextClip()
    {
        if((clipIndex + 1) % 3 == 0) { //if not already switching
            //find the next slide in clip (its a clip divisible by 3 or 0)
            bool nextClipFound = false;
            while (!nextClipFound)
            {
                if ((clipIndex + 1) % 3 == 0){
                    ToNextClip();
                    nextClipFound = true;
                } else {
                    clipIndex++;

                    if (clipIndex >= myClipsArr.Length)
                    {
                        clipIndex = 0;
                    }
                }
            }
        }
    }

}
