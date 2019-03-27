using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// This state is for enemies caught by the lure trap.
/// </summary>
public class LureEnemy : EnemyAIState
{
    public struct LureContext : StateContext
    {
        public StatefulEnemyAI.State prevState;

        public LureContext(StatefulEnemyAI.State prevState) : this()
        {
            this.prevState = prevState;
        }
    }

    private HashSet<GameObject> lures = new HashSet<GameObject>();
    private StatefulEnemyAI.State prev;

    /// <summary>
    /// Adds the given lure to the internal set. This enemy will be attracted to the lure as long as it exists and the enmy is in this state.
    /// </summary>
    /// <param name="trap">The lure trap to add</param>
    public void AddLure(LureTrap trap)
    {
        lures.Add(trap.gameObject);
    }

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        if(context is LureContext)
        {
            prev = ((LureContext)context).prevState;
        }
        else
        {
            throw new Exception("Lure state must be given a LureContext");
        }
    }

    public override void UpdateState()
    {
        Util.RemoveNulls(lures);
        if(lures.Count <= 0)
        {
            master.EnterState(prev);
        }
        else
        {
            GameObject[] gameObjects = new GameObject[lures.Count];
            lures.CopyTo(gameObjects);
            MoveToward(Closest(gameObjects));
        }

    }

    public override Color StateColor()
    {
        return Color.cyan;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Lure;
    }
}
