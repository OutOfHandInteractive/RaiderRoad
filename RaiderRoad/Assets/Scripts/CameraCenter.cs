using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour {

    private GameObject[] players;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = centerCamera();
	}

    public Vector3 centerCamera()
    {
        if(players == null){
            return this.transform.position;
        }else{
            float totalX = 0f;
            float totalZ = 0f;
            for(int i = 0; i < players.Length - 1; i++){
                totalX += players[i].transform.position.x;
                totalZ += players[i].transform.position.z;
            }
            float centerX = totalX / players.Length;
            float centerZ = totalZ / players.Length;
            return new Vector3(centerX,this.transform.position.y,centerZ-10);
        }
    }
}
