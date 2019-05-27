using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class for vehicles in the wander state
/// </summary>
public class WanderVehicle : MonoBehaviour {
    //Patrols points and object agent
    private List<Transform> patrols;
    private GameObject patrolList;
    private int wanderPoints = 0;
    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private int action;
    private bool hasWeapon;
    private bool firstPos = false;
    private bool hasAttacked = false;

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="agent">The agent</param>
    /// <param name="enemy">The enemy</param>
    /// <param name="side">The side of the RV we're assigned to</param>
    /// <param name="weapon">Flag to indicate a weaponon board</param>
    public void StartWander(NavMeshAgent agent, GameObject enemy, VehicleAI.Side side, bool weapon)
    {
        //Set it to the VehicleAI
        cEnemy = agent;
        cObject = enemy;
        hasWeapon = weapon;
        action = Random.Range(0, 100);
        patrols = new List<Transform>();

        //Choose to patrol left or right, random chance
        if (side == VehicleAI.Side.Left)
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
            //Debug.Log(child);
            patrols.Add(child);
        }
        if(!firstPos)
        {
            wanderPoints = Random.Range(0, patrols.Count);
            firstPos = true;
        }

    }

    /// <summary>
    /// Do the wander action
    /// </summary>
    public void Wander()
    {
        //Return null if no patrol points
        if (patrols.Count == 0)
            return;
        float time = Mathf.SmoothStep(0, 1, 3*Time.deltaTime);
        //Have agent go to different points
        /*cEnemy.SetDestination(patrols[wanderPoints].position);
        //Choose random patrol point
        */
        if (Vector3.Distance(cObject.transform.position, patrols[wanderPoints].position) < 1f)
        {
            wanderPoints = Random.Range(0, patrols.Count);
            time = 0;
        }
        cObject.transform.position = Vector3.Lerp(cObject.transform.position, patrols[wanderPoints].position, time);
        if (!GetComponentInChildren<EnemyAI>() && !hasWeapon)
        {
            if (GetComponentInChildren<PlayerController_Rewired>() == null)
            {
                cObject.GetComponent<VehicleAI>().EnterLeave();
            }
        }

        if (cObject.GetComponentInChildren<PlayerController_Rewired>())
        {
            StartCoroutine(waitToLeaveWithPlayer());
        }
        //Chance to attack or chase the RV
        if (hasWeapon)
        {
            StartCoroutine(changeChase());
        }
        else if (Radio.GetRadio().checkState())
        {
            Debug.LogWarning("TRUEEEEEEEEEEE");
            cObject.GetComponent<VehicleAI>().EnterWander();
        }
        else if (!hasAttacked)
        {
            StartCoroutine(changeAttack());
            //StartCoroutine(waitToLeave());
        }

    }

    private IEnumerator changeAttack()
    {
        yield return new WaitForSeconds(10);
        hasAttacked = true;
        cObject.GetComponent<VehicleAI>().EnterAttack();
    }

    private IEnumerator changeChase()
    {
        yield return new WaitForSeconds(10);
        cObject.GetComponent<VehicleAI>().EnterChase();

    }
    private IEnumerator waitToLeave()
    {
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterLeave();

    }
    private IEnumerator waitToLeaveWithPlayer()
    {
        yield return new WaitForSeconds(3);
        cObject.GetComponent<VehicleAI>().EnterLeave();
    }
}
