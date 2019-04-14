using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeaveVehicle : MonoBehaviour {
    //Exit point and agent info
    private Transform exit;
    private NavMeshAgent cEnemy;
    private GameObject cObject;

    //Initialize exit point and agent
    public void StartLeave(NavMeshAgent agent, GameObject enemy)
    {
        exit = GameObject.Find("EnemyExit").transform;
        cEnemy = agent;
        cObject = enemy;
    }

    //Go to exit position
    public void Leave()
    {
        cEnemy.radius = 5f;
        float time = Mathf.SmoothStep(0, 1, 2 * Time.deltaTime);
        //Have agent go to different points
        /*cEnemy.SetDestination(patrols[wanderPoints].position);
        //Choose random patrol point
        */
        cObject.transform.position = Vector3.Lerp(cObject.transform.position, exit.position, time);
        //cEnemy.SetDestination(exit.position);
    }


}
