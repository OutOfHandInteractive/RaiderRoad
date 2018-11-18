using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StayVehicle : MonoBehaviour {

    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private Transform player;
    public void StartStay(NavMeshAgent agent, GameObject enemy)
    {
        cEnemy = agent;
        cObject = enemy;
    }

    public void Stay(string side)
    {
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

        if(!GameObject.FindGameObjectWithTag("Enemy") && GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>().GetState() != EnemyAI.State.Weapon)
        {
            cObject.GetComponent<VehicleAI>().EnterLeave();
        }
    }
}
