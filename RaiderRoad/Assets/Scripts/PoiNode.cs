using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiNode : MonoBehaviour {

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

    public void BuildPoi(GameObject PoiToShow, float myDur)
    {
        Vector3 dir = gameObject.transform.forward;
        if (isFloor)
        {
            item = Instantiate(PoiToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.identity);
            item.transform.parent = this.gameObject.transform;
        }
        else
        {
            item = Instantiate(PoiToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.LookRotation(dir));
            item.transform.parent = this.gameObject.transform;
        }

        occupied = true;
        item.GetComponent<Engine>().myNode = gameObject;

        item.GetComponent<Engine>().SetDurability(myDur);
    }

    public void Show(GameObject PoiToShow)
    { //hologram function, not efficient/working properly

        holo = Instantiate(PoiToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        holo.GetComponent<Engine>().isHolo = true;
        holo.transform.parent = this.gameObject.transform;
    }

    public void RemoveShow()
    {
        Destroy(holo);
    }
}