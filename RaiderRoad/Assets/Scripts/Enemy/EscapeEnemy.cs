using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnemy : JumpEnemy {

    //Gameobject, rigidbody, vehicle, initialangle for jump, if enemy jumped, current side 
    private GameObject eVehicle;
    public override void StartJump(GameObject enemy, Rigidbody rb, string side)
    {
        base.StartJump(enemy, rb, side);
        Radio.GetRadio().CallForEvac(this);
        Debug.Log("I need evac!!");
    }

    public void RadioEvacCallback(StayVehicle vehicle)
    {
        if(IsNull(vehicle))
        {
            Debug.Log("Received null vehicle! " + vehicle.ToString());
            return;
        }
        cSide = vehicle.Side();
        //Debug.Log("Roger!");
        eVehicle = vehicle.GetObject();
    }

    public void Escape()
    {
        // Wait to recieve vehicle
        if (eVehicle == null) {

            //Todo enter fight function
            //cObject.GetComponent<StatefulEnemyAI>().EnterFight();
            return;
        }
        //TODO: move to the same side as the vehicle
        float movement = speed * Time.deltaTime;

        //If a reasonable jumping distance to vehicle, escape
        if (Vector3.Distance(cObject.transform.position, eVehicle.transform.position) < 5f)
        {
            //Enemy vehicle destination position
            Vector3 pos = eVehicle.transform.position;
            float zSign = cSide.Equals("left") ? -1 : 1;
            Jump(pos, zSign);
        }
        else
        {
            Vector3 targetPosition = new Vector3(eVehicle.transform.position.x, cObject.transform.position.y, eVehicle.transform.position.z);
            cObject.transform.LookAt(targetPosition);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, eVehicle.transform.position, movement);
        }

        if(cObject.transform.root.tag == "eVehicle" && cObject.transform.parent != null && cObject.GetComponent<StatefulEnemyAI>().GetState() != StatefulEnemyAI.State.Weapon)
        {
            StartCoroutine(waitToLeave());
        }
        //cObject.GetComponent<EnemyAI>().EnterWait();
    }

    IEnumerator waitToLeave()
    {
        eVehicle.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        eVehicle.GetComponent<VehicleAI>().EnterLeave();

    }
}
