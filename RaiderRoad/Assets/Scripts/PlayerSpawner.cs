using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {


    public GameObject SceneRV;
    private sceneManagerScript sceneManage;

    // Use this for initialization
    void Awake () {
        sceneManage = sceneManagerScript.Instance;

        //Debug.Log(SceneRV.name);
        Transform[] SpawnArray = new Transform[4];
        SpawnArray[0] = SceneRV.transform.Find("SpawnPoints").Find("player1Spawn");
        SpawnArray[1] = SceneRV.transform.Find("SpawnPoints").Find("player2Spawn");
        SpawnArray[2] = SceneRV.transform.Find("SpawnPoints").Find("player3Spawn");
        SpawnArray[3] = SceneRV.transform.Find("SpawnPoints").Find("player4Spawn");
        sceneManage.SpawnPlayers(SceneRV, SpawnArray);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
