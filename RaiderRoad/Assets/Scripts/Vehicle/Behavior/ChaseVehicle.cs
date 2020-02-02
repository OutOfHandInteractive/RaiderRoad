using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class for vehicles in the chase state
/// </summary>
public class ChaseVehicle : MonoBehaviour {
    //Loading point location, agent info
    private List<Transform> attackList;
    private int attackPoints = 0;
    private GameObject WallsRV;
    private GameObject attackPosition;
    private Transform player;
    //private NavMeshAgent cEnemy;
    private GameObject cObject;
    private float timer = 0f;

    /// <summary>
    /// Initialize the state
    /// </summary>
    /// <param name="agent">The agent</param>
    /// <param name="enemy">The enemy</param>
    /// <param name="side">The side of the RV we're assigned to</param>
    public virtual void StartChase(NavMeshAgent agent, GameObject enemy, VehicleAI.Side side)
    {
        //cEnemy = agent;
        cObject = enemy;
        attackList = new List<Transform>();
        if (enemy.GetComponentInChildren<cannon>() != null)
        {
            if (side == VehicleAI.Side.Left)
            {
                WallsRV = GameObject.Find("CannonsLeft");
            }
            else
            {
                WallsRV = GameObject.Find("CannonsRight");
            }

        }
        else if (enemy.GetComponentInChildren<flamethrower>() != null)
        {
            if (side == VehicleAI.Side.Left)
            {
                WallsRV = GameObject.Find("FireLeft");
            }
            else
            {
                WallsRV = GameObject.Find("FireRight");
            }

        }

        //Get all building points
        foreach (Transform child in WallsRV.transform)
        {
            attackList.Add(child);
        }

        if (attackList.Count == 0)
            return;
        Debug.Log(attackList.Count);
        attackPoints = 1;

    }

    /// <summary>
    /// Do the chase action
    /// </summary>
    /// <param name="side">The side of the RV we're assigned to</param>
    public void Chase(VehicleAI.Side side)
    {
        //Stop completely when next to spot
        //cEnemy.autoBraking = true;
        //Randomly choose to load left or right side
        //Go to loading area
        float time = Mathf.SmoothStep(0, 1, 4 * Time.deltaTime);
        //Have agent go to different points
        /*cEnemy.SetDestination(patrols[wanderPoints].position);
        //Choose random patrol point
        */
        //cObject.transform.position = Vector3.Lerp(cObject.transform.position, attackList[attackPoints].position, time);

        //Increase time if state destination has not been reached

        if (cObject.GetComponent<VehicleAI>().getState() == VehicleAI.State.Chase)
        {
            /*if (cEnemy.remainingDistance > .1f)
            {
                timer += Time.deltaTime;
            }*/
            //Debug.Log(timer);
            //Leave if you can't enter state destination
            if (timer > 5)
            {
                cObject.GetComponent<VehicleAI>().EnterLeave();
            }
        }

        Invoke("WaitForNextState", 10f);
    }

    private void WaitForNextState()
    {
        StartCoroutine(waitToLeave());
    }

    private IEnumerator waitToLeave()
    {
        cObject.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterLeave();

    }
}
