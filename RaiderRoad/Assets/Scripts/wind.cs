using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour {

    private float i;

	void Update () 
	{
        i = -0.01f;
        i *= transform.position.z;
		transform.Translate (new Vector3 (0, 0, i));
	}
}
