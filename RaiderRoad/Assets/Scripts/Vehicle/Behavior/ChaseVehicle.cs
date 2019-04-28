using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    //Initialize agent
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

    }

    public void Chase(VehicleAI.Side side)
    {
        //Stop completely when next to spot
        //cEnemy.autoBraking = true;
        //Randomly choose to load left or right side
        //Go to loading area
        if (attackList.Count == 0)
            return;

        //cEnemy.SetDestination(attackList[attackPoints].position);
        attackPoints = Random.Range(0, attackList.Count);

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

    void WaitForNextState()
    {
        StartCoroutine(waitToLeave());
    }

    IEnumerator waitToLeave()
    {
        cObject.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterLeave();

    }
}
