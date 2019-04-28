using UnityEngine;
using System.Collections;

/// <summary>
/// Class for constructables that have a persistent durability. The durability will persist even if the item is broken and replaced. When the durability wears out the item will be destroyed.
/// </summary>
public abstract class DurableConstruct : Constructable
{
    /// <summary>
    /// The original (max) durability. Usually set in the prefab
    /// </summary>
    public float durability;
    
    /// <summary>
    /// The current durability of the item.
    /// </summary>
    [SerializeField] protected float currDur = -1f;

    /// <summary>
    /// Hook into Start() to initialize the durability. Will set it to max if currDur is invalid (less than 0)
    /// </summary>
    public override void OnStart()
    {
        if (currDur < 0f) currDur = durability; //if current durability was not assigned before start, assume full health
    }

    /// <summary>
    /// Hook that sets the durability of the drop so it can be persisted.
    /// </summary>
    /// <param name="item">The drop being created</param>
    public override void OnDrop(GameObject item)
    {
        base.OnDrop(item);
        item.GetComponent<ItemDrop>().myItemDur = currDur; //give new drop item correct durabilty
    }
    
    public virtual void CheckDur() //originally not public or virtual (needed to change to be overrided in engine script)
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

    public float currentDurVal()
    {
        return currDur;
    }

    /// <summary>
    /// Subtract from the current durability.
    /// </summary>
    /// <param name="damage">The amount of damage to take</param>
    public void DurabilityDamage(float damage)
    {
        currDur -= damage;
        CheckDur();
    }

    /// <summary>
    /// Set the current durability
    /// </summary>
    /// <param name="newDur">The new durability</param>
    public void SetDurability(float newDur)
    {
        currDur = newDur;
        CheckDur();
    }

    /// <summary>
    /// Get the current durability
    /// </summary>
    /// <returns>The current durability</returns>
    public float GetDurability()
    {
        return currDur;
    }
}

/// <summary>
/// Abstract version of DurableConstruct. Useful for type safety.
/// </summary>
/// <typeparam name="N">The type of node this construct needs to be built from</typeparam>
public abstract class DurableConstructGen<N> : DurableConstruct where N : AbstractBuildNode
{
    /// <summary>
    /// Fetches the correct type of node from the given object.
    /// </summary>
    /// <param name="myNode">The object to look inside</param>
    /// <returns>The appropriate node. Guaranteed to be of type N</returns>
    protected override AbstractBuildNode GetNodeComp(GameObject myNode)
    {
        return myNode.GetComponent<N>();
    }
}
