using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for all traps. Implements cooldown and selective targeting
/// </summary>
public abstract class Trap : DurableConstructGen<TrapNode>
{
    /// <summary>
    /// The time it takes for the trap to reset (in seconds)
    /// </summary>
    public float cooldownTime;

    private List<Collider> colliders = new List<Collider>();
    private float cooldownRemaining = 0;

    /// <summary>
    /// Update hook to update the cooldown
    /// </summary>
    public override void OnUpdate()
    {
        cooldownRemaining = Mathf.Max(0, cooldownRemaining - Time.deltaTime);
        CheckTrap();
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

    /// <summary>
    /// Overridable method for selecting targets. By default it selects only enemies
    /// </summary>
    /// <param name="target">The target to check</param>
    /// <returns>True if and only if the trap should target them</returns>
    public virtual bool CanTarget(GameObject target)
    {
        return Util.isEnemy(target);
    }

    /// <summary>
    /// Abstract method for the implementer to fill with whatever it is their trap does. Called for each applicable target
    /// </summary>
    /// <param name="victim">The enemy to target</param>
    public abstract void Activate(GameObject victim);

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collider Entered");
        if (!isHolo && CanTarget(other.gameObject))
        {
            colliders.Add(other);
            CheckTrap();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collider Exited");
        colliders.Remove(other);
    }
}
