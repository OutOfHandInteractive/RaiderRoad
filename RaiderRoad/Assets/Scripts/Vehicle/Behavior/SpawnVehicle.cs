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
            //Get range of spawn, DO NOT GET 0 IT IS THE PARENT
            numPoints = Random.Range(1, spawnPoints.Count);
            Debug.Log(numPoints);
            //Debug.Log(numPoints);
            types = (VehicleFactoryManager.vehicleTypes)Random.Range(0,3);
            //Debug.Log(types);
            GameObject vehicle = factory.newConstructVehicle(VehicleFactoryManager.vehicleTypes.medium,3, spawnPoints[numPoints].transform.position);  //3 is the minimum threat for light vehicles - need for construction
            //GameObject vehicle = factory.newConstructVehicle(types);
            vehicle.GetComponent<VehicleAI>().setSide(spawnPoints[numPoints].name);
            vehicle.transform.position = spawnPoints[numPoints].transform.position;
        }

    }
}
