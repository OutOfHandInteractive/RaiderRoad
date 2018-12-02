using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    private GameObject rv;
    private float rvZStart;
    private float warningZStart;

	void Awake() {
        warningZStart = transform.position.z;
	}
	
	void Update () {
        float newZ = warningZStart + (rv.transform.position.z - rvZStart);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
	}

    public void SetValues(float value, GameObject other)
    {
        rvZStart = value;
        rv = other;
    }
}
