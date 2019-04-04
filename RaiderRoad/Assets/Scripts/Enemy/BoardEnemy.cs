using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is for when the enemy jumps onto the RV. Inherits jump logic from JumpEnemy
/// </summary>
public class BoardEnemy : JumpEnemy {

    private float survey = 0;

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
    }

    private Vector3 GetTarget(Vector3 planePos)
    {
        Debug.Log(cSide);
        if (cSide == VehicleAI.Side.Left)
        {
            return Closest(planePos, GameObject.FindGameObjectsWithTag("JumpL")).transform.position;
        }
        else if (cSide == VehicleAI.Side.Right)
        {
            return Closest(planePos, GameObject.FindGameObjectsWithTag("JumpR")).transform.position;
        }
        return new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Performs the boarding action then switches to the next state
    /// </summary>
    public override void UpdateState()
    {
        //agent.enabled = true;
        //RV destination position
        Vector3 planePos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        Vector3 pos = GetTarget(planePos);
        float zSign = cSide == VehicleAI.Side.Left ? 1 : -1;
        Debug.Log(zSign + " THIS IS THE SIGN");
        Jump(pos, zSign);

        //40% chance to go into Destroy State or Fight State, 20% to go into steal
        //string actionStr = (action < 50) ? "EnterDestroy" : "EnterFight";
        StatefulEnemyAI ai = gameObject.GetComponent<StatefulEnemyAI>();
        if (transform.parent != null && transform.parent.tag == "RV")
        {
            ai.getAnimator().Grounded = true;
            agent.velocity = Vector3.zero;
            //agent.isStopped = true;
            survey += Time.deltaTime;
            Debug.Log(survey);
            if (survey > 1f)
            {
                master.EnterState(master.myType.BoardingChooser().Choose());
            }
        }
    }

    public override Color StateColor()
    {
        return Color.green;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Board;
    }
}
