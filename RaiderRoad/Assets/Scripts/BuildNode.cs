using System.Collections;
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
    //public float height = 1f;

    //--------------------
    //  Private Variables
    //--------------------

    private GameObject holo;

    public void Build(GameObject wallToPlace)
    {
        if (this.isHorizontal)
        {
            Instantiate(wallToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            occupied = true;
        }
        else
        {
            Instantiate(wallToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
            occupied = true;
        }
        
    }

    public void Show(GameObject wallToShow){ //hologram function, not efficient/working properly

        holo = wallToShow;

        if (this.isHorizontal)
        {
            Instantiate(holo, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(holo, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
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
