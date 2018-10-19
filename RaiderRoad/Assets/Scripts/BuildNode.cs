using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNode : MonoBehaviour {

    //--------------------
    //  Public Variables
    //--------------------

    public GameObject wall;
    //public float height = 1f;
    public bool isHorizontal;
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Build()
    {
        if (this.isHorizontal)
        {
            Instantiate(wall, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(wall, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
        }
    }

    //TODO: To be removed later when testing is complete
    void OnMouseDown(){
        // this object was clicked - do something
        if(this.isHorizontal)
        {
            Instantiate(wall, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(wall, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
        }
    }
}
