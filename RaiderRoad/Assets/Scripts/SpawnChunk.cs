using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChunk : MonoBehaviour {
    //--------------------
    // Public Variables
    //--------------------
    public List<GameObject> roadChunks;
    public bool randomChunk = false;
    public float speed = -35.0f;
    public GameObject warningSprite;
    public Transform warningCamvas;
    public GameObject rv;
    public float spawnDespawnZDistance = 100.0f;

    //--------------------
    // Private Variables
    //--------------------
    private int nextChunk;
    private List<GameObject> warnings = new List<GameObject>();
    private float rvZStart;
    private BoxCollider col;

    // Use this for initialization
    void Start () {
        rvZStart = rv.transform.position.z;
        col = gameObject.GetComponent<BoxCollider>();

        // Spawn First Chunk
        GameObject road = Instantiate(roadChunks[0], transform.position, roadChunks[0].transform.rotation);
        road.GetComponent<MoveChunk>().SetSpeed(speed);
        road.GetComponent<MoveChunk>().SetSpawnDespawnZDistance(spawnDespawnZDistance);
        nextChunk = 1;
    }

    // Spawning
    public void Spawn(Vector3 spawnLocation) {
        if (randomChunk)
        {
            // Spawn Chunks in Random Order
            int rand = Random.Range(0, roadChunks.Count);
            GameObject road = Instantiate(roadChunks[rand], spawnLocation, roadChunks[rand].transform.rotation);
            road.GetComponent<MoveChunk>().SetSpeed(speed);
            road.GetComponent<MoveChunk>().SetSpawnDespawnZDistance(spawnDespawnZDistance);
        }
        else
        {
            // Spawn Chunks in Order
            if (nextChunk >= roadChunks.Count)
            {
                GameObject road = Instantiate(roadChunks[0], spawnLocation, roadChunks[0].transform.rotation);
                road.GetComponent<MoveChunk>().SetSpeed(speed);
                road.GetComponent<MoveChunk>().SetSpawnDespawnZDistance(spawnDespawnZDistance);
                nextChunk = 1;
            }
            else
            {
                GameObject road = Instantiate(roadChunks[nextChunk], spawnLocation, roadChunks[nextChunk].transform.rotation);
                road.GetComponent<MoveChunk>().SetSpeed(speed);
                road.GetComponent<MoveChunk>().SetSpawnDespawnZDistance(spawnDespawnZDistance);
                nextChunk++;
            }
        }
    }


    // Warning Arrows
    void OnTriggerEnter(Collider other)
    {
        /*
        //Debug.Log(other + " has entered");
        if (other.gameObject.tag.Equals("Obstacle"))
        {
            warnings.Add(Instantiate(warningSprite, new Vector3(other.transform.position.x, warningSprite.transform.position.y, warningSprite.transform.position.z), Quaternion.identity));
            warnings[warnings.Count - 1].GetComponent<FollowCamera>().SetValues(rvZStart, rv);
        }
        */

        if (other.gameObject.tag.Equals("Obstacle"))
        {
            warnings.Add(Instantiate(warningSprite, warningCamvas));
            warnings[warnings.Count - 1].GetComponent<UiElementFollowObject>().SetObject(other.gameObject);
            warnings[warnings.Count - 1].GetComponent<UiElementFollowObject>().SetCanvas(warningCamvas.GetComponent<RectTransform>());
            warnings[warnings.Count - 1].GetComponent<UiElementFollowObject>().SetRv(rv);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Obstacle"))
        {
            //Debug.Log(warnings[0]);
            Destroy(warnings[0]);
            warnings.RemoveAt(0);
        }
    }
}
