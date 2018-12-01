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
    private Vector3 impact = Vector3.zero;
    private float mass;

    //Initialize agent and attack points
    public void StartAttack(NavMeshAgent agent, GameObject enemy, Rigidbody rb, string side)
    {
        cEnemy = agent;
        cObject = enemy;
        attackList = new List<Transform>();
        cRb = rb;
        mass = cRb.mass;
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
            //hitCount++;
            //cEnemy.SetDestination(attackPosition.transform.position);
            StartCoroutine(Knockback());
        }
        if (cObject.GetComponent<VehicleAI>().getState() == VehicleAI.State.Attack)
        {
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


    }
    IEnumerator waitToLeave()
    {
        cObject.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterLeave();

    }
    /*IEnumerator Knockback(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;
    }*/

    IEnumerator Knockback()
    {
        AddImpact(attackPosition.transform.position - cObject.transform.position, 5000f);
        Debug.Log(impact);
        if (impact.magnitude > 0.2F)
            cEnemy.speed = 60;
            cEnemy.destination = Vector3.Lerp(cObject.transform.position, impact, 1f); //Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        yield return new WaitForSeconds(1);
        hitCount++;
        cEnemy.speed = 20;
        cEnemy.SetDestination(attackList[attackPoints].position);
        //cObject.GetComponent<VehicleAI>().EnterWander();
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }

}
