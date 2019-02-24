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
    public void Build(GameObject objToBuild, float durability)
    {
        Vector3 dir = gameObject.transform.forward;
        item = Instantiate(objToBuild, new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.identity);
        item.transform.parent = this.gameObject.transform;
        Material tempMat = item.GetComponent<Renderer>().material;
        if(tempMat.shader.name == "Outlined/Uniform")
        {
            Material outline = Instantiate(tempMat);
            item.GetComponent<Renderer>().material = outline;
            outline.SetFloat("_Active", 0.0f);
        }

        occupied = true;
        item.GetComponent<DurableConstruct>().myNode = gameObject;
        item.GetComponent<DurableConstruct>().SetDurability(durability);
    }

    /// <summary>
    /// Shows a hologram of the given item
    /// </summary>
    /// <param name="objToShow"></param>
    public void Show(GameObject objToShow)
    { //hologram function, not efficient/working properly

        RemoveShow();
        holo = Instantiate(objToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        holo.GetComponent<DurableConstruct>().isHolo = true;
        holo.transform.parent = this.gameObject.transform;
    }

    /// <summary>
    /// Destroys the hologram, if any.
    /// </summary>
    public void RemoveShow()
    {
        Destroy(holo);
    }
}
