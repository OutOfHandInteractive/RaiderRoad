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
    public GameObject warningSprite;
    public GameObject rv;

    //--------------------
    // Private Variables
    //--------------------
    private int next;
    private List<GameObject> warnings = new List<GameObject>();
    private float rvZStart;
    private BoxCollider col;

    // Use this for initialization
    void Start () {
        rvZStart = rv.transform.position.z;
        col = gameObject.GetComponent<BoxCollider>();
        Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z + FirstSpawnOffset);
        GameObject road = Instantiate(roadSegments[0], offset, roadSegments[0].transform.rotation);
        road.GetComponent<MoveRoad>().SetSpeed(speed);
        next = 1;
    }

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

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other + " has entered");
        if (other.gameObject.tag.Equals("Obstacle"))
        {
            warnings.Add(Instantiate(warningSprite, new Vector3(other.transform.position.x, warningSprite.transform.position.y, warningSprite.transform.position.z), Quaternion.identity));
            warnings[warnings.Count - 1].GetComponent<FollowCamera>().SetValues(rvZStart, rv);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Obstacle"))
        {
            Debug.Log(warnings[0]);
            Destroy(warnings[0]);
            warnings.RemoveAt(0);
        }
    }
}
