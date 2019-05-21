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

    /// <summary>
    /// Sets the speed of the chunk.
    /// </summary>
    /// <param name="speed">Chunk speed.</param>
    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    /// <summary>
    /// Sets the despawn distance of the chunk.
    /// </summary>
    /// <param name="distance">Z despawn distance.</param>
    public void SetSpawnDespawnZDistance(float distance)
    {
        spawnDespawnZDistance = distance;
    }

    /// <summary>
    /// Gets the spawn node of the chunk.
    /// </summary>
    /// <returns>the Transform of the spawn node.</returns>
    public Transform GetSpawnNode()
    {
        return spawnNode.transform;
    }
}
