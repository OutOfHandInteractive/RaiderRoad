using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackVehicle : MonoBehaviour{
    //Attack points and agent info
    private int wanderPoints = 0;
    private List<Transform> attackPoints;
    private NavMeshAgent cEnemy;
    private Rigidbody cRb;
    private GameObject cObject;
    private GameObject WallsRV;
    private GameObject attackPosition;
    private int hitCount = 0;
    private float timer = 0f;

    //Initialize agent and attack points
    public void StartAttack(NavMeshAgent agent, GameObject enemy, Rigidbody rb, string side)
    {
        cEnemy = agent;
        cObject = enemy;
        attackPoints = new List<Transform>();
        cRb = rb;
        //Find random attack point
        wanderPoints = Random.Range(0, attackPoints.Count);
        if (side.Equals("left"))
        {
            WallsRV = GameObject.Find("NodesLeft");
            attackPosition = GameObject.Find("AttackLeft");
        }
        else
        {
            WallsRV = GameObject.Find("NodesRight");
            attackPosition = GameObject.Find("AttackRight");
        }

        //Get all building points
        foreach (Transform child in WallsRV.transform)
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

        //Check if vehicle hit, add "knockback"
        if (cEnemy.remainingDistance < 1f)
        {
            hitCount++;
            cEnemy.transform.position = Vector3.Lerp(cEnemy.transform.position, attackPosition.transform.position, .2f);
        }
        //Increase time if state destination has not been reached
        if (cEnemy.pathPending)
        {
            timer += Time.deltaTime;
        }
        //Debug.Log(timer);
        //Leave if you can't enter state destination
        if (timer > 5)
        {
            cObject.GetComponent<VehicleAI>().EnterLeave();
        }

        //If vehicle hit RV more than 5 times leave
        if (hitCount >= 5)
        {
            cObject.GetComponent<VehicleAI>().EnterLeave();
        }

    }


}
