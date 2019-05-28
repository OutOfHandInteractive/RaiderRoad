using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingDots : MonoBehaviour
{
	public int maxDots = 3;
	public float secondsPerDot;
	public Text loadingText;

	private int numberDots = 0;
	private float currentTime = 0f;


    // Update is called once per frame
    void Update()
    {
		currentTime += Time.deltaTime;

		if (currentTime >= secondsPerDot) {
			currentTime = 0;
			if (numberDots < maxDots) {
				loadingText.text = loadingText.text + ".";
				numberDots++;
			}
			else {
				loadingText.text = "LOADING";
				numberDots = 0;
			}
		}
    }
}
