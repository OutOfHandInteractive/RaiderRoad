using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnemy : JumpEnemy {

    //Gameobject, rigidbody, vehicle, initialangle for jump, if enemy jumped, current side 
    private GameObject eVehicle;
    public override void StartJump(GameObject enemy, Rigidbody rb, string side)
    {
        base.StartJump(enemy, rb, side);
        //Initialize vehicle, enemy, rigidbody, side and angle for jumping
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("eVehicle");
        eVehicle = Closest(enemy.transform.position, vehicles);
    }

    public void Escape()
    {

        //Enemy vehicle destination position
        Vector3 pos = eVehicle.transform.position;
        float zSign = cSide.Equals("left") ? -1 : 1;
        Jump(pos, zSign);

        if(cObject.transform.root.tag == "eVehicle" && cObject.transform.parent != null && cObject.GetComponent<EnemyAI>().GetState() != EnemyAI.State.Weapon)
        {
            eVehicle.GetComponent<VehicleAI>().EnterLeave();
        }
        //cObject.GetComponent<EnemyAI>().EnterWait();
    }
}
