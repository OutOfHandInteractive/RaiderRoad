using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLvlMove : MonoBehaviour {

	public GameObject road;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * 40 * Time.deltaTime);
	}
}
