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
        return Util.isEnemy(target);
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
