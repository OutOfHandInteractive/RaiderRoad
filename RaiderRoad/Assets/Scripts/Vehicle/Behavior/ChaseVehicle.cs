using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseVehicle : MonoBehaviour {
    //Loading point location, agent info
    private Transform player;
    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private float timer = 0f;
    //Initialize agent
    public virtual void StartChase(NavMeshAgent agent, GameObject enemy)
    {
        cEnemy = agent;
        cObject = enemy;
    }

    public void Chase(string side)
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

        //Increase time if state destination has not been reached
        if(cEnemy.remainingDistance > .1f)
        {
            timer += Time.deltaTime;
        }
        //Debug.Log(timer);
        //Leave if you can't enter state destination
        if (timer > 5)
        {
            cObject.GetComponent<VehicleAI>().EnterLeave();
        }

        //Enter stay if you get to the loading position
        if(!cEnemy.pathPending && cEnemy.remainingDistance < .01f)
        {
            cObject.GetComponent<VehicleAI>().EnterStay();
        }

    }
}
