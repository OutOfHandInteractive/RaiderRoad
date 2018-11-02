using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StayVehicle : MonoBehaviour {

    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private Transform player;
    private string cSide;
    private bool calledRadio = false;
    public void StartStay(NavMeshAgent agent, GameObject enemy)
    {
        cEnemy = agent;
        cObject = enemy;
    }

    public string Side()
    {
        return cSide;
    }

    private int CountEnemiesOnBoard()
    {
        int res = 0;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.transform.parent == cObject.transform)
            {
                res++;
            }
        }
        return res;
    }

    public void Stay(string side)
    {
        cSide = side;
        //Stop completely when next to spot
        cEnemy.autoBraking = true;

        //Randomly choose to load left or right side
        if (side.Equals("left"))
        {
            player = GameObject.FindGameObjectWithTag("EnemyL").transform;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("EnemyR").transform;
        }
        //Go to loading area
        cEnemy.SetDestination(player.transform.position);

        bool leave = false;
        if(!GameObject.FindGameObjectWithTag("Enemy"))
        {
            leave = true;
        }
        else if(cEnemy.remainingDistance < 0.5f)
        {
            // At loading area
            if (!calledRadio)
            {
                Radio.GetRadio().ReadyForEvac(this);
                calledRadio = true;
            }else if(CountEnemiesOnBoard() > 5) //TODO this limit should depend on size of vehicle
            {
                leave = true;
            }
        }
        if (leave)
        {
            if (calledRadio)
            {
                Radio.GetRadio().EvacLeaving(this);
            }
            cObject.GetComponent<VehicleAI>().EnterLeave();
        }
    }
}
