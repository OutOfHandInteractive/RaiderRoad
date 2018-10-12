using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class brickBuilding : MonoBehaviour
{
    //--------------------
    // Public Variables
    //--------------------
    public int playerId = 0;
    public GameObject wall;
    public Material selected, unselected;

    //--------------------
    // Private Variables
    //--------------------
    private Player player;

    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        initialized = true;
    }

    void Update()
    {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "floor")
        {
            Debug.Log("selectable");
            other.gameObject.GetComponent<MeshRenderer>().material = selected;
        }

        Debug.Log(other.name);
        if (player.GetButtonDown("Build") && other.gameObject.tag == "floor")
        {
            float h = other.gameObject.GetComponent<floor>().height;
            if (h < 5)
            {
                Instantiate(wall, new Vector3(other.transform.position.x, (0.8f * (h)), other.transform.position.z), Quaternion.identity);
                other.gameObject.GetComponent<floor>().height += 2f;
            }
            else
            {
                Debug.Log("Max Height");
            }
            
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "floor")
        {
            other.gameObject.GetComponent<MeshRenderer>().material = unselected;
        }
    }
}
