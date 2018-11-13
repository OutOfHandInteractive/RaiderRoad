﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManagerScript : MonoBehaviour {

    public static sceneManagerScript Instance;
    public List<string> NeedPlaySelScene; // a list of scenes that require the Player Select Scene before playing

    private string playSelScene = "PlayerSelect";
    private static string nextScene;
    private static int[] char1Players;
    private static int[] char2Players;
    private static int[] char3Players;
    private static int[] char4Players;
    private Transform[] playerPos = new Transform[4];

    public Transform character1; //FOR MOMENT, MENU MANAGER WILL HANDLE SPAWNING PLAYERS
    public Transform character2;
    public Transform character3;
    public Transform character4;
    private Transform rv; //rv is reference to RV obj in scene

    void Awake() {
		// Have playlist persist across scenes.
		if (Instance == null) {
			DontDestroyOnLoad(gameObject); // Don't destroy this object
			Instance = this;
		}
		else {
			Destroy(this);
		}
    }

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("PlayerSelect"))
        {   
            nextScene = null;
        }
        else    //check in case someone tests game starting in the player select screen, if so default to first scene
        {
            nextScene = NeedPlaySelScene[0];
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey("escape"))     //Set all variables back to null and send player to Scene menu
        {
            nextScene = null;
            char1Players = null;
            char1Players = null;
            char1Players = null;
            char1Players = null;
            SceneManager.LoadScene("SceneMenu", LoadSceneMode.Single);
        }
	}

    public void LoadScene(string myScene) {
        if (NeedPlaySelScene.Contains(myScene))     //if scene needs player select, go to that and set it to be next scene
        {
            nextScene = myScene;
            SceneManager.LoadScene(playSelScene, LoadSceneMode.Single);
        } else
        {
            SceneManager.LoadScene(myScene, LoadSceneMode.Single);
        }
    }

    public void PlaySelDone(int[] c1Array, int[] c2Array, int[] c3Array, int[] c4Array) {
        char1Players = c1Array;     //Pull all the arrays from the player select menu (so we know how many of each character we have)
        char2Players = c2Array;
        char3Players = c3Array;
        char4Players = c4Array;

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }


    //Load Main Game Scene Functions
    public void SpawnPlayers(GameObject myRv, Transform[] SpawnPoints)
    {
        //Debug.Log("Players are Spawned");
        //finding RV
        rv = myRv.transform;
        //Debug.Log(rv.name);
        playerPos = SpawnPoints;
        //Debug.Log(char1Players.Length);
        spawnChar1();
        spawnChar2();
        spawnChar3();
        spawnChar4();

        //gameObject.SetActive(false);
    }

    void spawnChar1()
    {
        if (char1Players.Length > 0)
        {
            Transform[] player = new Transform[char1Players.Length];
            for (int i = 0; i < char1Players.Length; i++)       //for loop in case multiples of character
            {
                player[i] = Instantiate(character1, playerPos[i].position, character1.rotation, rv);    //create character, set them to player spawn position
                player[i].gameObject.GetComponent<PlayerController_Rewired>().SetId(char1Players[i]);
                player[i].Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(char1Players[i]);
                //Debug.Log(player[i]);
            }
        }
    }

    void spawnChar2()
    {
        if (char2Players.Length > 0)
        {
            Transform[] player = new Transform[char2Players.Length];
            for (int i = 0; i < char2Players.Length; i++)
            {
                player[i] = Instantiate(character2, playerPos[i].position, character2.rotation, rv);
                player[i].gameObject.GetComponent<PlayerController_Rewired>().SetId(char2Players[i]);
                player[i].Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(char2Players[i]);
            }
        }
    }

    void spawnChar3()
    {
        if (char3Players.Length > 0)
        {
            Transform[] player = new Transform[char3Players.Length];
            for (int i = 0; i < char3Players.Length; i++)
            {
                player[i] = Instantiate(character3, playerPos[i].position, character3.rotation, rv);
                player[i].gameObject.GetComponent<PlayerController_Rewired>().SetId(char3Players[i]);
                player[i].Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(char3Players[i]);
            }
        }
    }

    void spawnChar4()
    {
        if (char4Players.Length > 0)
        {
            Transform[] player = new Transform[char4Players.Length];
            for (int i = 0; i < char4Players.Length; i++)
            {
                player[i] = Instantiate(character4, playerPos[i].position, character4.rotation, rv);
                player[i].gameObject.GetComponent<PlayerController_Rewired>().SetId(char4Players[i]);
                player[i].Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(char4Players[i]);
            }
        }
    }
}
