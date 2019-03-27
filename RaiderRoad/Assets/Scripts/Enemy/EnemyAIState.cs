using UnityEngine;
using System.Collections;

public abstract class EnemyAIState : EnemyAI
{
    protected StatefulEnemyAI master;

    public void EnterState(StatefulEnemyAI stateMachine, StateContext context = null)
    {
        this.master = stateMachine;
        OnEnter(context);
    }

    public virtual void LeaveState(StateContext context = null)
    {
        // Nothing by default
    }

    protected virtual void OnEnter(StateContext context)
    {
        // Nothing by default
    }

    public virtual void TriggerEnter(Collider other)
    {
        // Nothing by default
    }

    public virtual void TriggerStay(Collider other)
    {
        // Nothing by default
    }

    public virtual void TriggerExit(Collider other)
    {
        // Nothing by default
    }

    public abstract void UpdateState();

    public abstract Color StateColor();

    public abstract StatefulEnemyAI.State State();
}
