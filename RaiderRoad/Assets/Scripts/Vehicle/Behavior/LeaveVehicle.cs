using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeaveVehicle : MonoBehaviour {
    //Exit point and agent info
    private Transform exit;
    private NavMeshAgent cEnemy;

    //Initialize exit point and agent
    public void StartLeave(NavMeshAgent agent)
    {
        exit = GameObject.Find("EnemyExit").transform;
        cEnemy = agent;
    }

    //Go to exit position
    public void Leave()
    {
        cEnemy.SetDestination(exit.position);
    }
}
