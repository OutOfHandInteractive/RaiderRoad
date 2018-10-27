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
        Vector3 dir = gameObject.transform.forward;
        if (isFloor)
        {
            Instantiate(trapToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.identity);
        }
        else
        {
            Instantiate(trapToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.LookRotation(dir));
        }
        
        occupied = true;
    }
}
