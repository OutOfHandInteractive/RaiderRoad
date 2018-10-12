using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour {

	void Update () 
	{
		transform.Translate (Vector3.back * 0.02f);
	}
}
