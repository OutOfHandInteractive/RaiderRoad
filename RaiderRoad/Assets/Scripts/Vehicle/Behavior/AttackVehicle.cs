﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackVehicle : MonoBehaviour{
    //Attack points and agent info
    private int wanderPoints = 0;
    private List<Transform> attackPoints;
    private NavMeshAgent cEnemy;
    private Rigidbody cRb;
    private GameObject cObject;
    private GameObject WallsRV;
    private int hitCount = 0;

    //Initialize agent and attack points
    public void StartAttack(NavMeshAgent agent, GameObject enemy, Rigidbody rb, string side)
    {
        cEnemy = agent;
        cObject = enemy;
        attackPoints = new List<Transform>();
        cRb = rb;
        if(side.Equals("left"))
        {
            WallsRV = GameObject.Find("NodesLeft");
        }
        else
        {
            WallsRV = GameObject.Find("NodesRight");
        }

        //Get all building points
        foreach (Transform child in WallsRV.transform)
        {
            attackPoints.Add(child);
        }
    }

     public void Attack()
     {
        //Stop if there is nothing to attack
        if (attackPoints.Count == 0)
             return;

         //Go to attack point
         cEnemy.SetDestination(attackPoints[wanderPoints].position);
        //Find random attack point
        wanderPoints = Random.Range(0, attackPoints.Count);
        if (cEnemy.remainingDistance < 1f)
        {
            hitCount++;
        }

        if(hitCount >= 5)
        {
            cObject.GetComponent<VehicleAI>().EnterLeave();
        }

    }


}
