using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : Constructable<TrapNode>
{
    public float cooldownTime;

    private List<Collider> colliders = new List<Collider>();
    private float cooldownRemaining = 0;

    public override void OnBreak()
    {
        // Do nothing
    }

    public override void OnStart()
    {
        
    }

    public override void OnUpdate()
    {
        cooldownRemaining = Mathf.Max(0, cooldownRemaining - Time.deltaTime);
    }

    private void CheckTrap()
    {
        if(isHolo || cooldownRemaining > 0)
        {
            return;
        }
        Debug.Log("Checking " + colliders.Count + " colliders");
        bool activated = false;
        foreach (Collider other in colliders)
        {
            if (other == null)
            {
                colliders.Remove(other);
                continue;
            }
            GameObject target = other.gameObject;
            Debug.Log("Collider object tag: " + target.tag);
            if (target.tag == "Enemy" || target.tag == "Player")
            {
                Activate(target);
                activated = true;
            }
            else
            {
                colliders.Remove(other);
            }
        }
        if (activated)
        {
            //TODO
            cooldownRemaining = cooldownTime;
        }
    }

    public abstract void Activate(GameObject victim);

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider Entered");
        if (!isHolo)
        {
            colliders.Add(other);
            CheckTrap();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CheckTrap();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collider Exited");
        colliders.Remove(other);
    }
}
