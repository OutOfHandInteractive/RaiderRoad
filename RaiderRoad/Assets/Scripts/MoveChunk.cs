using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChunk : MonoBehaviour {

    //--------------------
    // Public Variables
    //--------------------
    public GameObject spawnNode;

    //--------------------
    // Private Variables
    //--------------------
    private float speed;
    private GameObject spawner;
    private float spawnDespawnZDistance;
    private bool chunkSpawned = false;

    // Use this for initialization
    void Start() {
        spawner = GameObject.FindGameObjectWithTag("Road Spawner");
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (transform.position.z <= spawnDespawnZDistance && !chunkSpawned)
        {
            spawner.GetComponent<SpawnChunk>().Spawn(spawnNode.transform.position);
            chunkSpawned = true;
        }
        if (transform.position.z <= -spawnDespawnZDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void SetSpawnDespawnZDistance(float distance)
    {
        spawnDespawnZDistance = distance;
    }

    public Transform GetSpawnNode()
    {
        return spawnNode.transform;
    }
}
