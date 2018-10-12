using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackVehicle : MonoBehaviour{
    //Attack points and agent info
    private int wanderPoints = 0;
    private List<Transform> attackPoints;
    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private GameObject floorsRV;

    //Initialize agent and attack points
    public void StartAttack(NavMeshAgent agent, GameObject enemy)
    {
        cEnemy = agent;
        cObject = enemy;
        attackPoints = new List<Transform>();
        floorsRV = GameObject.Find("FloorRV");
        //Get all building points
        foreach (Transform child in floorsRV.transform)
        {
            attackPoints.Add(child);
        }
    }

     public void Attack()
     {
        //Stop if there is nothing to attack
         if (attackPoints.Count == 0)
             return;

         //Go to attack point
         cEnemy.SetDestination(attackPoints[wanderPoints].position);
         
         //Find random attack point
         wanderPoints = Random.Range(0, attackPoints.Count);

         //Leave after 10 seconds
         cObject.GetComponent<VehicleAI>().Invoke("EnterLeave", 10f);
    }


}
