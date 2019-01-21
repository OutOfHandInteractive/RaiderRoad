using UnityEngine;
using System.Collections;

public abstract class DurableConstruct<N> : Constructable<N> where N : AbstractBuildNode
{
    public float durability; //original amount
    public float currDur = -1f; //current durability

    public override void OnStart()
    {
        if (currDur < 0f) currDur = durability; //if current durability was not assigned before start, assume full health
    }

    public override void OnDrop(GameObject item)
    {
        base.OnDrop(item);
        item.GetComponent<ItemDrop>().myItemDur = currDur; //give new drop item correct durabilty
    }

    public override void OnUpdate()
    {
        CheckDur();
    }

    private void CheckDur()
    {
        if (currDur <= 0f)
        {
            myNode.GetComponent<N>().occupied = false; // set node to unoccupied again
            Destroy(gameObject);
        }
    }

    public void DurabilityDamage(float damage)
    {
        currDur -= damage;
        //CheckDur();
    }

    public void SetDurability(float newDur)
    {
        currDur = newDur;
        //CheckDur();
    }

    public float GetDurability()
    {
        return currDur;
    }
}
