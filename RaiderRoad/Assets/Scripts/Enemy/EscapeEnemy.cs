﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnemy : JumpEnemy {

    //Gameobject, rigidbody, vehicle, initialangle for jump, if enemy jumped, current side 
    private Transform eVehicle;
    public override void StartJump(GameObject enemy, Rigidbody rb, string side)
    {
        base.StartJump(enemy, rb, side);
        Radio.GetRadio().CallForEvac(this);
        Debug.Log("I need evac!!");
    }

    public void RadioEvacCallback(StayVehicle vehicle)
    {
        cSide = vehicle.Side();
        Debug.Log("Roger!");
        eVehicle = vehicle.transform;
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
        cObject.transform.LookAt(eVehicle.transform);
        cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, eVehicle.transform.position, movement);
        //If a reasonable jumping distance to vehicle, escape
        if (Vector3.Distance(cObject.transform.position, eVehicle.transform.position) < 5f)
        {
            //Enemy vehicle destination position
            Vector3 pos = eVehicle.position;
            float zSign = cSide.Equals("left") ? -1 : 1;
            Jump(pos, zSign);
        }


        if(cObject.transform.parent.tag == "eVehicle" && cObject.transform.parent != null)
        {
            eVehicle.GetComponent<VehicleAI>().EnterLeave();
        }
        //cObject.GetComponent<EnemyAI>().EnterWait();
    }
}
