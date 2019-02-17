using UnityEngine;
using System.Collections;

public abstract class DurableConstruct : Constructable
{
    public float durability; //original amount
    
    [SerializeField] protected float currDur = -1f; //current durability

    public override void OnStart()
    {
        if (currDur < 0f) currDur = durability; //if current durability was not assigned before start, assume full health
    }

    public override void OnDrop(GameObject item)
    {
        base.OnDrop(item);
        item.GetComponent<ItemDrop>().myItemDur = currDur; //give new drop item correct durabilty
    }

    //public override void OnUpdate()
    //{
    //    //CheckDur();
    //}

    private void CheckDur()
    {
        if(currDur > durability)
        {
            Debug.LogError("WTF, durability should never be that high: " + currDur);
        }
        if (isPlaced() && currDur <= 0f)
        {
            GetNodeComp(myNode).occupied = false; // set node to unoccupied again
            Destroy(gameObject);
        }
    }

    public void DurabilityDamage(float damage)
    {
        currDur -= damage;
        CheckDur();
    }

    public void SetDurability(float newDur)
    {
        currDur = newDur;
        CheckDur();
    }

    public float GetDurability()
    {
        return currDur;
    }
}

public abstract class DurableConstruct<N> : DurableConstruct where N : AbstractBuildNode
{
    protected override AbstractBuildNode GetNodeComp(GameObject myNode)
    {
        return myNode.GetComponent<N>();
    }
}
