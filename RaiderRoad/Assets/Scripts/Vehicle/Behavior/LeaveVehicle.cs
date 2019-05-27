using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class for vehicles that are leaving the scene
/// </summary>
public class LeaveVehicle : MonoBehaviour {
    //Exit point and agent info
    private Transform exit;
    private NavMeshAgent cEnemy;
    private GameObject cObject;

    /// <summary>
    /// Initialize exit point and agent
    /// </summary>
    /// <param name="agent">The agent</param>
    /// <param name="enemy">The enemy</param>
    public void StartLeave(NavMeshAgent agent, GameObject enemy)
    {
        exit = GameObject.Find("EnemyExit").transform;
        cEnemy = agent;
        cObject = enemy;
        //Destroy(cObject.GetComponent<WanderVehicle>());
        //Destroy(cObject.GetComponent<ChaseVehicle>());
        //Destroy(cObject.GetComponent<AttackVehicle>());
        //Destroy(cObject.GetComponent<StayVehicle>());
        //Destroy(cObject.GetComponent<WaitVehicle>());
    }

    /// <summary>
    /// Go to exit position
    /// </summary>
    public void Leave()
    {
        //cEnemy.radius = 5f;
        float time = Mathf.SmoothStep(0, 1, 10 * Time.deltaTime);
        //Have agent go to different points
        /*cEnemy.SetDestination(patrols[wanderPoints].position);
        //Choose random patrol point
        */
        cObject.transform.Translate(Vector3.forward * time);
        //cObject.transform.position = Vector3.Lerp(cObject.transform.position, exit.position, time);
        //cEnemy.SetDestination(exit.position);
    }


}
