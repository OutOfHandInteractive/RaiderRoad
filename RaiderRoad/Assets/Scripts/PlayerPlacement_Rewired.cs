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
    public bool buildMode = false;
    public Text inventoryText;
    public Text mode;
    public GameObject heldItem = null; //probably make private later on
    public GameObject trap;
    public float damage = 25.0f;

    public Material TempAttMat; //for temporary attack for prototype

    //--------------------
    // Private Variables
    //--------------------
    private Player player;
    private GameObject rv;
    private ArrayList nodes = new ArrayList();      //probably better way to do this, REVISIT!
    private ArrayList trapNodes = new ArrayList();
    private ArrayList attackRange = new ArrayList();
    private bool hasItem = false;
    private GameObject floatingItem;

    private Color currentAttColor; //for temporary attack for prototype

    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        rv = GameObject.FindGameObjectWithTag("RV");
        initialized = true;
        changeInventory(); //set inventory text to players inventory count

        currentAttColor = TempAttMat.color; //get current color so we can play with alpha
    }

    void Update()
    {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor
        changeInventory();
        displayMode();

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
                    Destroy(floatingItem);
                    buildMode = false;
                }
                else
                {
                    Debug.Log("Occupied >:(");
                }
            }
        }
        else
        {
            if (player.GetButtonDown("Build Mode"))
            {
                if (buildMode) attackRange = new ArrayList(); //When switching out of build mode, attack will get stuck in InvalidOperationException: List has changed. This helps
                buildMode = !buildMode;
            }
            if (buildMode)
            {
                if (wallInventory > 0 && player.GetButtonDown("Place Object") && nodes.Count > 0)
                {
                    GameObject toBuild = (GameObject)nodes[0];
                    if (!toBuild.GetComponent<BuildNode>().occupied)
                    {
                        toBuild.GetComponent<BuildNode>().Build(wall);
                        wallInventory--;
                        changeInventory();
                        //other.gameObject.SetActive (false);
                    }
                    else
                    {
                        Debug.Log("Occupied >:(");
                    }
                }
            }
            else if (player.GetButtonDown("Attack"))
            {
                foreach (GameObject item in attackRange)
                {
                    //Debug.Log(item);
                    if (item == null)
                    {
                        attackRange.Remove(item);
                    }
                    else if (item.CompareTag("Wall"))
                    {
                        item.GetComponent<Wall>().Damage(damage);
                    }
                    else if (item.CompareTag("Trap"))
                    {
                        item.GetComponent<Trap>().Damage(damage);
                    }
                }

                currentAttColor.a = 0.5f; //setting attack model's mat to 1/2 visible

            }
            
        }
        
        if (currentAttColor.a > 0) {
            currentAttColor.a -= 1f * Time.deltaTime; //transitioning color from visibile to invisible
            TempAttMat.color = currentAttColor;
        } else if (currentAttColor.a < 0) { // getting alpha to exactly 0, and after that won't check further
            currentAttColor.a = 0;
            TempAttMat.color = currentAttColor;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        
        //Debug.Log(other.name);
        if ((other.gameObject.name == "BuildNode" || other.name == "xNode"))
        {
            //Debug.Log("Added");
            nodes.Add(other.gameObject);
            if(buildMode && heldItem == null) other.GetComponent<BuildNode>().Show(wall); //if player is in build mode, activate show wall in the build node script
            //GameObject toRemove = (GameObject)nodes[0];
            //toRemove.GetComponent<BuildNode>().Show(wall);
        }
        if (other.name == "Trap")
        {
            //Debug.Log("Trap node added");
            trapNodes.Add(other.gameObject);
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            attackRange.Add(other.gameObject);
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            attackRange.Add(other.gameObject);
        }

        // Pick Up
        //if (other.gameObject.CompareTag("Drops"))
        //{
            if(other.name == "Wall Drop")
            {
                wallInventory++;
                Destroy(other.gameObject);
            }
            else if(other.name == "Trap Drop")
            {
                heldItem = trap;
                Destroy(other.gameObject);
                buildMode = true;
            }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Removed");
        //GameObject toRemove = (GameObject)nodes[0];
        if(other.gameObject.name == "BuildNode" || other.name == "xNode") other.GetComponent<BuildNode>().RemoveShow(); //if object leaving is a build node, make sure it isn't showing holo of object
        nodes.Remove(other.gameObject);
        trapNodes.Remove(other.gameObject);
        attackRange.Remove(other.gameObject);
    }

    public void changeInventory() //change inventory in text only after building wall, saves overhead
    {
        inventoryText.text = "Walls: " + wallInventory.ToString();
    }

    void displayMode()
    {
        mode.text = "Build Mode: " + buildMode;
    }

    public void floatItem() //makes held item float and spin above player
    {
        if (!hasItem)
        {
            floatingItem = Instantiate(heldItem.GetComponent<Trap>().drop, //fix later for prettier
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
