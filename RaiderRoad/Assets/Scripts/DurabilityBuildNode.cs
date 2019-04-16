using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for build nodes that construct items with durability
/// </summary>
public abstract class DurabilityBuildNode : AbstractBuildNode
{


    private GameObject holo;
    private GameObject item;

    /// <summary>
    /// Builds a copy of the given item with the given durability. Disables the outline in the shader if applicable.
    /// </summary>
    /// <param name="objToBuild">The object to build</param>
    /// <param name="durability">The durability to use</param>
    /// <returns>The built object</returns>
    public GameObject Build(GameObject objToBuild, float durability)
    {
        Vector3 dir = gameObject.transform.forward;
        item = Instantiate(objToBuild, new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.identity);
        item.transform.parent = this.gameObject.transform;
        item.transform.forward = this.gameObject.transform.forward;
        item.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        SetOutlineActive(item, 0.0f);

        occupied = true;
        DurableConstruct construct = item.GetComponent<DurableConstruct>();
        construct.myNode = gameObject;
        construct.SetDurability(durability);
        RemoveShow();
        return item;
    }

    /// <summary>
    /// Shows a hologram of the given item
    /// </summary>
    /// <param name="objToShow"></param>
    /// <returns>The hologram</returns>
    public GameObject Show(GameObject objToShow)
    { //hologram function, not efficient/working properly
        RemoveShow();
        holo = Instantiate(objToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        holo.GetComponent<DurableConstruct>().isHolo = true;
        holo.transform.parent = this.gameObject.transform;
        return holo;
    }

    /// <summary>
    /// Destroys the hologram, if any.
    /// </summary>
    public void RemoveShow()
    {
        Destroy(holo);
    }
}
