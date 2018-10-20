using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerPlacement_Rewired : MonoBehaviour {
    //This file is changed

    //--------------------
    // Public Variables
    //--------------------
    public int playerId = 0;
    public GameObject wall;
    public int wallInventory;
    public Text inventoryText;

    //--------------------
    // Private Variables
    //--------------------
    private Player player;
    private GameObject rv;
    private ArrayList nodes = new ArrayList();

    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        rv = GameObject.FindGameObjectWithTag("RV");
        initialized = true;
        changeInventory();
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
            nodes.Add(other.gameObject);
            if(wallInventory > 0)
            {
                other.GetComponent<BuildNode>().Build(wall);
                wallInventory--;
                changeInventory();
                //other.gameObject.SetActive (false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nodes.Remove(other.gameObject);
    }

    public void changeInventory()
    {
        inventoryText.text = "Walls: " + wallInventory.ToString();
    }

    public void SetId(int id)
    {
        playerId = id;
        initialized = false;
    }
}
