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
	public GameObject damageCollider, damageColliderEnemy;
    public bool isOccupied = false;
    
	// gameplay values
	public float reticuleMoveSpeed;
    public float coneAngle;
	public float overheatTime;
	public float overheatCooldown;
	public float tickDamage, tickDamageEnemy;	// damage per ticks
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
	private flamethrowerDamage damage;
	private flamethrowerDamageEnemy damageEnemy;
    private AudioSource audio;
    
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
        overheatCount = overheatTime;
        cooldownCount = overheatCooldown;
        overheated = false;
        firing = false;
        cooldownTimer = cooldown;
        audio = GetComponent<AudioSource>();

		damage = damageCollider.GetComponent<flamethrowerDamage>();
		damage.setTickDamage(tickDamage);
		damage.setTickTime(tickTime);

		damageEnemy = damageColliderEnemy.GetComponent<flamethrowerDamageEnemy>();
		damageEnemy.setTickDamage(tickDamageEnemy);
		damageEnemy.setTickTime(tickTime);

		damageCollider.SetActive(false);
		damageColliderEnemy.SetActive(false);
	}
    
	// Update is called once per frame
	void Update () {
        if (inUse) {
            weapon.transform.LookAt(reticule.transform);
            fireInstance.transform.LookAt(reticule.transform);
        }
        if (isOnCooldown())
        {
            cooldownTimer -= Time.deltaTime;
        }
        
        GetInput();
		ProcessInput();
        
        CheckOverheat();
	}

    /// <summary>
    /// Begin firing using the anti-raider hitbox
    /// </summary>
    public void StartFiring()
    {
        StartFiring(damageCollider);
    }

    /// <summary>
    /// Start firing using the anti-player hitbox
    /// </summary>
	public void StartFiringEnemy()
    {
        StartFiring(damageColliderEnemy);
	}

    /// <summary>
    /// Stop firing
    /// </summary>
	public void StopFiringEnemy()
    {
        StopFiring();
	}
    
    private void StartFiring(GameObject collider)
    {
        audio.Play();
        fireInstance.Play();
        firing = true;
        collider.SetActive(true);
    }

    /// <summary>
    /// Stop firing
    /// </summary>
    public void StopFiring()
    {
        audio.Stop();
        fireInstance.Stop();
        firing = false;
        damageCollider.SetActive(false);
        damageColliderEnemy.SetActive(false);
    }

	public void SetRotation(Quaternion rot)
    {
        fireInstance.transform.rotation = rot;
    }

    /// <summary>
    /// Returns true if this flaethrower is overheated
    /// </summary>
    /// <returns>true if this flaethrower is overheated</returns>
    public bool isOverheated()
    {
        return overheated;
    }
    
	private void GetInput() {
		if (!paused && inUse) {
			moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * reticuleMoveSpeed;
			moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * reticuleMoveSpeed;
            
			if (player.GetButtonDown("Exit Interactable") && interacting) {
				Leave();
			}
            
			if (player.GetButtonDown("Shoot Weapon") && !overheated)
            {
                StartFiring();
			}
            if (player.GetButtonUp("Shoot Weapon"))
            {
                StopFiring();
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

	public void CheckOverheat() 
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
            StopFiring();
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

    public override bool Occupied()
    {
        return inUse;
    }

    public override void Leave() {
        cooldownTimer = cooldown;
        user.unsetInteractingFlag();
		inUse = false;
		reticule.SetActive(false);
        interacting = false;

		playerUsing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		interacting = false;
	}
}
