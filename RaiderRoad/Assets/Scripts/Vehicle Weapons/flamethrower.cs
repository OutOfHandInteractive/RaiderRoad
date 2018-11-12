using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class flamethrower : Interactable {
    
	// ------------------------- public variables ---------------------------
	// references
	public GameObject flame;
	public GameObject reticule;
	public GameObject barrel;
	public GameObject weapon;
    public Text overheat;
    public Material normalMat;
    public Material overheatMat;
    
	// gameplay values
	public float reticuleMoveSpeed;
    public float coneAngle;
	public float overheatTime;
	public float overheatCooldown;

	// ----------------------------------------------------------------------

	// ----------------------
	// Private variables
	// ----------------------
	private bool paused = false;
	private Player player;
	private Vector2 moveVector;
    private bool overheated = false;
    private float overheatCount;
    private float cooldownCount;
    private bool firing = false;
    private float newAngle;
    
    
	[System.NonSerialized]
        private bool initialized;
    
	// Use this for initialization
	void Start () {
		inUse = false;
		user = null;
		userPlayerId = -1;
        barrel.GetComponent<MeshRenderer>().material = normalMat;
        overheatCount = overheatTime;
        cooldownCount = overheatCooldown;
        overheated = false;
        firing = false;
        cooldownTimer = cooldown;
    }
    
	// Update is called once per frame
	void Update () {
		weapon.transform.LookAt(reticule.transform);
        if (isOnCooldown())
        {
            cooldownTimer -= Time.deltaTime;
        }
        
        GetInput();
		ProcessInput();
        
        CheckOverheat();
	}
    
	private void GetInput() {
		if (!paused && inUse) {
			moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * reticuleMoveSpeed;
			moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * reticuleMoveSpeed;
            
			if (player.GetButtonDown("Exit Interactable")) {
				Leave();
			}
            
			if (player.GetButtonDown("Shoot Weapon") && !overheated)
            {
                flame.SetActive(true);
                firing = true;
			}
            if (player.GetButtonUp("Shoot Weapon"))
            {
                flame.SetActive(false);
                firing = false;
            }
        }
	}
    
	private void ProcessInput() {
		// Old Reticule
        /*if (moveVector.x != 0.0f || moveVector.y != 0.0f) {
            if (reticule.transform.position.z + moveVector.y >= transform.position.z + range)
            {
                reticule.transform.position = new Vector3 (reticule.transform.position.x, reticule.transform.position.y, reticule.transform.position.z - 0.001f);
            }
            else if (reticule.transform.position.z + moveVector.y <= transform.position.z - range)
            {
                reticule.transform.position = new Vector3(reticule.transform.position.x, reticule.transform.position.y, reticule.transform.position.z + 0.001f);
            }
            else
            {
                reticule.transform.Translate(0, 0, moveVector.y, Space.World);
            }
			
  }*/
        
        // New Reticule
        // If the player has given input, move the reticule accordingly
		if (moveVector.x != 0.0f || moveVector.y != 0.0f) {
			reticule.transform.Translate(0, 0, moveVector.y, Space.World);
			newAngle = Mathf.Atan((reticule.transform.localPosition.z) / (reticule.transform.localPosition.x));
		}
		//Debug.Log("x: " + reticule.transform.localPosition.x + " z: " + reticule.transform.localPosition.z + " angle: " + newAngle);
        
		// Clamp x (opposite leg) transform between -tan(angle)*z and tan(angle)*z
		// Clamp z (adj. leg) between 0 and maxRange - tan(pi/2 - (pi - (pi/2 + newAngle)))*reticuleX
		reticule.transform.localPosition = new Vector3(reticule.transform.localPosition.x, 0,
                                                       Mathf.Clamp(reticule.transform.localPosition.z, reticule.transform.localPosition.x * Mathf.Tan(coneAngle * Mathf.Deg2Rad), reticule.transform.localPosition.x * Mathf.Tan(-coneAngle * Mathf.Deg2Rad)));
        
        
        /*if (reticule.activeSelf == true)
        {
            transform.LookAt(reticule.transform);
        }*/
	}
    
    void CheckOverheat() 
    {
        if (firing)
        {
            overheatCount -= Time.deltaTime;
        }
        else if (!overheated && overheatCount < overheatTime)
        {
            overheatCount += Time.deltaTime;
        }
        else if (overheatCount >= overheatTime)
        {
            overheatCount = overheatTime;
        }
        
        
        if (overheatCount <= 0.0f)
        {
            overheated = true;
            firing = false;
            flame.SetActive(false);
            barrel.GetComponent<MeshRenderer>().material = overheatMat;
            cooldownCount = overheatCooldown;
            overheatCount = overheatTime;
        }
        
        if (overheated)
        {
            cooldownCount -= Time.deltaTime;
        }
        
        if (cooldownCount <= 0.0f)
        {
            overheated = false;
            barrel.GetComponent<MeshRenderer>().material = normalMat;
            cooldownCount = overheatCooldown;
            overheatCount = overheatTime;
        }
        
        if (!overheated)
        {
            overheat.text = overheatCount.ToString("F2");
        }
        else
        {
            overheat.text = cooldownCount.ToString("F2");
        }
    }
    
	// ------------------- Interaction Methods ---------------------
    
	public override void Interact(PlayerController_Rewired pController) {
		user = pController;
		player = user.GetPlayer();
		userPlayerId = user.playerId;
		user.setInteractingFlag();
        
		inUse = true;
		reticule.SetActive(true);
	}
    
	public override void Leave() {
        cooldownTimer = cooldown;
        user.unsetInteractingFlag();
		inUse = false;
		reticule.SetActive(false);
	}
}
