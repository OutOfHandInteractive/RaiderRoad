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

    //particles
    public ParticleSystem charaHitPart;
    public ParticleSystem objHitParticle;

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
    [SerializeField] private Transform myHoldObj;

    private PlayerAudio myAudio;

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

	#region System Functions
	private void Start() {
        pController = GetComponentInParent<PlayerController_Rewired>();
	}

	void Initialize() {
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

        myAudio = GetComponent<PlayerAudio>();

        g = GameManager.GameManagerInstance;
    }

    void Update() {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor
        changeInventory();
        //displayMode(); //display "building" text

        if(!(g == null)) {
            myPauseInput = g.GetComponent<GameManager>().pauseInput;
        }

        if (!myPauseInput) {
            // Attack cooldown
            if (!canAttack) {
                attackCount -= Time.deltaTime;
            }

            if (attackCount <= 0.0) {
                canAttack = true;
            }

            //Sheath Weapon
            if(sheathTimer > 0f) {
                sheathTimer -= Time.deltaTime;
                if(sheathTimer < sheathAnimTime) {
                    //myAni.SetTrigger("sheathWeapon");
                    myWeapon.transform.localScale = MeleeWeapScale * Mathf.SmoothStep(1f, 0f, sheathAnimTime - sheathTimer);
                }
            }
			else {
                myWeapon.SetActive(false);
            }

            myInteracting = pController.interacting;
            //checking that player isn't "interacting" (driving, piloting weapon, etc)
            if (myInteracting) {
                // Early exit
                return;
            }

            if (heldItem != null) { //change this to tags later
                HoldingItem();
            }
            else {
                NotHoldingItem();
            }

            if (currentAttColor.a > 0) {
                currentAttColor.a -= 1f * Time.deltaTime; //transitioning color from visibile to invisible
                TempAttMat.color = currentAttColor;
            }
            else if (currentAttColor.a < 0) { // getting alpha to exactly 0, and after that won't check further
                currentAttColor.a = 0;
                TempAttMat.color = currentAttColor;
            }
        }
    }
	#endregion

	#region Building Functions
	private void BuildDurableConstruct(DurabilityBuildNode node) {
        if (!node.occupied) {
            //myAni.SetTrigger("build");
            GameObject obj = node.Build(heldItem, floatingItem.GetComponent<DurableConstruct>().GetDurability());
            SetSpringTrapPosition(obj);
            heldItem = null;
            hasItem = false;
            Destroy(floatingItem);
            buildMode = false;

            //Tell node that battery is placed 
            node.GetComponent<PoiNode>().PoiPresent();

            myAni.SetTrigger("build");
            myAni.SetBool("isHolding", false);
        }
        else {
            //Healing Mechanic, if occupied, heal for what health is missing
            if(node.GetComponentInChildren<Engine>() != null) {
                Engine targetEngine = node.GetComponentInChildren<Engine>();

                if(targetEngine.currentDurVal() < targetEngine.durability)
                { // healing existing engine
                    targetEngine.EngineHeal(floatingItem.GetComponent<DurableConstruct>().GetDurability());
                    heldItem = null;
                    hasItem = false;
                    Destroy(floatingItem);
                    buildMode = false;

                    myAni.SetTrigger("build");
                    myAni.SetBool("isHolding", false);

                } else {
                    // if engine is at full health, do not heal
                    node.GetComponent<PoiNode>().PoiFlash(Color.gray);

                }
            }
        }
    }

    private void BuildTrap() {
        GameObject trapBuild = trapNodes[0];
        //Debug.Log(trapBuild);
        BuildDurableConstruct(trapBuild.GetComponent<TrapNode>());

        myAni.SetTrigger("build");
        myAni.SetBool("isHolding", false);
    }

	private void SetSpringTrapPosition(GameObject obj) {
		SpringTrap trap = obj.GetComponent<SpringTrap>();
		if (trap != null) {
			trap.SetRotation(gameObject);
		}
	}

	private void BuildEngine() {
        GameObject EngineBuild = engineNodes[0];
        BuildDurableConstruct(EngineBuild.GetComponent<PoiNode>());

    }

    private void BuildWeapon(GameObject toBuild) {
        if (!toBuild.GetComponent<BuildNode>().occupied) {
            myAni.SetTrigger("build");
            myAni.SetBool("isHolding", false);
            toBuild.GetComponent<BuildNode>().Build(heldItem, toBuild);
            heldItem = null;
            hasItem = false;
            Destroy(floatingItem);
            buildMode = false;
            //other.gameObject.SetActive (false);
        }
        else {
            Debug.Log("Occupied >:(");
        }
    }

	private void BuildWall() {
		GameObject toBuild = nodes[0];
		if (!toBuild.GetComponent<BuildNode>().occupied) {
			myAni.SetTrigger("build");
			toBuild.GetComponent<BuildNode>().Build(wall, toBuild);
			wallInventory--;
			changeInventory();
			//other.gameObject.SetActive (false);
		}
		else {
			Debug.Log("Occupied >:(");
		}
	}
	#endregion

	#region Item Holding Functions
	private void HoldingItem() {
		floatItem();
		SheathWeapon();
		if (player.GetButtonDown("Place Object")) {
			if (heldItem.tag == "Trap" && trapNodes.Count > 0) {
				BuildTrap();
			}
			else if (heldItem.tag == "Engine" && engineNodes.Count > 0) {
				BuildEngine();
			}
			else if (heldItem.tag == "Weapon" && nodes.Count > 0) { // to change later?
				foreach (GameObject node in nodes) {
					if (node.GetComponent<BuildNode>().canPlaceWeapon) {
						BuildWeapon(node);
						break;
					}
				}
			}
		}

		if (player.GetButton("Use")) {
			//GETTING RID OF HOLD TO DROP
			//holdTime += Time.deltaTime;
			//if (holdTime > timeToDrop)
			//{
			dropItem();
			//}
		}
	}

	/// <summary>
	/// Drops the currently held item, if any.
	/// </summary>
	public void dropItem() {
		if (heldItem != null) {
			GameObject dropItem = heldItem.GetComponent<Constructable>().drop;
			//if (heldItem.tag == "Trap") dropItem = heldItem.GetComponent<Trap>().drop; //get the drop prefab item from item's script
			//if (heldItem.tag == "Engine") dropItem = heldItem.GetComponent<Engine>().drop;
			// more ifs for other items
			GameObject item = Instantiate(dropItem, transform.parent.position + new Vector3(0, 0.3f, 0) + transform.parent.forward * 1.7f, Quaternion.identity, transform.parent.parent);
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

	private void NotHoldingItem() {
        if (player.GetButton("Build Mode") && pController.state == PlayerController_Rewired.playerStates.up && wallInventory>0)  {
            buildMode = true;

            checkHologram();
            SheathWeapon();
            myAni.SetBool("isHolding", true);
        }
		else {
            LeaveBuildMode();
        }

        if (buildMode) {
            floatItem(); //float a wall piece instead

            if (wallInventory > 0 && player.GetButtonDown("Place Object") && nodes.Count > 0) {
                BuildWall();
            }
        }
        else if (player.GetButtonDown("Attack") && canAttack && pController.state == PlayerController_Rewired.playerStates.up) {
            Attack();
        }
    }

    private void LeaveBuildMode() {
        if (!buildMode) {
            return;
        }
        //When switching out of build mode, attack will get stuck in InvalidOperationException: List has changed. This helps
        //attackRange = new List<GameObject>();
        if (nodes.Count > 0) { //If showing holo wall, turn off every holo wall currently being displayed
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].GetComponent<BuildNode>().RemoveShow();
            }
        }
        buildMode = false;
        myAni.SetBool("isHolding", false);
        hasItem = false;
        Destroy(floatingItem);
    }

	public void floatItem() { //makes held item float and spin above player
        if (!hasItem)
        {
            GameObject myFloat = null;
            if (heldItem != null) {
                myFloat = heldItem;
                floatingItem = Instantiate(myFloat, //place and position under the myHoldObj inside characters hand joint
                new Vector3(myHoldObj.position.x, myHoldObj.position.y, myHoldObj.position.z), myHoldObj.rotation, myHoldObj);
            } else { //Else hold a wall piece
                myFloat = wall;
                floatingItem = Instantiate(myFloat, //place and position under the myHoldObj inside characters hand joint
                new Vector3(myHoldObj.position.x, myHoldObj.position.y, myHoldObj.position.z), myHoldObj.rotation, myHoldObj);
            }

            hasItem = true;
            floatingItem.tag = "Untagged";
            floatingItem.GetComponentInChildren<BoxCollider>().enabled = false;
        }
	}
	#endregion

	#region Combat Functions
	private bool AttackVehicleParts() {
        foreach(GameObject part in destructableParts) {
            if(part!=null) {
                Instantiate(objHitParticle, transform.position, Quaternion.identity); //temporary solution, also placed slightly to left for some reason
                if (part.GetComponent<DestructiblePart>().TakeDamage(10) <= 0) {	// MAGIC NUMBER ALERT
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

		//Debug.Log("attackRange Count:" + attackRange.Count);
		bool hit = AttackVehicleParts();
        if (!hit) {
            Util.RemoveNulls(attackRange);
            foreach (GameObject item in attackRange) {
                //Debug.Log(item);
                Constructable construct;

                if(item == null) {
                    // This should've been removed!!
                    continue;
                }

                if (item.CompareTag("Weapon")) {
                    item.GetComponent<Weapon>().Damage(damage, gameObject.transform.parent.gameObject);
                    Instantiate(objHitParticle, item.transform.position, Quaternion.identity);
                    hit = true;
                }
                else if ((construct = item.GetComponent<Constructable>()) != null) {
                    construct.Damage(damage);
                    Instantiate(objHitParticle, item.transform.position, Quaternion.identity);
                    hit = true;
                }
                else if (item.CompareTag("Enemy")) {
                    Vector3 dir = item.transform.position - transform.parent.position;
                    dir = Vector3.Normalize(new Vector3(dir.x, 0.0f, dir.z));
                    item.GetComponent<Rigidbody>().AddForce(dir * knockback_force);
                    item.GetComponent<StatefulEnemyAI>().takeDamage(damage);
                    item.GetComponent<StatefulEnemyAI>().Stunned();
                    //Debug.Log(dir);

                    Instantiate(charaHitPart, item.transform.position, Quaternion.identity);
                    hit = true;
                }
            }
        }

        myAudio.Swing(hit);

        // Remove comment lines to bring temp Red attack cube back
        //currentAttColor.a = 0.5f; //setting attack model's mat to 1/2 visible
    }

	public void SheathWeapon() {
		myWeapon.SetActive(false);
		sheathTimer = 0f;
	}
	#endregion

	#region Build Node Functions
	private void CheckBuildNodes() {

        bool isWall;
        GameObject item;
        if (heldItem == null && buildMode) {
            item = wall;
            isWall = wallInventory > 0;
        }
        else if (heldItem != null && heldItem.CompareTag("Weapon")) {
            item = heldItem;
            isWall = false;
        }
        else {
            return;
        }

        bool found = false;
        foreach (GameObject obj in nodes) {
            BuildNode node = obj.GetComponent<BuildNode>();
            node.RemoveShow();
            if (!found && !node.occupied) { //"&& (isWall || node.canPlaceWeapon)" at end of if
                node.Show(item);
                found = true;
            }
        }
    }

	public void removeWrongDirectionWallNodes() {
		if (heldItem != null && heldItem.CompareTag("Weapon")){
			/*foreach (GameObject node in nodes) {
			    if (node.gameObject.tag == "WallNodeHorizontal") {
				    nodes.Remove(node);
			    }
		    }*/
			nodes.RemoveAll(node => node.gameObject.tag == "WallNodeHorizontal");
        }
		else { //for walls
				 /*foreach (GameObject node in nodes) {
					 if (pController.isFacingVertical && node.gameObject.tag == "WallNodeHorizontal") {
						 nodes.Remove(node);
					 }
					 else if (!pController.isFacingVertical && node.gameObject.tag == "WallNodeVertical") {
						 nodes.Remove(node);
					 }
				 }*/
			nodes.RemoveAll(node => (pController.isFacingVertical && node.gameObject.tag == "WallNodeHorizontal")
			|| (!pController.isFacingVertical && node.gameObject.tag == "WallNodeVertical"));
		}
	}
	#endregion

	#region Detection Functions
	void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if ((other.tag == "WallNodeVertical" && pController.isFacingVertical) || 
			(other.tag == "WallNodeHorizontal" && !pController.isFacingVertical & (heldItem == null || !heldItem.CompareTag("Weapon")) ) || //need
            (heldItem != null && heldItem.CompareTag("Weapon") && other.tag == "WallNodeVertical"))
        {
            // This thing adds nodes to the view if they correspond to the facing direction of the player
			// or if they player is holding a weapon (so that weapon placement doesnt feel weird)
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
                if (buildMode) other.GetComponent<PoiNode>().Show(heldItem); //if player is in build mode, activate show engine in the build node script
            }
        }
        if (Util.isEnemy(other.gameObject))
        {
            attackRange.Add(other.gameObject);
        }
        else
        {
            Constructable construct = other.GetComponent<Constructable>();
            if (construct != null && !construct.isHolo && construct.isPlaced() && other.tag != "Engine") //other.tag check prevents hitting Engine, remove to break engine
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
        ItemDrop drop = other.GetComponent<ItemDrop>();
        if(drop != null && !drop.isTaken)
        {
            if (other.tag == "WallDrop")
            {
                drop.isTaken = true;
                wallInventory++;
                Destroy(other.gameObject);
            }
            else if (other.tag == "Drops" && !heldItem)
            {
                LeaveBuildMode();
                drop.isTaken = true;
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

#endregion

	public void changeInventory() //change inventory in text only after building wall, saves overhead
    {
        inventoryText.text = wallInventory.ToString();
    }

    //Using it for re-entering wall build mode
    private void checkHologram() {
        if(nodes.Count <= 1 && nodes.Count > 0) { // previously didn't include > 0, but you need a node to pass
			GameObject first = (GameObject)nodes[0];
            if (buildMode && heldItem == null) {
                if (nodes.Count <= 1 && !first.GetComponent<BuildNode>().occupied) {
                    first.GetComponent<BuildNode>().Show(wall);
                }
            }
        }
    }

    //Once "Building" is Officially no longer going to be displayed, DELETE function
    void displayMode()
    {
        if (buildMode) mode.text = "Building";
        else mode.text = " ";
        //mode.text = "Build Mode: " + buildMode;
    }

	#region Getters and Setters
	public void SetId(int id) {
        playerId = id;
        initialized = false;
    }

	public void addDestructableVehiclePart(GameObject p) {
		destructableParts.Add(p);
	}

	public void removeDestructableVehiclePart(GameObject p) {
		destructableParts.Remove(p);
	}
	#endregion
}
