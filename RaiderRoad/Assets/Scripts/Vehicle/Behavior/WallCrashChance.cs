using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallCrashChance : MonoBehaviour {

    private int hitChance;
    private NavMeshObstacle obstacle;
	// Use this for initialization
	void Start () {
        hitChance = Random.Range(0, 100);
        obstacle = GetComponent<NavMeshObstacle>();
	}
	
	// Update is called once per frame
	void Update () {
		if(hitChance < 80)
        {
            obstacle.enabled = true;
        }
        else
        {
            obstacle.enabled = false;
        }
	}
}
