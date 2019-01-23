using UnityEngine;
using System.Collections;

public class LureTrap : Trap
{

    public override void OnUpdate()
    {
        base.OnUpdate();
        DurabilityDamage(Time.deltaTime);
    }

    public override bool CanTarget(GameObject target)
    {
        if (Util.isEnemy(target))
        {
            StatefulEnemyAI.State state = target.GetComponent<StatefulEnemyAI>().GetState();
            return state != StatefulEnemyAI.State.Wait && state != StatefulEnemyAI.State.Weapon;
        }
        return false;
    }

    public override void Activate(GameObject victim)
    {
        victim.GetComponent<LureEnemy>().AddLure(this);
        victim.GetComponent<StatefulEnemyAI>().EnterStateIfNotAlready(StatefulEnemyAI.State.Wait);
    }

    public override void OnBreak()
    {
        // Nothing!
    }
}
