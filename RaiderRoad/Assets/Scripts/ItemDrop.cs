﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {
    //contains a reference to the item it will bestow the player
    public GameObject item;
    public float myItemDur = -1f;

    private void Start()
    {
        if(myItemDur < 0 && item.GetComponent<DurableConstruct>() != null)
        {
            myItemDur = item.GetComponent<DurableConstruct>().GetDurability();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "floor")
        {
            transform.SetParent(other.transform, true);
            transform.localPosition = new Vector3(0, 0, 0);
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
