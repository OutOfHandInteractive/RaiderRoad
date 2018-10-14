using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingV2 : MonoBehaviour {

    public GameObject floor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        // this object was clicked - do something
        Instantiate(floor, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(90,0,0));
    }
}
