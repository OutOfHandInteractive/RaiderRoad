using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnemy : JumpEnemy {

    //Gameobject, rigidbody, vehicle, initialangle for jump, if enemy jumped, current side 
    private Transform eVehicle;
    public override void StartJump(GameObject enemy, Rigidbody rb, string side)
    {
        base.StartJump(enemy, rb, side);
        //Initialize vehicle, enemy, rigidbody, side and angle for jumping
        eVehicle = GameObject.FindGameObjectWithTag("eVehicle").transform;
    }

    public void Escape()
    {

        //Enemy vehicle destination position
        Vector3 pos = eVehicle.position;
        float zSign = cSide.Equals("left") ? -1 : 1;
        Jump(pos, zSign);

        if(cObject.transform.root.tag == "eVehicle" && cObject.transform.parent != null)
        {
            eVehicle.GetComponent<VehicleAI>().EnterLeave();
        }
        //cObject.GetComponent<EnemyAI>().EnterWait();
    }
}
