﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapNode : MonoBehaviour {
    //Michael

    //--------------------
    //  Public Variables
    //--------------------
    public bool occupied = false;
    public bool isFloor; //to determine how to place if its on a floor or wall

    //--------------------
    //  Private Variables
    //--------------------
    private GameObject holo;
    private GameObject item;

    public void BuildTrap(GameObject trapToPlace)
    {
        Vector3 dir = gameObject.transform.up;
        if (isFloor)
        {
            item = Instantiate(trapToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.LookRotation(dir));
        }
        else
        {
            item = Instantiate(trapToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.LookRotation(dir));
        }
        
        occupied = true;
        item.GetComponent<Trap>().myNode = gameObject;
    }

    public void Show(GameObject trapToShow)
    { //hologram function, not efficient/working properly

        holo = Instantiate(trapToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        holo.GetComponent<Trap>().isHolo = true;
    }

    public void RemoveShow()
    {
        Destroy(holo);
    }
}