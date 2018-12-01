using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderVehicle : MonoBehaviour {
    //Patrols points and object agent
    private List<Transform> patrols;
    private GameObject patrolList;
    private int wanderPoints = 0;
    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private int action = Random.Range(0, 100);
    private bool hasWeapon;
    public void StartWander(NavMeshAgent agent, GameObject enemy, string side, bool weapon)
    {
        //Set it to the VehicleAI
        cEnemy = agent;
        cObject = enemy;
        cEnemy.speed = 20;
        hasWeapon = weapon;
        patrols = new List<Transform>();

        //Choose to patrol left or right, random chance
        if (side.Equals("left"))
        {
            patrolList = GameObject.Find("Patrol Left");
        }
        else
        {
            patrolList = GameObject.Find("Patrol Right");
        }
        //Get the patrol points
        foreach (Transform child in patrolList.transform)
        {
            Debug.Log(child);
            patrols.Add(child);
        }
    }

    public void Wander()
    {
        //Return null if no patrol points
        if (patrols.Count == 0)
            return;
        //Have agent go to different points
        cEnemy.SetDestination(patrols[wanderPoints].position);
        //Choose random patrol point
        wanderPoints = Random.Range(0, patrols.Count);

        //Chance to attack or chase the RV
        if (hasWeapon)
        {
            cObject.GetComponent<VehicleAI>().Invoke("EnterChase", 10f);
        }
        else
        {
            if (action < 50)
            {
                cObject.GetComponent<VehicleAI>().Invoke("EnterChase", 10f);
            }
            else
            {
                cObject.GetComponent<VehicleAI>().Invoke("EnterAttack", 10f);
            }
        }

    }
}
