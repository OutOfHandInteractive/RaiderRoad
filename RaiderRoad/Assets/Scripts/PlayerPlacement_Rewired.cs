using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerPlacement_Rewired : MonoBehaviour {
    //--------------------
    // Public Variables
    //--------------------
    public int playerId = 0;
    public GameObject wall;
	public GameObject RV;

    //--------------------
    // Private Variables
    //--------------------
    private Player player;

    [System.NonSerialized]
    private bool initialized;

    void Initialize() {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        initialized = true;
    }

    void Update () {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if ((other.gameObject.name == "BuildNode" || other.name == "xNode") && player.GetButtonDown("Build Wall"))
        {
            if (other.name != "xNode")
            {
                GameObject walltemp = Instantiate(wall, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), Quaternion.identity);
				walltemp.transform.parent = RV.transform;
            }
            else
            {
                GameObject walltemp = Instantiate(wall, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
				walltemp.transform.parent = RV.transform;
            }
			other.gameObject.SetActive (false);
        }
    }
}
