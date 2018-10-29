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
    private GameObject item;

    public void Build(GameObject wallToPlace, GameObject spawnNode)
    {
        if (this.isHorizontal)
        {
            item = Instantiate(wallToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            item.transform.localScale = new Vector3(1, 1, 0.1f);
            item.transform.parent = spawnNode.transform;
            occupied = true;
        }
        else
        {
            item = Instantiate(wallToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
            item.transform.localScale = new Vector3(1, 1, 0.1f);
            item.transform.parent = spawnNode.transform;
            occupied = true;
        }
        item.GetComponent<Wall>().myNode = gameObject;
    }

    public void Show(GameObject wallToShow){ //hologram function

        if (this.isHorizontal)
        {
            holo = Instantiate(wallToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else
        {
            holo = Instantiate(wallToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
        }

        holo.GetComponent<Wall>().isHolo = true;
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
