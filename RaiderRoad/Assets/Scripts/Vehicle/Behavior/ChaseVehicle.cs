using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseVehicle : MonoBehaviour {
    //Loading point location, agent info
    private Transform player;
    private NavMeshAgent cEnemy;
    private GameObject cObject;

    //Initialize agent
    public void StartChase(NavMeshAgent agent, GameObject enemy)
    {
        cEnemy = agent;
        cObject = enemy;
    }

    public void Chase()
    {
        //Stop completely when next to spot
        cEnemy.autoBraking = true;

        //Randomly choose to load left or right side
        int action = Random.Range(0, 100);
        if (action < 50)
        {
            player = GameObject.FindGameObjectWithTag("EnemyL").transform;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("EnemyR").transform;
        }
        //Go to loading area
        cEnemy.SetDestination(player.transform.position);

        //Leave after 10 seconds
        cObject.GetComponent<VehicleAI>().Invoke("EnterLeave", 10f);
    }
}
