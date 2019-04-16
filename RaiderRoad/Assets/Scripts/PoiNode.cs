using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for POI (Engine) nodes. Currently just an alias for DurabilityBuildNode.
/// </summary>
public class PoiNode : DurabilityBuildNode {

    // -------- POI Warning Indicator (flashing red square) --------

    [SerializeField] private Material myIndicMat;
    [SerializeField] private Color warnCol;
    [SerializeField] private Color missCol;
    private float quickFadeSpeed = 3f;
    private float fadeSpeed = 1.5f;
    private IEnumerator currCorou;

    private void Start()
    {
        //set object material to instance of that mat. Save it as variable
        if(transform.Find("POIWarning") != null)
        {
            Renderer myIndic = transform.Find("POIWarning").gameObject.GetComponent<Renderer>();
            myIndicMat = Instantiate(myIndic.material);
            myIndic.material = myIndicMat;
        }
    }

    public void PoiHit()
    {
        if(currCorou != null) StopCoroutine(currCorou);
        myIndicMat.color = warnCol;

        currCorou = IndicFade();
        StartCoroutine(currCorou);
    }

    public void PoiMissing()
    {
        if (currCorou != null) StopCoroutine(currCorou);
        myIndicMat.color = missCol;

        currCorou = IndicAlert();
        StartCoroutine(currCorou);
    }

    public void PoiPresent()
    {
        if (currCorou != null) StopCoroutine(currCorou);
        //set it back to 0 alpha
        Color c = myIndicMat.color;
        c.a = 0f;
        myIndicMat.color = c;
    }

    IEnumerator IndicFade()
    {
        for (int flashCount = 0; flashCount < 3; flashCount++) {
            for (float myA = 1f; myA >= 0; myA -= quickFadeSpeed * Time.deltaTime)
            {
                Color c = myIndicMat.color;
                c.a = myA;
                myIndicMat.color = c;
                yield return null;
            }
        }
    }

    IEnumerator IndicAlert()
    {
        float myA = 1f;
        while (true)
        {
            myA -= fadeSpeed * Time.deltaTime;
            if(myA < 0f)
            {
                myA = 1f;
            }
            Color c = myIndicMat.color;
            c.a = myA;
            myIndicMat.color = c;
            yield return null;
        }
    }
}