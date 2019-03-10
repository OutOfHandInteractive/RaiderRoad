using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build node for walls/weapons. Can be on vertical or horizontal edges of the RV grid.
/// </summary>
public class BuildNode : AbstractBuildNode {
    //Michael

    //--------------------
    //  Public Variables
    //--------------------

    /// <summary>
    /// Filled with the wall prefab in the editor. (Deprecated)
    /// </summary>
    public GameObject wall;

    /// <summary>
    /// Boolean flag that indicates whether this node is on a horizontal edge. Necessary for getting the rotation correct. Ususally set in editor
    /// </summary>
    public bool isHorizontal;

    /// <summary>
    /// Boolean flag that indicates that this node can build weapons. Ususally set in editor
    /// </summary>
    public bool canPlaceWeapon = false;

    //public float height = 1f;

    //--------------------
    //  Private Variables
    //--------------------

    private Material outline;
    private GameObject holo;
    private GameObject item;

    /// <summary>
    /// Builds a copy of the given wall or weapon, if possible
    /// </summary>
    /// <param name="objectToPlace">The object to build</param>
    /// <param name="spawnNode">The node to spawn it at (Possibly deprecated)</param>
    public void Build(GameObject objectToPlace, GameObject spawnNode)
    {
        if (objectToPlace.tag == "Wall"){ //If object is a wall
            if (this.isHorizontal)
            {
                item = Instantiate(objectToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                item.transform.parent = spawnNode.transform;
                occupied = true;
            }
            else
            {
                item = Instantiate(objectToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
                item.transform.parent = spawnNode.transform;
                occupied = true;
            }

            SetOutlineActive(item, 0.0f);

            item.GetComponent<Wall>().myNode = gameObject;
        }
        else if(objectToPlace.tag == "Weapon" && canPlaceWeapon)
        {
            Vector3 dir = gameObject.transform.forward;
			item = Instantiate(objectToPlace, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.LookRotation(dir));
			item.transform.localScale = new Vector3(1.5f, 0.7f, 0.7f);
			item.transform.parent = spawnNode.transform;

			RemoveShow();	// get rid of holo when item is placed

			BoxCollider coll = item.GetComponentsInChildren<BoxCollider>()[1];
            coll.enabled = true;

            item.GetComponent<Weapon>().DisableNear();
            occupied = true;
            item.GetComponent<Weapon>().myNode = gameObject;
        }
        
    }

    /// <summary>
    /// Show a hologram of the given object
    /// </summary>
    /// <param name="makeHolo">THe object to show</param>
    public void Show(GameObject makeHolo){ //hologram function
        RemoveShow();
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

    /// <summary>
    /// Destroy the hologram, if any
    /// </summary>
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
