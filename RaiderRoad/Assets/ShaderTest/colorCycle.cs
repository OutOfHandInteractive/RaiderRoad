using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorCycle : MonoBehaviour {

    public Color[] myColors;
    public float cycleTime = 2f;
    private int currInt = 0;
    private int nextInt= 1;
    private float timer = 0f;
    private Color currColor;
    private Material myMat;

	// Use this for initialization
	void Start () {
        myMat = gameObject.GetComponent<Renderer>().material;
        myMat.SetColor("_Color", myColors[0]);
	}
	
	// Update is called once per frame
	void Update () {
		if(currInt >= myColors.Length)
        {
            currInt = 0;
        }

        if (nextInt >= myColors.Length)
        {
            nextInt = 0;
        }

        timer += Time.deltaTime;

        currColor = Color.Lerp(myColors[currInt], myColors[nextInt], timer);
        myMat.SetColor("_Color", currColor);

        if (timer >= cycleTime)
        {
            currInt++;
            nextInt++;
            timer = 0f;
        }
	}
}
