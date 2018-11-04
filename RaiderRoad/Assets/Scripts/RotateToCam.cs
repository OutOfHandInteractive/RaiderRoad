using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCam : MonoBehaviour {

    private Quaternion CamRot;

	// Use this for initialization
	void Start () {
        CamRot = Camera.main.transform.rotation;

    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.rotation = CamRot;
    }
}
