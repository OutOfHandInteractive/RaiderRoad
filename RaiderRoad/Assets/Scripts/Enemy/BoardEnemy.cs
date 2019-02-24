﻿using System.Collections;
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
    //public override void StartJump(GameObject enemy, Rigidbody rb, string side, int stateChance)
    //{
    //    base.StartJump(enemy, rb, side, stateChance);
    //    //action = stateChance;
    //    //Set rv, enemy, rigidbody, current side, and angle to jump


    //}

    private Vector3 GetTarget(Vector3 planePos)
    {
        return Closest(planePos, GameObject.FindGameObjectsWithTag("floor")).transform.position;
    }

    /// <summary>
    /// Performs the boarding action then switches to the next state
    /// </summary>
    public void Board()
    {
        //RV destination position
        Vector3 planePos = new Vector3(cObject.transform.position.x, 0, cObject.transform.position.z);
        Vector3 pos = GetTarget(planePos);
        float zSign = cSide.Equals("left") ? 1 : -1;

        Jump(pos, zSign);

        //40% chance to go into Destroy State or Fight State, 20% to go into steal
        string actionStr = (action < 50) ? "EnterDestroy" : "EnterFight";
        StatefulEnemyAI ai = cObject.GetComponent<StatefulEnemyAI>();
        Transform parent = gameObject.transform.parent;
        if(parent != null && parent.tag == "RV")
        {
            survey += Time.deltaTime;
            Debug.Log(survey);
            if (survey > 1f)
            {
                if (action < 40)
                {
                    ai.EnterDestroy();
                }
                else if (action > 40 && action < 80)
                {
                    ai.EnterFight();
                }
                else
                {
                    ai.EnterSteal();
                }
            }
        }

        
    }
}
