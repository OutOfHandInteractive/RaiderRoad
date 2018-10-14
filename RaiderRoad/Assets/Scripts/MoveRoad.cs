using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour {
    //--------------------
    // Private Variables
    //--------------------
    private float speed;
    private GameObject spawner;
    private GameObject end;
    private Vector3 spawnLocation;
    private bool newSpawn = false;

    // Use this for initialization
    void Start() {
        spawner = GameObject.FindGameObjectWithTag("Road Spawner");
        end = transform.Find("End").gameObject;
        spawnLocation = spawner.transform.position;
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (end.transform.position.z <= spawnLocation.z && !newSpawn) {
            spawner.GetComponent<SpawnRoad>().Spawn();
            newSpawn = true;
        } else if (end.transform.position.z <= -50) {
            Destroy(gameObject);
        }

    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }
}
