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
    public int holdingNumber;

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
        if ((other.gameObject.name == "BuildNode" || other.name == "xNode") && player.GetButtonDown("Build"))
        {
            if (other.name != "xNode")
            {
                float h = other.gameObject.GetComponent<Testing>().height;
                if (h <= 3 && holdingNumber > 0)
                {
                    GameObject walltemp = Instantiate(wall, new Vector3(other.transform.position.x, (0.3f * (h)), other.transform.position.z), Quaternion.identity);
                    walltemp.transform.parent = RV.transform;
                    other.gameObject.GetComponent<Testing>().height += 1f;
                    holdingNumber -= 1;
                }
                else
                {
                    Debug.Log("Max Height");
                }

               // GameObject walltemp = Instantiate(wall, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), Quaternion.identity);
				//walltemp.transform.parent = RV.transform;
            }
            else
            {
                float h = other.gameObject.GetComponent<Testing>().height;
                if (h <= 3 && holdingNumber > 0)
                {
                    GameObject walltemp = Instantiate(wall, new Vector3(other.transform.position.x, (0.3f * (h)), other.transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
                    walltemp.transform.parent = RV.transform;
                    other.gameObject.GetComponent<Testing>().height += 1f;
                    holdingNumber -= 1;
                }
                else
                {
                    Debug.Log("Max Height");
                }
                //GameObject walltemp = Instantiate(wall, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
				//walltemp.transform.parent = RV.transform;
            }
			//other.gameObject.SetActive (false);
        }

        if ((other.gameObject.name == "Scraps") && player.GetButtonDown("Build"))
        {
            holdingNumber += 2;
        }

        
    }
}
