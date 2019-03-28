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

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        //eVehicle = master.Vehicle.gameObject;
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
            master.EnterFight();
        }
        else
        {
            cSide = vehicle.Side();
            //Debug.Log("Roger!");
            eVehicle = vehicle.gameObject;
            Debug.Log(eVehicle);
            if (master.GetState() == StatefulEnemyAI.State.Fight)
            {
                master.EnterEscape();
            }
        }
    }

    /// <summary>
    /// Performs the escape actions
    /// </summary>
    public override void UpdateState()
    {
        Debug.Log(eVehicle);
        // Wait to recieve vehicle
        if (eVehicle == null)
        {
            Radio.GetRadio().CallForEvac(this);
            Debug.Log("I need evac!!");
            // If we don't get an immediate response, start fighting
            if(eVehicle == null)
            {
                master.EnterFight();
                return;
            }
        }

        //If a reasonable jumping distance to vehicle, escape
        if (Vector3.Distance(transform.position, eVehicle.transform.position) < 2f)
        {
            //Enemy vehicle destination position
            agent.enabled = false;
            master.getAnimator().Running = false;
            Vector3 pos = eVehicle.transform.position;
            float zSign = cSide == VehicleAI.Side.Left ? -1 : 1;
            Jump(pos, zSign);
        }
        else
        {
            agent.enabled = true;
            Vector3 targetPosition = new Vector3(eVehicle.transform.position.x, gameObject.transform.position.y, eVehicle.transform.position.z);
            gameObject.transform.LookAt(targetPosition);
            agent.SetDestination(targetPosition);
            master.getAnimator().Running = true;
            //cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, eVehicle.transform.position, movement);
        }
        Debug.Log(gameObject.transform.tag + " HEEEEEEEEY");
        if(gameObject.transform.root.tag == "eVehicle" && gameObject.transform.parent != null)
        {
            Debug.Log("HEYYYY");
            master.getAnimator().Grounded = true;
            StartCoroutine(waitToLeave());
        }
        //cObject.GetComponent<EnemyAI>().EnterWait();
    }

    IEnumerator waitToLeave()
    {
        NavMeshAgent agent = eVehicle.GetComponent<NavMeshAgent>();
        if (agent.enabled)
        {
            agent.isStopped = false;
        }
        yield return new WaitForSeconds(5);
        eVehicle.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        eVehicle.GetComponent<VehicleAI>().EnterLeave();

    }

    public override Color StateColor()
    {
        return Color.blue;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Escape;
    }
}
