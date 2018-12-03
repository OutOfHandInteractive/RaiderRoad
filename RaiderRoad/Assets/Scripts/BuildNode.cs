﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNode : MonoBehaviour {
    //Michael

    //--------------------
    //  Public Variables
    //--------------------

    public GameObject wall;
    public bool occupied = false;
    public bool isHorizontal;
    public bool canPlaceWeapon = false;
    //public float height = 1f;

    //--------------------
    //  Private Variables
    //--------------------

    private GameObject holo;
    private GameObject item;

    public void Build(GameObject objectToPlace, GameObject spawnNode)
    {
        if(objectToPlace.tag == "Wall"){ //If object is a wall
            if (this.isHorizontal)
            {
                item = Instantiate(objectToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                item.transform.localScale = new Vector3(1, 1, 0.1f);
                item.transform.parent = spawnNode.transform;
                occupied = true;
            }
            else
            {
                item = Instantiate(objectToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
                item.transform.localScale = new Vector3(1, 1, 0.1f);
                item.transform.parent = spawnNode.transform;
                occupied = true;
            }
            item.GetComponent<Wall>().myNode = gameObject;
        }
        else if(objectToPlace.tag == "Weapon" && canPlaceWeapon)
        {
            Vector3 dir = gameObject.transform.forward;
            item = Instantiate(objectToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.LookRotation(dir));
            item.transform.localScale = new Vector3(1.5f, 0.7f, 0.7f);
            item.transform.parent = spawnNode.transform;

            BoxCollider coll = item.GetComponentsInChildren<BoxCollider>()[1];
            coll.enabled = true;

            item.GetComponent<Weapon>().DisableNear();
            occupied = true;
            item.GetComponent<Weapon>().myNode = gameObject;
        }
        
    }

    public void Show(GameObject makeHolo){ //hologram function
        if(makeHolo.tag == "Wall"){
            if (this.isHorizontal)
            {
                holo = Instantiate(makeHolo, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                holo.transform.parent = this.gameObject.transform;
            }
            else
            {
                holo = Instantiate(makeHolo, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
                holo.transform.parent = this.gameObject.transform;
            }
            holo.GetComponent<Wall>().isHolo = true;
        }else{
            Vector3 dir = gameObject.transform.forward;
            holo = Instantiate(makeHolo, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.LookRotation(dir));
        }
        
    }

    public void RemoveShow()
    {
        Destroy(holo);
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
