using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour {

    public float windSpeed = 0.02f;

	void Update () 
	{
		transform.Translate (Vector3.back * windSpeed * Time.deltaTime);
	}
}
