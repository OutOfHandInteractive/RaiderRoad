﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {


    public GameObject SceneRV;
    private sceneManagerScript sceneManage;
    private bool done = false;

    // Use this for initialization
    void Start () {
        sceneManage = sceneManagerScript.Instance;

        //Debug.Log(SceneRV.name);

    }
	
	// Update is called once per frame
	void Update () {
		if(!done && GameManager.GameManagerInstance != null)
        {
            Transform[] SpawnArray = new Transform[4];
            SpawnArray[0] = SceneRV.transform.Find("SpawnPoints").Find("player1Spawn");
            SpawnArray[1] = SceneRV.transform.Find("SpawnPoints").Find("player2Spawn");
            SpawnArray[2] = SceneRV.transform.Find("SpawnPoints").Find("player3Spawn");
            SpawnArray[3] = SceneRV.transform.Find("SpawnPoints").Find("player4Spawn");
            sceneManage.SpawnPlayers(SceneRV, SpawnArray);
            done = true;
        }
	}
}
