using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoad : MonoBehaviour {
    //--------------------
    // Public Variables
    //--------------------
    public List<GameObject> roadSegments;
    public float speed;
    public float FirstSpawnOffset = 10f;

    //--------------------
    // Private Variables
    //--------------------
    private int next;

    // Use this for initialization
    void Start () {
        Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z + FirstSpawnOffset);
        GameObject road = Instantiate(roadSegments[0], offset, roadSegments[0].transform.rotation);
        road.GetComponent<MoveRoad>().SetSpeed(speed);
        next = 1;
    }
	
	// Update is called once per frame
	public void Spawn() {
        if (next >= roadSegments.Count) {
            GameObject road = Instantiate(roadSegments[0], transform.position, roadSegments[0].transform.rotation);
            road.GetComponent<MoveRoad>().SetSpeed(speed);
            next = 1;
        } else {
            GameObject road = Instantiate(roadSegments[next], transform.position, roadSegments[next].transform.rotation);
            road.GetComponent<MoveRoad>().SetSpeed(speed);
            next++;
        } 
    }
}
