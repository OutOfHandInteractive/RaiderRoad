using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedEnemy : EnemyAIState
{
    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Stunned;
    }

    public override Color StateColor()
    {
        return Color.black;
    }

    public override void UpdateState()
    {
        // Nothing
    }
}
