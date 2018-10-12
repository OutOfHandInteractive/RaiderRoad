using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPlacement : MonoBehaviour {

    public GameObject wall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
        if ((other.gameObject.name == "BuildNode" || other.name == "xNode") && Input.GetKeyDown("space"))
        {
            if (other.name != "xNode")
            {
                Instantiate(wall, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(wall, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
            }
        }
    }
}
