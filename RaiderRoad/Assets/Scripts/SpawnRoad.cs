using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoad : MonoBehaviour {
    //--------------------
    // Public Variables
    //--------------------
    public List<GameObject> roadSegments;
    public float speed;

    //--------------------
    // Private Variables
    //--------------------
    private int next;

    // Use this for initialization
    void Start () {
        GameObject road = Instantiate(roadSegments[0]);
        road.GetComponent<MoveRoad>().SetSpeed(speed);
        next = 1;
    }
	
	// Update is called once per frame
	public void Spawn() {
        if (next >= roadSegments.Count) {
            GameObject road = Instantiate(roadSegments[0], transform);
            road.GetComponent<MoveRoad>().SetSpeed(speed);
            next = 1;
        } else {
            GameObject road = Instantiate(roadSegments[next], transform);
            road.GetComponent<MoveRoad>().SetSpeed(speed);
            next++;
        } 
    }
}
