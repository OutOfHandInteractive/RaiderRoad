using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is for enemies trying to get off the RV. It inherits jump logic from JumpEnemy
/// </summary>
public class EscapeEnemy : JumpEnemy {

    //Gameobject, rigidbody, vehicle, initialangle for jump, if enemy jumped, current side 
    private GameObject eVehicle;
    private bool jumped = false;
    /// <summary>
    /// Initializes this state
    /// </summary>
    /// <param name="enemy">This enemy</param>
    /// <param name="rb">The rigid body</param>
    /// <param name="side">The side of the RV we're on</param>
    /// <param name="stateChance"></param>
    public override void StartJump(GameObject enemy, Rigidbody rb, VehicleAI.Side side,NavMeshAgent agent, int stateChance, VehicleAI _vehicle)
    {
        base.StartJump(enemy, rb, side, agent, stateChance, _vehicle);
        eVehicle = _vehicle.gameObject;
        Radio.GetRadio().CallForEvac(this);
        Debug.Log("I need evac!!");
    }

    /// <summary>
    /// This method is used by the radio to signal when a vehicle becomes available
    /// </summary>
    /// <param name="vehicle">The vehicle the radio found</param>
    public void RadioEvacCallback(StayVehicle vehicle)
    {
        if(IsNull(vehicle))
        {
            Debug.Log("Received null vehicle! " + vehicle.ToString());
            cObject.GetComponent<StatefulEnemyAI>().EnterFight();
        }
        else
        {
            cSide = vehicle.Side();
            //Debug.Log("Roger!");
            eVehicle = vehicle.GetObject();
            Debug.Log(eVehicle);
        }
    }

    /// <summary>
    /// Performs the escape actions
    /// </summary>
    public void Escape()
    {
        //Debug.Log(eVehicle);
        // Wait to recieve vehicle
        if (eVehicle == null) {

            //Todo enter fight function
            cObject.GetComponent<StatefulEnemyAI>().EnterFight();
            //return;
        }
        //TODO: move to the same side as the vehicle
        float movement = speed * Time.deltaTime;

        //If a reasonable jumping distance to vehicle, escape
        if (Vector3.Distance(cObject.transform.position, eVehicle.transform.position) < 4f)
        {
            //Enemy vehicle destination position
            //agent.enabled = false;
            Debug.LogWarning("REACHED");
            cObject.GetComponent<StatefulEnemyAI>().getAnimator().SetBool("Running", false);
            Vector3 pos = eVehicle.transform.position;
            float zSign = cSide == VehicleAI.Side.Left ? -1 : 1;
            if(!jumped)
            {
                Debug.LogWarning("HELLLO" + initialAngle);
                Jump(pos, zSign);
                jumped = true;
            }
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, eVehicle.transform.position, Time.deltaTime * 2);
        }
        else
        {
            Vector3 targetPosition = new Vector3(eVehicle.transform.position.x, cObject.transform.position.y, eVehicle.transform.position.z);
            cObject.transform.LookAt(targetPosition);
            //agent.SetDestination(targetPosition);
            //cObject.GetComponent<StatefulEnemyAI>().getAnimator().SetBool("Running", true);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, targetPosition, movement);
            agent.enabled = false;
        }
        //Debug.Log(cObject.transform.tag + " HEEEEEEEEY");
        if(cObject.transform.root.tag == "eVehicle" && cObject.transform.parent != null)
        {
            Debug.Log("HEYYYY");
            cObject.GetComponent<StatefulEnemyAI>().getAnimator().SetBool("Grounded", true);
            StartCoroutine(waitToLeave());
        }
        //cObject.GetComponent<EnemyAI>().EnterWait();
    }

    IEnumerator waitToLeave()
    {
        yield return new WaitForSeconds(5);
        eVehicle.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        eVehicle.GetComponent<VehicleAI>().EnterLeave();

    }
}
