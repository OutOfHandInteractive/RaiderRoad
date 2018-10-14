using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVehicle : MonoBehaviour {

    private System.Random random = new System.Random();
    public VehicleFactoryManager factory;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            factory.ConstructVehicle(VehicleFactoryManager.vehicleTypes.light);
        }

    }
}
