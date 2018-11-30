using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackVehicle : MonoBehaviour{
    //Attack points and agent info
    private int attackPoints = 0;
    private List<Transform> attackList;
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
        attackList = new List<Transform>();
        cRb = rb;
        //Find random attack point
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
            attackList.Add(child);
        }
    }

     public void Attack()
     {
        //Stop if there is nothing to attack
        if (attackList.Count == 0)
             return;

         //Go to attack point
        cEnemy.SetDestination(attackList[attackPoints].position);
        attackPoints = Random.Range(0, attackList.Count);
        //Check if vehicle hit, add "knockback"
        if (cEnemy.remainingDistance < 1f)
        {
            hitCount++;
            cEnemy.SetDestination(attackPosition.transform.position);
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
            StartCoroutine(waitToLeave());
        }

        //If vehicle hit RV more than 5 times leave
        if (hitCount >= 5)
        {
            StartCoroutine(waitToLeave());
        }

    }
    IEnumerator waitToLeave()
    {
        cObject.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterLeave();

    }
    IEnumerator Knockback(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;
    }


}
