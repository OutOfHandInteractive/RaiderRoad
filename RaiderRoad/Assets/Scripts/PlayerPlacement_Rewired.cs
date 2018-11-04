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
    public GameObject engine;
    public float damage = 25.0f;
    public float timeToDrop = 1f; //Time needed to drop item (by holding down button)

    public Material TempAttMat; //for temporary attack for prototype

    //--------------------
    // Private Variables
    //--------------------
    private Player player;
    private GameObject rv;
    private ArrayList nodes = new ArrayList();      //probably better way to do this, REVISIT!
    private ArrayList trapNodes = new ArrayList();
    private ArrayList engineNodes = new ArrayList();
    private ArrayList attackRange = new ArrayList();
    private bool hasItem = false;
    private GameObject floatingItem;

    private Color currentAttColor; //for temporary attack for prototype
    private float holdTime; //timer for "how long button is held"

    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        //TEMP
        //inventoryText = GameObject.Find("WallText").GetComponent<Text>(); //make this work for all players
        //mode = GameObject.Find("BuildingMode").GetComponent<Text>();
        //Direct connection, because each player has their own canvas

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
            if (player.GetButtonDown("Place Object"))
            {
                if (heldItem.tag == "Trap" && trapNodes.Count > 0)
                {
                    GameObject trapBuild = (GameObject)trapNodes[0];
                    //Debug.Log(trapBuild);
                    if (!trapBuild.GetComponent<TrapNode>().occupied)
                    {
                        trapBuild.GetComponent<TrapNode>().BuildTrap(heldItem);
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
                else if (heldItem.tag == "Engine" && engineNodes.Count > 0)
                {
                    GameObject EngineBuild = (GameObject)engineNodes[0];
                    if (!EngineBuild.GetComponent<PoiNode>().occupied)
                    {
                        EngineBuild.GetComponent<PoiNode>().BuildPoi(heldItem);
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

            if (player.GetButton("Use"))
            {
                holdTime += Time.deltaTime;
                if (holdTime > timeToDrop)
                {
                    GameObject dropItem = null;
                    if (heldItem.tag == "Trap") dropItem = heldItem.GetComponent<Trap>().drop; //get the drop prefab item from item's script
                    if (heldItem.tag == "Engine") dropItem = heldItem.GetComponent<Engine>().drop;
                    // more ifs for other items
                    GameObject item = Instantiate(dropItem, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);    //create drop item
                    item.name = heldItem.name + " Drop";

                    heldItem = null;
                    hasItem = false;
                    Destroy(floatingItem);
                    buildMode = false;
                    holdTime = 0f;
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
                        toBuild.GetComponent<BuildNode>().Build(wall, toBuild);
                        wallInventory--;
                        changeInventory();
                        //other.gameObject.SetActive (false);
                    }
                    else
                    {
                        Debug.Log("Occupied >:(");
                    }
                }
                if (wallInventory <= 0) buildMode = false; //leave wall build mode if you have no wall (needs more feedback)
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
                    else if (item.CompareTag("Engine"))
                    {
                        item.GetComponent<Engine>().Damage(damage);
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
        if ((other.gameObject.name == "BuildNode" || other.name == "xNode") && wallInventory > 0)
        {
            //Debug.Log("Added");
            nodes.Add(other.gameObject);
            if(buildMode && heldItem == null) other.GetComponent<BuildNode>().Show(wall); //if player is in build mode, activate show wall in the build node script
            //GameObject toRemove = (GameObject)nodes[0];
            //toRemove.GetComponent<BuildNode>().Show(wall);
        }
        if (heldItem != null && other.name == "Trap")
        {
            if (heldItem.tag == "Trap")
            {
                //Debug.Log("Trap node added");
                trapNodes.Add(other.gameObject);
                if (buildMode) other.GetComponent<TrapNode>().Show(trap); //if player is in build mode, activate show wall in the build node script
            }
        }
        if (heldItem != null && other.name == "PoiNode")
        {
            if (heldItem.tag == "Engine")
            {
                //Debug.Log("Trap node added");
                engineNodes.Add(other.gameObject);
                if (buildMode) other.GetComponent<PoiNode>().Show(engine); //if player is in build mode, activate show wall in the build node script
            }
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            attackRange.Add(other.gameObject);
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            attackRange.Add(other.gameObject);
        }
        if (other.gameObject.CompareTag("Engine"))
        {
            attackRange.Add(other.gameObject);
        }

        // Pick Up
        //if (other.gameObject.CompareTag("Drops"))
        //{
        if (other.name == "Wall Drop")
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
            else if(other.name == "Engine Drop")
            {
                heldItem = engine;
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
        if (other.name == "Trap") other.GetComponent<TrapNode>().RemoveShow();
        if (other.name == "PoiNode") other.GetComponent<PoiNode>().RemoveShow();

        nodes.Remove(other.gameObject);
        trapNodes.Remove(other.gameObject);
        engineNodes.Remove(other.gameObject);
        attackRange.Remove(other.gameObject);
    }

    public void changeInventory() //change inventory in text only after building wall, saves overhead
    {
        inventoryText.text = wallInventory.ToString();
    }

    void displayMode()
    {
        if (buildMode) mode.text = "Building";
        else mode.text = " ";
        //mode.text = "Build Mode: " + buildMode;
    }

    public void floatItem() //makes held item float and spin above player
    {
        if (!hasItem)
        {
            GameObject myFloat = null;
            if (heldItem == trap) myFloat = heldItem.GetComponent<Trap>().drop;
            else if (heldItem == engine) myFloat = heldItem.GetComponent<Engine>().drop;
            floatingItem = Instantiate(myFloat, //fix later for prettier
                new Vector3(transform.parent.position.x, transform.parent.position.y + 1.5f, transform.parent.position.z), Quaternion.identity, transform.parent);
            hasItem = true;
        }

    }

    public void SetId(int id)
    {
        playerId = id;
        initialized = false;
    }
}
