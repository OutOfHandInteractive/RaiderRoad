using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnVehicle : MonoBehaviour {

    private List<Transform> spawnPoints;
    private int numPoints;
    public VehicleFactoryManager factory;
    private VehicleFactoryManager.vehicleTypes types;
    
    // Use this for initialization
    void Start()
    {
        spawnPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            Debug.Log(child);
            spawnPoints.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            numPoints = Random.Range(0, spawnPoints.Count);
            //Debug.Log(numPoints);
            types = (VehicleFactoryManager.vehicleTypes)Random.Range(0,3);
            //Debug.Log(types);
            GameObject vehicle = factory.newConstructVehicle(types);
            vehicle.transform.position = spawnPoints[numPoints].transform.position;
        }

    }
}
