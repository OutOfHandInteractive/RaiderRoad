using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour {
    //Michael

    //--------------------
    //  Public Variables
    //--------------------
    public bool occupied = false;

	public void BuildTrap(GameObject trapToPlace)
    {
        Instantiate(trapToPlace, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity);
        occupied = true;
    }
}
