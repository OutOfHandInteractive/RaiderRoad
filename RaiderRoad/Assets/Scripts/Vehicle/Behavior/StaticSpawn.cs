using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpawn : MonoBehaviour {

    public VehicleFactoryManager factory;
    private VehicleFactoryManager.vehicleTypes types;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            types = (VehicleFactoryManager.vehicleTypes)Random.Range(0, 3);
            //Debug.Log(types);
            GameObject vehicle = factory.NewConstructVehicle(types,3, transform.position, 0.25f);  //3 = base medium threat, needed so this doesn't break, 0.25 = 1/4 chance of getting a weapon
            vehicle.transform.position = transform.position;
        }
        


    }
}
