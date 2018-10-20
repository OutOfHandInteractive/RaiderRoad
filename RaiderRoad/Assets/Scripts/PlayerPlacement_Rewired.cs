using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerPlacement_Rewired : MonoBehaviour {
    //Michael

    //--------------------
    // Public Variables
    //--------------------
    public int playerId = 0;
    public GameObject wall;
    public int wallInventory;
    public Text inventoryText;
    public GameObject heldItem = null; //probably make private later on

    //--------------------
    // Private Variables
    //--------------------
    private Player player;
    private GameObject rv;
    private ArrayList nodes = new ArrayList();      //probably better way to do this, REVISIT!
    private ArrayList trapNodes = new ArrayList();
    private bool hasItem = false;

    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        rv = GameObject.FindGameObjectWithTag("RV");
        initialized = true;
        changeInventory(); //set inventory text to players inventory count
    }

    void Update()
    {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor

        if(heldItem != null)
        {
            floatItem(); //not following player
            if (player.GetButtonDown("Place Object") && trapNodes.Count > 0)
            {
                GameObject floorBuild = (GameObject)trapNodes[0];
                if (!floorBuild.GetComponentInParent<floor>().occupied)
                {
                    floorBuild.GetComponentInParent<floor>().BuildTrap(heldItem);
                    heldItem = null;
                    hasItem = false;
                }
                else
                {
                    Debug.Log("Occupied >:(");
                }
            }
        }
        else
        {
            if (wallInventory > 0 && player.GetButtonDown("Place Object") && nodes.Count > 0)
            {
                GameObject toBuild = (GameObject) nodes[0];
                if (!toBuild.GetComponent<BuildNode>().occupied)
                {
                    toBuild.GetComponent<BuildNode>().Build(wall);
                    wallInventory--;
                    changeInventory();
                    //other.gameObject.SetActive (false);
                }else{
                    Debug.Log("Occupied >:(");
                }
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        //Debug.Log(other.name);
        if ((other.gameObject.name == "BuildNode" || other.name == "xNode"))
        {
            //Debug.Log("Added");
            nodes.Add(other.gameObject);
            //other.GetComponent<BuildNode>().Show(wall);
            //GameObject toRemove = (GameObject)nodes[0];
            //toRemove.GetComponent<BuildNode>().Show(wall);
        }
        if (other.name == "Trap")
        {
            Debug.Log("Trap node added");
            trapNodes.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Removed");
        //GameObject toRemove = (GameObject)nodes[0];
        //other.GetComponent<BuildNode>().RemoveShow();
        nodes.Remove(other.gameObject);
        trapNodes.Remove(other.gameObject);
    }

    public void changeInventory() //change inventory in text only after building wall, saves overhead
    {
        inventoryText.text = "Walls: " + wallInventory.ToString();
    }

    public void floatItem() //makes held item float and spin above player
    {
        if (!hasItem)
        {
            Instantiate(heldItem.GetComponent<Trap>().drop, //fix later for prettier
                new Vector3(transform.parent.position.x, transform.parent.position.y + 0.65f, transform.parent.position.z), Quaternion.identity, transform.parent);
            hasItem = true;
        }

    }

    public void SetId(int id)
    {
        playerId = id;
        initialized = false;
    }
}
