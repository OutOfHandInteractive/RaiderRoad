using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for lure traps. They attract enemies in until the trap wears down and disappears.
/// Takes durability damage each frame equal to the time delta in seconds (e.g. a lure trap with durability 10 will last 10 seconds).
/// </summary>
public class LureTrap : Trap
{
    /// <summary>
    /// Update hook to take time based damage
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isPlaced())
        {
            DurabilityDamage(Time.deltaTime);
        }
    }

    /// <summary>
    /// Custom targeting logic. Only targets enemies (naturally) that aren't in the Wait, WeaponAttack, or Lure states.
    /// </summary>
    /// <param name="target">The target object</param>
    /// <returns></returns>
    public override bool CanTarget(GameObject target)
    {
        if (Util.isEnemy(target))
        {
            StatefulEnemyAI.State state = target.GetComponent<StatefulEnemyAI>().GetState();
            return state != StatefulEnemyAI.State.Wait && state != StatefulEnemyAI.State.Weapon && state != StatefulEnemyAI.State.Lure;
        }
        return false;
    }

    /// <summary>
    /// On activation, this trap puts the enemy into the Lure state
    /// </summary>
    /// <param name="victim">The enemy to lure</param>
    public override void Activate(GameObject victim)
    {
        victim.GetComponent<LureEnemy>().AddLure(this);
        victim.GetComponent<StatefulEnemyAI>().EnterLure();
    }

    public override void OnBreak()
    {
        // Nothing!
    }
}
