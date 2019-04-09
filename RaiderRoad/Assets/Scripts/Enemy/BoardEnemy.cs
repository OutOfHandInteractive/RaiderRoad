using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is for when the enemy jumps onto the RV. Inherits jump logic from JumpEnemy
/// </summary>
public class BoardEnemy : JumpEnemy {

    //enemy, rigidbody,rv, angle to jump, if enemy jumped, chance to take action, current side 
    //private int action;

    private float survey = 0;
    private Transform parent = null;
    //public override void StartJump(GameObject enemy, Rigidbody rb, string side, int stateChance)
    //{
    //    base.StartJump(enemy, rb, side, stateChance);
    //    //action = stateChance;
    //    //Set rv, enemy, rigidbody, current side, and angle to jump


    //}

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
    public void Board()
    {
        //agent.enabled = true;
        //RV destination position
        Vector3 planePos = new Vector3(cObject.transform.position.x, 0, cObject.transform.position.z);
        Vector3 pos = GetTarget(planePos);
        float zSign = cSide == VehicleAI.Side.Left ? 1 : -1;
        Debug.Log(zSign + " THIS IS THE SIGN");
        Jump(pos, zSign);
        
        

        //40% chance to go into Destroy State or Fight State, 20% to go into steal
        string actionStr = (action < 50) ? "EnterDestroy" : "EnterFight";
        StatefulEnemyAI ai = cObject.GetComponent<StatefulEnemyAI>();
        if(transform.parent != null)
        {
            if(transform.parent.tag == "RV")
            {
                ai.getAnimator().SetBool("Grounded", true);
                //agent.speed = 0;
                //agent.isStopped = true;
                survey += Time.deltaTime;
                Debug.Log(survey);
                if (survey > 1f)
                {
                    if (action < 40)
                    {
                        //agent.speed = speed;
                        ai.EnterDestroy();
                    }
                    else if (action > 40 && action < 80)
                    {
                        //agent.speed = speed;
                        ai.EnterFight();
                    }
                    else
                    {
                        //agent.speed = speed;
                        ai.EnterSteal();
                    }
                }
            }
        }
        else if (transform.parent == null)
        {
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, pos, Time.deltaTime * .5f);
        }

        
    }
}
