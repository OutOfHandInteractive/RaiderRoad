using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerPlacement_Rewired : MonoBehaviour {
    //This file is changed

    //--------------------
    // Public Variables
    //--------------------
    public int playerId = 0;
    public GameObject wall;

    //--------------------
    // Private Variables
    //--------------------
    private Player player;
    private GameObject rv;

    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        rv = GameObject.FindGameObjectWithTag("RV");
        initialized = true;
    }

    void Update()
    {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if ((other.gameObject.name == "BuildNode" || other.name == "xNode") && player.GetButtonDown("Build Wall"))
        {
            other.GetComponent<BuildNode>().Build();
			//other.gameObject.SetActive (false);
        }
    }

    public void SetId(int id)
    {
        playerId = id;
        initialized = false;
    }
}
