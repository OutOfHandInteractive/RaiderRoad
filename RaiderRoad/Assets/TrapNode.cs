using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapNode : MonoBehaviour {
    //Michael

    //--------------------
    //  Public Variables
    //--------------------
    public bool occupied = false;
    public bool isFloor; //to determine how to place if its on a floor or wall

    public void BuildTrap(GameObject trapToPlace)
    {
        Quaternion parentRot = gameObject.transform.rotation;
        Instantiate(trapToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), parentRot);
        occupied = true;
    }
}
