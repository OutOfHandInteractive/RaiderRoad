using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnemy : JumpEnemy {

    //Gameobject, rigidbody, vehicle, initialangle for jump, if enemy jumped, current side 
    private Transform eVehicle;
    public override void StartJump(GameObject enemy, Rigidbody rb, string side)
    {
        base.StartJump(enemy, rb, side);
        Radio.GetRadio().CallForEvac(this);
    }

    public void RadioEvacCallback(StayVehicle vehicle)
    {
        cSide = vehicle.Side();
        eVehicle = vehicle.transform;
    }

    public void Escape()
    {
        // Wait to recieve vehicle
        if (eVehicle == null) {
            return;
        }
        //TODO: move to the same side as the vehicle
        //Enemy vehicle destination position
        Vector3 pos = eVehicle.position;
        float zSign = cSide.Equals("left") ? -1 : 1;
        Jump(pos, zSign);

        if(cObject.transform.parent.tag == "eVehicle")
        {
            eVehicle.GetComponent<VehicleAI>().EnterLeave();
        }
        //cObject.GetComponent<EnemyAI>().EnterWait();
    }
}
