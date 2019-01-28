using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapNode : AbstractBuildNode {
    //Michael

    //--------------------
    //  Public Variables
    //--------------------

    //--------------------
    //  Private Variables
    //--------------------
    private GameObject holo;
    private GameObject item;

    public void BuildTrap(GameObject trapToPlace)
    {
        Vector3 dir = gameObject.transform.up;
        item = Instantiate(trapToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.identity);
        
        occupied = true;
        item.transform.parent = transform;
        item.GetComponent<Trap>().myNode = gameObject;
    }

    public void Show(GameObject trapToShow)
    { //hologram function, not efficient/working properly
        Debug.Log(trapToShow.GetComponent<Trap>().isHolo);
        holo = Instantiate(trapToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Trap trap = holo.GetComponent<Trap>();
        trap.isHolo = true;
    }

    public void RemoveShow()
    {
        Destroy(holo);
    }
}
