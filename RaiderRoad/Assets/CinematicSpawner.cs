using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicSpawner : MonoBehaviour
{
    public List<GameObject> vehicles;
    public Vector2 spawnTimes;
    public float timer;

    void Start()
    {
        RollSpawn();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            int chosenVehicleNumber = Random.Range(0, vehicles.Count);
            Instantiate(vehicles[chosenVehicleNumber], gameObject.transform.position, gameObject.transform.rotation);
            RollSpawn();
        }

    }

    void RollSpawn()
    {
        timer = Random.Range(spawnTimes.x, spawnTimes.y);
    }
}
