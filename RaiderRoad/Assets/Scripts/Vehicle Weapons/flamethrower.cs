using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class flamethrower : Interactable {
    
	// ------------------------- public variables ---------------------------
	// references
	public GameObject reticule;
	public GameObject barrel;
	public GameObject weapon;
    public Text overheat;
    public Material normalMat;
    public Material overheatMat;
	public GameObject fireFX;
	public GameObject damageCollider;
    
	// gameplay values
	public float reticuleMoveSpeed;
    public float coneAngle;
	public float overheatTime;
	public float overheatCooldown;
	public float tickDamage;	// damage between ticks
	public float tickTime;		// amount of seconds between ticks

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
    private bool interacting = false;
	private ParticleSystem fireInstance;
    
	[System.NonSerialized]
        private bool initialized;

	void Awake() {
		fireInstance = Instantiate(fireFX, barrel.transform.position, fireFX.transform.rotation, barrel.transform).GetComponent<ParticleSystem>();
		fireInstance.Stop();
	}

	// Use this for initialization
	void Start () {
		inUse = false;
		user = null;
		userPlayerId = -1;
        //barrel.GetComponent<MeshRenderer>().material = normalMat;
        overheatCount = overheatTime;
        cooldownCount = overheatCooldown;
        overheated = false;
        firing = false;
        cooldownTimer = cooldown;

		damageCollider.GetComponent<flamethrowerDamage>().setTickDamage(tickDamage);
		damageCollider.GetComponent<flamethrowerDamage>().setTickTime(tickTime);
	}
    
	// Update is called once per frame
	void Update () {
		weapon.transform.LookAt(reticule.transform);
		fireInstance.transform.LookAt(reticule.transform);
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
            
			if (player.GetButtonDown("Exit Interactable") && interacting) {
				Leave();
                Debug.Log("Left Flamethrower");
			}
            
			if (player.GetButtonDown("Shoot Weapon") && !overheated)
            {
				fireInstance.Play();
                firing = true;
				damageCollider.SetActive(true);
			}
            if (player.GetButtonUp("Shoot Weapon"))
            {
				fireInstance.Stop();
                firing = false;
				damageCollider.SetActive(false);
			}
            
            if (reticule.activeSelf == true) {
                interacting = true;
            }
        }
	}
    
	private void ProcessInput() {  
        // New Reticule
        // If the player has given input, move the reticule accordingly
		if (moveVector.x != 0.0f || moveVector.y != 0.0f) {
			reticule.transform.Translate(0, 0, moveVector.y, Space.World);
			newAngle = Mathf.Atan((reticule.transform.localPosition.z) / (reticule.transform.localPosition.x));
		}
		//Debug.Log("x: " + reticule.transform.localPosition.x + " z: " + reticule.transform.localPosition.z + " angle: " + newAngle);

		// Clamp x (opposite leg) transform between -tan(angle)*z and tan(angle)*z
		// Clamp z (adj. leg) between 0 and maxRange - tan(pi/2 - (pi - (pi/2 + newAngle)))*reticuleX
		reticule.transform.localPosition = new Vector3(
			Mathf.Clamp(reticule.transform.localPosition.x, reticule.transform.localPosition.z * Mathf.Tan(-coneAngle * Mathf.Deg2Rad), reticule.transform.localPosition.z * Mathf.Tan(coneAngle * Mathf.Deg2Rad)), 
			0,
			reticule.transform.localPosition.z);
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
			damageCollider.SetActive(false);
			fireInstance.Stop();
			//weapon.GetComponent<MeshRenderer>().material = overheatMat;
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
            //weapon.GetComponent<MeshRenderer>().material = normalMat;
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
        interacting = false;
	}
}
