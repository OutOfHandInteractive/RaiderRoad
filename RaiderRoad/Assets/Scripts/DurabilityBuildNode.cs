using UnityEngine;
using System.Collections;

public abstract class DurabilityBuildNode : AbstractBuildNode
{
    private GameObject holo;
    private GameObject item;

    public void Build(GameObject objToBuild, float durability)
    {
        Vector3 dir = gameObject.transform.forward;
        item = Instantiate(objToBuild, new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.identity);
        item.transform.parent = this.gameObject.transform;

        occupied = true;
        item.GetComponent<DurableConstruct>().myNode = gameObject;
        item.GetComponent<DurableConstruct>().SetDurability(durability);
    }

    public void Show(GameObject objToShow)
    { //hologram function, not efficient/working properly

        holo = Instantiate(objToShow, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        holo.GetComponent<DurableConstruct>().isHolo = true;
        holo.transform.parent = this.gameObject.transform;
    }

    public void RemoveShow()
    {
        Destroy(holo);
    }
}
