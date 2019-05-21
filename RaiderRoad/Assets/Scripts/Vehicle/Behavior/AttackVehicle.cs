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
    private bool hasHit = false;

    //Initialize agent and attack points
    public void StartAttack(NavMeshAgent agent, GameObject enemy, Rigidbody rb, VehicleAI.Side side)
    {
        cObject = enemy;
        attackList = new List<Transform>();
        cRb = rb;
        mass = cRb.mass;
        //Find random attack point
        if (side == VehicleAI.Side.Left)
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
        //Stop if there is nothing to attack
        if (attackList.Count == 0)
            return;
        //Debug.Log(attackList.Count);
        attackPoints = 1;// Random.Range(0, attackList.Count);
    }

     public void Attack()
     {
         //Go to attack point
        //cEnemy.SetDestination(attackList[attackPoints].position);
        float time = Mathf.SmoothStep(0, 1, 4 * Time.deltaTime);
        //Have agent go to different points
        /*cEnemy.SetDestination(patrols[wanderPoints].position);
        //Choose random patrol point
        */
        cObject.transform.position = Vector3.Lerp(cObject.transform.position, attackList[attackPoints].position, time);

        //Debug.Log(Vector3.Distance(cEnemy.transform.position, attackList[attackPoints].position));
        //Check if vehicle hit, add "knockback"
        if (Vector3.Distance(cObject.transform.position, attackList[attackPoints].position) < 1.1f && hasHit == false)
        {
            //hitCount++;
            //cEnemy.SetDestination(attackPosition.transform.position);
            //StartCoroutine(Knockback());
            Debug.Log("STAY STATE ACTIVE");
            GameObject.FindGameObjectWithTag("RV").GetComponent<rvHealth>().damagePOI(20f);
            cObject.GetComponent<VehicleAI>().EnterStay(attackPoints);
            hasHit = true;
        }


    }
    IEnumerator waitToLeave()
    {
        cEnemy.transform.parent = null;
        //cEnemy.transform.GetComponent<NavMeshAgent>().enabled = true;
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
            cEnemy.destination = Vector3.Lerp(cObject.transform.position, impact, 5f); //Time.deltaTime);
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
