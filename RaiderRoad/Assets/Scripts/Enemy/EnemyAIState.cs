using UnityEngine;
using System.Collections;

/// <summary>
/// This is a superclass for all raider AI states. It exposes a common interface that the state machine uses
/// </summary>
public abstract class EnemyAIState : EnemyAI
{
    protected StatefulEnemyAI master;

    /// <summary>
    /// By default AI states use the same speed as their controller
    /// </summary>
    /// <returns>The ams speed specified by the state machine</returns>
    public override float Speed()
    {
        return master.Speed();
    }

    /// <summary>
    /// Moves toward the given point.
    /// Simple method extension that sets the running flag on the animator
    /// </summary>
    /// <param name="target">The target to move towards</param>
    public override void MoveToward(Vector3 target)
    {
        base.MoveToward(target);
        master.getAnimator().Running = true;
    }

    /// <summary>
    /// Enter this state
    /// </summary>
    /// <param name="stateMachine">The state machine controlling this state</param>
    /// <param name="context">Any additional context information for the state</param>
    public void EnterState(StatefulEnemyAI stateMachine, StateContext context = null)
    {
        this.master = stateMachine;
        OnEnter(context);
    }

    /// <summary>
    /// Method called when leaving the state. Does nothing by default.
    /// </summary>
    /// <param name="context">Any additional context  infromation for the state</param>
    public virtual void LeaveState(StateContext context = null)
    {
        // Nothing by default
    }

    /// <summary>
    /// Method called when entering this state. Only called internally. Does nothing by default.
    /// </summary>
    /// <param name="context">Any additional context information for the state</param>
    protected virtual void OnEnter(StateContext context)
    {
        // Nothing by default
    }

    /// <summary>
    /// Method callback for entering collider tiggers. Called by the controller only when this is the current state. Does nothing by default
    /// </summary>
    /// <param name="other">The collider that caused the trigger</param>
    public virtual void TriggerEnter(Collider other)
    {
        // Nothing by default
    }

    /// <summary>
    /// Method callback for staying collider tiggers. Called by the controller only when this is the current state. Does nothing by default
    /// </summary>
    /// <param name="other">The collider that caused the trigger</param>
    public virtual void TriggerStay(Collider other)
    {
        // Nothing by default
    }

    /// <summary>
    /// Method callback for exiting collider tiggers. Called by the controller only when this is the current state. Does nothing by default
    /// </summary>
    /// <param name="other">The collider that caused the trigger</param>
    public virtual void TriggerExit(Collider other)
    {
        // Nothing by default
    }

    /// <summary>
    /// Update method. Called by the controller once per frame while this is the current state
    /// </summary>
    public abstract void UpdateState();

    /// <summary>
    /// Returns this state's color code
    /// </summary>
    /// <returns>A unique color for the state</returns>
    public abstract Color StateColor();

    /// <summary>
    /// Returns the state enum that corresponds with this state
    /// </summary>
    /// <returns>The state enum that corresponds with this state</returns>
    public abstract StatefulEnemyAI.State State();
}
