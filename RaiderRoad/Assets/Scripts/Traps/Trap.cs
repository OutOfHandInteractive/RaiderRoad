using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : DurableConstruct<TrapNode>
{
    public float cooldownTime;

    private List<Collider> colliders = new List<Collider>();
    private float cooldownRemaining = 0;

    public override void OnUpdate()
    {
        base.OnUpdate();
        cooldownRemaining = Mathf.Max(0, cooldownRemaining - Time.deltaTime);
    }

    private void CheckTrap()
    {
        if(isHolo || cooldownRemaining > 0 || !isPlaced())
        {
            return;
        }
        //Debug.Log("Checking " + colliders.Count + " colliders");
        bool activated = false;
        Util.RemoveNulls(colliders);
        foreach (Collider other in colliders)
        {
            if (other == null)
            {
                continue;
            }
            GameObject target = other.gameObject;
            //Debug.Log("Collider object tag: " + target.tag);
            Activate(target);
            activated = true;
        }
        if (activated)
        {
            DurabilityDamage(1.0f);
            cooldownRemaining = cooldownTime;
        }
    }

    public virtual bool CanTarget(GameObject target)
    {
        return Util.isEnemy(target);
    }

    public abstract void Activate(GameObject victim);

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider Entered");
        if (!isHolo && CanTarget(other.gameObject))
        {
            colliders.Add(other);
            CheckTrap();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collider Exited");
        colliders.Remove(other);
    }
}
