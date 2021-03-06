﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {
    //contains a reference to the item it will bestow the player
    public GameObject item;
    public float myItemDur = -1f;
    public bool isOccupied = false;
    public bool isTaken = false;
    private void Start()
    {
        if(myItemDur < 0 && item.GetComponent<DurableConstruct>() != null)
        {
            myItemDur = item.GetComponent<DurableConstruct>().GetDurability();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "floor")
        {
            transform.parent = other.transform.root;
			// If object has rigidbody (Battery)
			if (GetComponent<Rigidbody>() != null) {
				Destroy(GetComponent<Rigidbody>());
			}
			//else
   //         {
   //             transform.localPosition = new Vector3(0, 0, 0);
   //         }
        }
        if(other.gameObject.tag == "Destructable")
        {
            transform.parent = other.transform.root;
            transform.localPosition = new Vector3(0, .5f, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "floor" || other.gameObject.tag == "Destructable")
        {
            transform.parent = null;
        }
    }

}
