using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

/// <summary>
/// Class for handling all user input related to building walls, weapons, traps, etc. as well as attacking.
/// </summary>
public class PlayerPlacement_Rewired : MonoBehaviour {
    //Michael

    //--------------------
    // Public Variables
    //--------------------
    public Animator myAni;

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
    public float knockback_force = 2000.0f;
    public float attack_cooldown = .25f;

    public GameObject AttackObject; //for temporary attack for prototype
    //melee weapon variables
    public GameObject myWeapon;
    public float timeTilSheath;
    public float sheathAnimTime;

    //--------------------
    // Private Variables
    //--------------------
    private Player player;
	private PlayerController_Rewired pController;
    private GameManager g;
    private bool myPauseInput = false; //used for stopping input at end of game or pause
    private GameObject rv;
    private List<GameObject> nodes = new List<GameObject>();      //probably better way to do this, REVISIT!
    private List<GameObject> trapNodes = new List<GameObject>();
    private List<GameObject> engineNodes = new List<GameObject>();
    private List<GameObject> attackRange = new List<GameObject>();
	private List<GameObject> destructableParts = new List<GameObject>();
	private bool hasItem = false;
    private GameObject floatingItem;

    private bool myInteracting = false;

    private Color currentAttColor; //for temporary attack for prototype
    private Material TempAttMat;
    private float holdTime; //timer for "how long button is held"
    private float attackCount;
    private bool canAttack = true;
    //melee weapon variables
    private float sheathTimer = 0f;
    private Vector3 MeleeWeapScale;

    [System.NonSerialized]
    private bool initialized;
    
	private void Start() {
        pController = GetComponentInParent<PlayerController_Rewired>();
	}

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

        TempAttMat = AttackObject.GetComponent<Renderer>().material;
        currentAttColor = TempAttMat.color; //get current color so we can play with alpha

        MeleeWeapScale = myWeapon.transform.localScale;
        myWeapon.SetActive(false);

        g = GameManager.GameManagerInstance;
    }

    void Update()
    {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor
        changeInventory();
        //displayMode(); //display "building" text

        if(!(g == null)) {
            myPauseInput = g.GetComponent<GameManager>().pauseInput;
        }

        if (!myPauseInput) {
            // Attack cooldown
            if (!canAttack)
            {
                attackCount -= Time.deltaTime;
            }
            if (attackCount <= 0.0)
            {
                //myWeapon.SetActive(false);
                canAttack = true;
            }

            //Sheath Weapon
            if(sheathTimer > 0f) {
                sheathTimer -= Time.deltaTime;
                if(sheathTimer < sheathAnimTime)
                {
                    //myAni.SetTrigger("sheathWeapon");
                    myWeapon.transform.localScale = MeleeWeapScale * Mathf.SmoothStep(1f, 0f, sheathAnimTime - sheathTimer);
                }
            } else {
                myWeapon.SetActive(false);
            }

            myInteracting = pController.interacting;
            //checking that player isn't "interacting" (driving, piloting weapon, etc)
            if (myInteracting)
            {
                // Early exit
                return;
            }

            if (heldItem != null) //change this to tags later
            {
                HoldingItem();
            }
            else
            {
                NotHoldingItem();
            }

            if (currentAttColor.a > 0)
            {
                currentAttColor.a -= 1f * Time.deltaTime; //transitioning color from visibile to invisible
                TempAttMat.color = currentAttColor;
            }
            else if (currentAttColor.a < 0)
            { // getting alpha to exactly 0, and after that won't check further
                currentAttColor.a = 0;
                TempAttMat.color = currentAttColor;
            }
        }
    }

    private void HoldingItem()
    {
        floatItem();
        SheathWeapon();
        if (player.GetButtonDown("Place Object"))
        {
            if (heldItem.tag == "Trap" && trapNodes.Count > 0)
            {
                BuildTrap();
            }
            else if (heldItem.tag == "Engine" && engineNodes.Count > 0)
            {
                BuildEngine();
            }
            else if (heldItem.tag == "Weapon" && nodes.Count > 0) //to change later?
            {
                foreach(GameObject node in nodes)
                {
                    if (node.GetComponent<BuildNode>().canPlaceWeapon)
                    {
                        BuildWeapon(node);
                        break;
                    }
                }
            }
        }

        if (player.GetButton("Use"))
        {
			//GETTING RID OF HOLD TO DROP
			//holdTime += Time.deltaTime;
			//if (holdTime > timeToDrop)
			//{
			dropItem();
            //}
        }
    }

	public void dropItem() {
		if (heldItem != null) {
			GameObject dropItem = heldItem.GetComponent<Constructable>().drop;
			//if (heldItem.tag == "Trap") dropItem = heldItem.GetComponent<Trap>().drop; //get the drop prefab item from item's script
			//if (heldItem.tag == "Engine") dropItem = heldItem.GetComponent<Engine>().drop;
			// more ifs for other items
			GameObject item = Instantiate(dropItem, new Vector3(transform.parent.position.x, transform.parent.position.y + 0.3f, transform.parent.position.z) + transform.parent.forward * 1.7f, Quaternion.identity);
			//create drop item in front of player (needs to be parent to get exact position in world space)
			item.name = heldItem.name + " Drop";

			heldItem = null;
			hasItem = false;
			Destroy(floatingItem);
			buildMode = false;
			holdTime = 0f;

			myAni.SetBool("isHolding", false);
		}
	}

    private void SetSpringTrapPosition(GameObject obj)
    {
        SpringTrap trap = obj.GetComponent<SpringTrap>();
        if (trap != null)
        {
            trap.SetRotation(gameObject);
        }
    }

    private void BuildDurableConstruct(DurabilityBuildNode node)
    {
        if (!node.occupied)
        {
            //myAni.SetTrigger("build");
            GameObject obj = node.Build(heldItem, floatingItem.GetComponent<DurableConstruct>().GetDurability());
            SetSpringTrapPosition(obj);
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

    private void BuildTrap()
    {
        GameObject trapBuild = trapNodes[0];
        //Debug.Log(trapBuild);
        BuildDurableConstruct(trapBuild.GetComponent<TrapNode>());

        myAni.SetTrigger("build");
        myAni.SetBool("isHolding", false);
    }

    private void BuildEngine()
    {
        GameObject EngineBuild = engineNodes[0];
        BuildDurableConstruct(EngineBuild.GetComponent<PoiNode>());

        myAni.SetTrigger("build");
        myAni.SetBool("isHolding", false);
    }

    private void BuildWeapon(GameObject toBuild)
    {
        if (!toBuild.GetComponent<BuildNode>().occupied)
        {
            myAni.SetTrigger("build");
            myAni.SetBool("isHolding", false);
            toBuild.GetComponent<BuildNode>().Build(heldItem, toBuild);
            heldItem = null;
            hasItem = false;
            Destroy(floatingItem);
            buildMode = false;
            //other.gameObject.SetActive (false);
        }
        else
        {
            Debug.Log("Occupied >:(");
        }
    }

    private void NotHoldingItem()
    {
        if (player.GetButton("Build Mode"))
        {
            if (!buildMode && wallInventory > 0 && pController.state == PlayerController_Rewired.playerStates.up)
            {
                //When switching out of build mode, attack will get stuck in InvalidOperationException: List has changed. This helps
                if (buildMode) attackRange = new List<GameObject>();
                buildMode = !buildMode;

                checkHologram();
                SheathWeapon();
                myAni.SetBool("isHolding", true);
            }
        } else {
            if (nodes.Count > 0) { //If showing holo wall, turn off every holo wall currently being displayed
                for(int i = 0; i < nodes.Count; i++){ 
                    nodes[i].GetComponent<BuildNode>().RemoveShow();
                }
            }
            buildMode = false;
            myAni.SetBool("isHolding", false);
            hasItem = false;
            Destroy(floatingItem);
        }
        if (buildMode)
        {
            floatItem(); //float a wall piece instead

            if (wallInventory > 0 && player.GetButtonDown("Place Object") && nodes.Count > 0)
            {
                BuildWall();
            }
            if (wallInventory <= 0)
            {
                buildMode = false; //leave wall build mode if you have no wall (needs more feedback)

                if (nodes.Count > 0) { //If showing holo wall, turn off every holo wall currently being displayed
                    for(int i = 0; i < nodes.Count; i++){ 
                        nodes[i].GetComponent<BuildNode>().RemoveShow();
                    }
                }
                buildMode = false;
                myAni.SetBool("isHolding", false);
                hasItem = false;
                Destroy(floatingItem);
            }
        }
        else if (player.GetButtonDown("Attack") && canAttack && pController.state == PlayerController_Rewired.playerStates.up)
        {
            Attack();
        }
    }

    private void BuildWall()
    {
        GameObject toBuild = nodes[0];
        if (!toBuild.GetComponent<BuildNode>().occupied)
        {
            myAni.SetTrigger("build");
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

    private bool AttackVehicleParts()
    {
        foreach(GameObject part in destructableParts)
        {
            if(part!=null)
            {
                if(part.GetComponent<DestructiblePart>().takeDamage(1) <= 0)
                {
                    destructableParts.Remove(part);
                }
                return true;
            }
        }
        return false;
    }

    private void Attack()
    {
        myAni.SetTrigger("attack");
        canAttack = false;
        attackCount = attack_cooldown;
        myWeapon.SetActive(true);
        myWeapon.transform.localScale = MeleeWeapScale;
        sheathTimer = timeTilSheath;

        if (!AttackVehicleParts())
        {
            Util.RemoveNulls(attackRange);
            foreach (GameObject item in attackRange)
            {
                //Debug.Log(item);
                Constructable construct;
                if(item == null)
                {
                    // This should've been removed!!
                    continue;
                }
                if (item.CompareTag("Weapon"))
                {
                    item.GetComponent<Weapon>().Damage(damage, gameObject.transform.parent.gameObject);
                }
                else if ((construct = item.GetComponent<Constructable>()) != null)
                {
                    construct.Damage(damage);
                }
                else if (item.CompareTag("Enemy"))
                {
                    Vector3 dir = item.transform.position - transform.parent.position;
                    dir = Vector3.Normalize(new Vector3(dir.x, 0.0f, dir.z));
                    item.GetComponent<Rigidbody>().AddForce(dir * knockback_force);
                    item.GetComponent<StatefulEnemyAI>().takeDamage(damage);
                    item.GetComponent<StatefulEnemyAI>().Stunned();
                    //Debug.Log(dir);
                }
            }
        }

        currentAttColor.a = 0.5f; //setting attack model's mat to 1/2 visible
    }

    private void CheckBuildNodes()
    {
        if (buildMode)
        {
            bool isWall;
            GameObject item;
            if (heldItem == null)
            {
                item = wall;
                isWall = wallInventory > 0;
            }
            else if (heldItem != null && heldItem.CompareTag("Weapon"))
            {
                item = heldItem;
                isWall = false;
            }
            else
            {
                return;
            }
            bool found = false;
            foreach (GameObject obj in nodes)
            {
                BuildNode node = obj.GetComponent<BuildNode>();
                node.RemoveShow();
                if (!found && !node.occupied && (isWall || node.canPlaceWeapon))
                {
                    node.Show(item);
                    found = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.name);
        if ((other.tag == "WallNode"))
        {
            //Debug.Log("Added");
            if (!other.GetComponent<BuildNode>().occupied)
            {
                nodes.Add(other.gameObject);
                CheckBuildNodes();
            }
            //if player is in build mode, activate show wall in the build node script
            //GameObject toRemove = (GameObject)nodes[0];
            //toRemove.GetComponent<BuildNode>().Show(wall);
        }
        if (heldItem != null && other.name == "Trap")
        {
            if (heldItem.tag == "Trap")
            {
                //Debug.Log("Trap node added");
                trapNodes.Add(other.gameObject);
                //if player is in build mode, activate show wall in the build node script
                if (buildMode && !other.GetComponent<TrapNode>().occupied)
                {
                    GameObject holo = other.GetComponent<TrapNode>().Show(heldItem);
                    SetSpringTrapPosition(holo);
                } 
            }
        }
        if (heldItem != null && other.name == "PoiNode")
        {
            if (heldItem.tag == "Engine")
            {
                //Debug.Log("Trap node added");
                engineNodes.Add(other.gameObject);
                if (buildMode && !other.GetComponent<PoiNode>()) other.GetComponent<PoiNode>().Show(heldItem); //if player is in build mode, activate show wall in the build node script
            }
        }
        if (Util.isEnemy(other.gameObject))
        {
            attackRange.Add(other.gameObject);
        }
        else
        {
            Constructable construct = other.GetComponent<Constructable>();
            if (construct != null && !construct.isHolo && construct.isPlaced())
            {
                attackRange.Add(other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("Interactable")) {
            if (other.GetComponentInChildren<BoxCollider>().enabled)
            {
                pController.addInteractable(other.gameObject);
            }
			
		}
        /*
        if (other.gameObject.CompareTag("Weapon"))
        {
            pController.addInteractable(other.gameObject);
        }
        */
		if (other.gameObject.CompareTag("Player")) {
			if (other.GetComponent<PlayerController_Rewired>().getState() == PlayerController_Rewired.playerStates.down) {
				Debug.Log("adding downed player");
				pController.addDownedPlayer(other.gameObject);
			}
		}
		if (other.gameObject.CompareTag("Destructable")) {
			if (other.GetComponent<DestructiblePart>().isIntact) {
				Debug.Log("adding destructable part");
				addDestructableVehiclePart(other.gameObject);
			}
		}

        // Pick Up
        //if (other.gameObject.CompareTag("Drops"))
        //{
        if (other.tag == "WallDrop")
        {
            wallInventory++;
            Destroy(other.gameObject);
        }
        else if(other.tag == "Drops" && !heldItem)
        {
            heldItem = other.GetComponent<ItemDrop>().item;
            floatItem();
            //pass durabilty if needed (POI)
            DurableConstruct construct = floatingItem.GetComponent<DurableConstruct>();
            if (construct != null)
            {
                construct.SetDurability(other.GetComponent<ItemDrop>().myItemDur);
            }

            Destroy(other.gameObject);
            myAni.SetBool("isHolding", true);
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
        
        Util.RemoveAll(nodes, other.gameObject);
        Util.RemoveAll(trapNodes, other.gameObject);
        Util.RemoveAll(engineNodes, other.gameObject);
        Util.RemoveAll(attackRange, other.gameObject);

        if (other.gameObject.CompareTag("Interactable")) {
			pController.removeInteractable(other.gameObject);
		}
		if (other.gameObject.CompareTag("Player")) {
			if (other.GetComponent<PlayerController_Rewired>().getState() == PlayerController_Rewired.playerStates.down) {
				Debug.Log("removing downed player");
				pController.removeDownedPlayer(other.gameObject);
			}
		}
		if (other.gameObject.CompareTag("Destructable")) {
			if (other.GetComponent<DestructiblePart>().isIntact) {
				Debug.Log("removing destructable part");
				removeDestructableVehiclePart(other.gameObject);
			}
		}
	}

    /// <summary>
    /// Change inventory in text only after building wall, saves overhead
    /// </summary>
    public void changeInventory()
    {
        inventoryText.text = wallInventory.ToString();
    }

    /// <summary>
    /// Using it for re-entering wall build mode
    /// </summary>
    private void checkHologram()
    {
        if(nodes.Count <= 1 && nodes.Count > 0) //previously didn't include > 0, but you need a node to pass
        {
            GameObject first = (GameObject)nodes[0];
            if (buildMode && heldItem == null)
            {
                if (nodes.Count <= 1 && !first.GetComponent<BuildNode>().occupied)
                {
                    first.GetComponent<BuildNode>().Show(wall);
                }
            }
        }
    }

    /// <summary>
    /// Shows Building mode text above the player
    /// Once "Building" is Officially no longer going to be displayed, DELETE function
    /// </summary>
    void displayMode()
    {
        if (buildMode) mode.text = "Building";
        else mode.text = " ";
        //mode.text = "Build Mode: " + buildMode;
    }

    /// <summary>
    /// Makes held item float and spin above player
    /// </summary>
    public void floatItem()
    {
        if (!hasItem)
        {
            GameObject myFloat = null;
            if (heldItem != null) {
                myFloat = heldItem;
                floatingItem = Instantiate(myFloat, //fix later for prettier
                new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), transform.parent.rotation, transform.parent);
            } else { //Else hold a wall piece
                myFloat = wall;
                floatingItem = Instantiate(myFloat, //fix later for prettier
                new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), transform.parent.rotation, transform.parent);
                //wall is too big to carry, scaling down
                floatingItem.transform.localScale *= 0.7f;
            }
            floatingItem.transform.localPosition = new Vector3(0f, 1.1f, 0.5f); //NEED SOLUTION FOR ALL CHARACTER SIZES

            hasItem = true;
            floatingItem.tag = "Untagged";
            floatingItem.GetComponentInChildren<BoxCollider>().enabled = false;
        }

    }

    /// <summary>
    /// Set the player ID for Rewired.
    /// </summary>
    /// <param name="id">The player ID</param>
    public void SetId(int id)
    {
        playerId = id;
        initialized = false;
    }

    /// <summary>
    /// Add the given destructable part to an internal list
    /// </summary>
    /// <param name="p">The part to add</param>
	public void addDestructableVehiclePart(GameObject p) {
		destructableParts.Add(p);
	}

    /// <summary>
    /// Remove the given destructable part from the internal list
    /// </summary>
    /// <param name="p">The part to remove</param>
	public void removeDestructableVehiclePart(GameObject p) {
		destructableParts.Remove(p);
	}

    /// <summary>
    /// Puts away the player's weapon
    /// </summary>
    public void SheathWeapon(){
        myWeapon.SetActive(false);
        sheathTimer = 0f;
    }
}
