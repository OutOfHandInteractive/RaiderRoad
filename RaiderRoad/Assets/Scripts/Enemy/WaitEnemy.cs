using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEnemy : MonoBehaviour {
    //Current game object
    private GameObject cObject;
    private VehicleAI cVehicle;
    //Set enemy to this script
    public void StartWait(GameObject enemy, VehicleAI vehicle)
    {
        cObject = enemy;
        cVehicle = vehicle;
    }

    public void Wait()
    {
        //Enter board state after 15 seconds
        if (cVehicle.getState() == VehicleAI.State.Stay)
            cObject.GetComponent<EnemyAI>().EnterBoard();
    }
}
