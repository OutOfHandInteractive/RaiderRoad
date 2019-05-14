using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController_Rewired : MonoBehaviour
{
    public enum playerStates { up, down };

	#region Variable Declarations
	// ----------------------- Public Variables ---------------------------
	public float basehealth;

    public int playerId = 0;

    public float moveSpeed = 10f;
	public bool isFacingVertical;

	public GameObject view;
    public GameObject jumpIndicator;

    public float jumpIndicatorScaling = 10f;

    public float jumpForce;
    public float counterJumpForce;
    public float distToGround = 0.9f;
    public bool isOccupied = false;

    //particles
    public ParticleSystem landingPart;

    // --------------------------------------------------------------------


    // ----------------------- Private Variables --------------------------
    private Player player;
	private PlayerPlacement_Rewired pPlacement;

	// Movement
	private Vector2 moveVector;
	private float angle;
    private Vector3 rotateVector;

    private Rigidbody rb;
    //Animator
    public Animator myAni;
    public float prevMoveVal; //used to polish movement transitions (in animations)
    private GameManager g;
    private bool myPauseInput = false;

	// health
    public float currentHealth;
	[SerializeField] private float hp5;	// health regen per 5 seconds
	[SerializeField] private float healthRegenDelay;
	private float healthRegenDelayCountdown;
	[SerializeField] private float reviveTime;
	public float reviveCountdown;
    //Invulnerability
    [SerializeField] private float invulTime;
    private float lastHitTimeStamp = 0f;

	private float baseJumpInidicatorScale;
    private float baseJumpIndicatorDist;

	// UI
	[SerializeField] private GameObject reviveHelpIcon;

    // states and flags
    public bool paused = false;
    public bool reviving = false;
    public bool beingRevived = false;
    public playerStates state;


    // object interaction
    public bool interacting = false;
	private Interactable objectInUse;
    private List<GameObject> interactables = new List<GameObject>();
    private List<GameObject> downedPlayers = new List<GameObject>();

    // [temporary] materials
    public Material myMat;
    private Color myOrigColor;

    private bool jumped = false;
    private bool jumpHeld = false;
    private bool exploded = false;
    private bool animJumped = false; //Jumped can be called at weird points, animJumped is a more accurate version for animation/particle purposes

	// audio
	PlayerAudio myAudio;

	// ----------------------------------------------------------------------
	#endregion

	[System.NonSerialized]
    private bool initialized;

	#region System Functions
	void Start()
    {
        currentHealth = basehealth;
        state = playerStates.up;
        reviveCountdown = 0;
        rb = gameObject.GetComponent<Rigidbody>();
        baseJumpInidicatorScale = jumpIndicator.transform.localScale.x;
        baseJumpIndicatorDist = Vector3.Distance(transform.position, jumpIndicator.transform.position);

        //find current color of player
        if (myMat != null) myOrigColor = myMat.color;

        //find animator
        myAni = gameObject.GetComponentInChildren<Animator>();

        //Get game manager for reference
        g = GameManager.GameManagerInstance;

		// get reference to PlayerPlacement
		pPlacement = view.GetComponent<PlayerPlacement_Rewired>();

		// get reference for audio player
		myAudio = GetComponentInChildren<PlayerAudio>();
	}

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        pPlacement.SetId(playerId);

        initialized = true;
    }

    void Update() {
        if (currentHealth <= 0) {
            state = playerStates.down;
        }

        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.

		if (!initialized) Initialize(); // Reinitialize after a recompile in the editor

        if (g != null) {
            myPauseInput = g.GetComponent<GameManager>().pauseInput;
        }

        if (!interacting && state == playerStates.up && !myPauseInput) {
            GetInput();
            ProcessInput();
        }

		// ------------- "Maintenance" ------------
        ScaleJumpIndicator();
		HealthRegen();
    }

    void FixedUpdate()
    {
        // ---- jumping ----
        if (jumped && !exploded)
        {
            if (!jumpHeld && Vector3.Dot(rb.velocity, transform.up) > 0)
            {
                rb.AddForce(transform.up * counterJumpForce * rb.mass);
            }
        }
    }
    #endregion

    #region Input and Input Processing
    private void GetInput() {
        // main input
        if (!paused && !interacting && !reviving) {
			// get movement directions and angles
            moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * moveSpeed;
            moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * moveSpeed;
            myAni.SetFloat("speed", moveVector.magnitude/ Time.deltaTime);

			//Single Stick Rotation
			if (!player.GetButton("Build Mode")) {
				rotateVector = Vector3.right * player.GetAxis("Move Horizontal") + Vector3.forward * player.GetAxis("Move Vertical");
				
				// adjust player facing angle if they are moving
				if (moveVector.magnitude > 0) {
					angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;

					if (!interacting && state == playerStates.up && !reviving && IsGrounded()) {
						myAudio.StartWalking();
					}
					else {
						myAudio.StopWalking();
					}
				}
				else {
					myAudio.StopWalking();
				}
			}

			// determine if the player is facing vertically or horizontally for wall placement
			// adjust isFacingVertical accordingly, remove "wrong direction" nodes from view if
			// changing from vertical to horizontal or vice versa
			if ((angle >= Constants.FACING_VERTICAL_MINIMUM && angle <= Constants.FACING_VERTICAL_MAXIMUM) ||
				(angle <= -Constants.FACING_VERTICAL_MINIMUM && angle >= -Constants.FACING_VERTICAL_MAXIMUM)) {
				if (!isFacingVertical) {
					pPlacement.removeWrongDirectionWallNodes();
				}
				isFacingVertical = true;
			}
			else {
				if (isFacingVertical) {
					pPlacement.removeWrongDirectionWallNodes();
				}
				isFacingVertical = false;
			}

			// ---- standard interaction ----
            if (player.GetButtonDown("Use")) {
                Debug.Log("pressing button");
                if (downedPlayers.Count > 0) {
                    pPlacement.SheathWeapon();
                    startRevive(downedPlayers[0].GetComponent<PlayerController_Rewired>());
                }
                else if (interactables.Count > 0 && !interactables[0].GetComponent<Interactable>().isOnCooldown()) {
                    pPlacement.SheathWeapon();
                    if (!interactables[0].GetComponent<Interactable>().Occupied()) {
                        interactables[0].GetComponent<Interactable>().Interact(this);
						Debug.Log("trying to interact");
                    }
                }
            }

            /*
			// ---- jumping ----
            Debug.DrawRay(transform.position + Vector3.up, -Vector3.up * (distToGround + 0.1f), Color.red);
            if (player.GetButtonDown("Jump") && IsGrounded() && jumped == false) {
                rb.AddForce(transform.up * jumpForce);
                myAni.SetTrigger("jump");
                myAni.SetBool("land", false);
                jumped = true;
                animJumped = true;
            }
            */

            // ---- jumping ----
            Debug.DrawRay(transform.position + Vector3.up, -Vector3.up * (distToGround + 0.1f), Color.red);
            if (player.GetButtonDown("Jump"))
            {
                jumpHeld = true;
                if (IsGrounded())
                {
                    jumped = true;
                    animJumped = true;
                    rb.AddForce(transform.up * jumpForce * rb.mass, ForceMode.Impulse);
                    myAni.SetTrigger("jump");
                    myAni.SetBool("land", false);
                }
            }
            else if (player.GetButtonUp("Jump"))
            {
                jumpHeld = false;
            }
        }

        // reviving other players
        if (player.GetButton("Use") && reviving) {
			if (downedPlayers.Count == 0) {	// if there are no downed players in range, stop the rez
				stopRevive(null);
			}

            reviveCountdown -= Time.deltaTime;
            if (reviveCountdown <= 0) {
                revive(downedPlayers[0].GetComponent<PlayerController_Rewired>());
                removeDownedPlayer(downedPlayers[0]);
            }
        }
        else if (player.GetButtonUp("Use") && reviving) {
            stopRevive(downedPlayers[0].GetComponent<PlayerController_Rewired>());
        }
    }

    private void ProcessInput() {
        if (moveVector.x != 0.0f || moveVector.y != 0.0f) {
            transform.Translate(moveVector.x, 0, moveVector.y, Space.World);
        }

        if (rotateVector.sqrMagnitude > 0.0f) {
            transform.rotation = Quaternion.LookRotation(rotateVector, Vector3.up);
        }
    }
	#endregion

	#region Jump Indicator
	private float map(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void ScaleJumpIndicator() {
        // Scale
        float dist = Vector3.Distance(transform.position, jumpIndicator.transform.position);
        dist = map(dist, baseJumpIndicatorDist, 3.0f, 1.0f, jumpIndicatorScaling);
        jumpIndicator.transform.localScale = new Vector3(baseJumpInidicatorScale / dist, baseJumpInidicatorScale / dist, baseJumpInidicatorScale / dist);

        // Follow
        jumpIndicator.transform.position = new Vector3(transform.position.x, jumpIndicator.transform.position.y, transform.position.z);

        // On ground
        int layerMask = ~((1 << 2) | (1 << 10)); // Ignore Layer NavMesh
        RaycastHit hit;
        //Debug.DrawRay(transform.position, -Vector3.up, Color.green);
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), -Vector3.up, out hit, Mathf.Infinity, layerMask)) {
            Vector3 pos = hit.point + hit.normal * 0.05f;
            jumpIndicator.transform.position = pos;
            //jumpIndicator.transform.position = new Vector3(jumpIndicator.transform.position.x, pos, jumpIndicator.transform.position.z);
        }
    }
	#endregion

	#region Detection Functions
	private void OnCollisionEnter(Collision collision) {
        if (IsGrounded() && animJumped == true) {
            animJumped = false;
            myAni.SetBool("land", true);
            Instantiate(landingPart, transform.position, landingPart.gameObject.transform.rotation);
        }
        jumped = false;

        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "RV" || collision.gameObject.tag == "eVehicle") {
            //Debug.Log("Can jump");
            transform.parent = collision.transform.root;
            jumped = false;
            exploded = false;
            //rb.isKinematic = true;
        }

        if (collision.gameObject.tag == "road") {
            takeDamage(Constants.PLAYER_ROAD_DAMAGE);
            transform.position = GameObject.Find("player1Spawn").transform.position;
        }
    }

    private void OnCollisionExit(Collision collision) {
        /*if (collision.gameObject.tag == "eVehicle") {
            //Debug.Log("Can jump");
            transform.parent = null;
        }

        /*
        if(collision.gameObject.tag == "floor")
        {
            rb.isKinematic = false;
        }
        */
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position + Vector3.up, -Vector3.up, distToGround + 0.1f);
    }
	#endregion

	#region Health Related Functions
	// ----------------------- reviving ----------------------------
	public void startRevive(PlayerController_Rewired p) {
        reviving = true;
        reviveCountdown = reviveTime;
        p.GetComponentInChildren<HealthBar_Player>().startRevive(reviveTime);

        moveVector.x = 0f;  //zero movement so you don't keep walking while revive
        moveVector.y = 0f;
        myAni.SetFloat("speed", moveVector.magnitude);
    }

    public void stopRevive(PlayerController_Rewired p) {
        reviving = false;

		if (p) {	// allows us to kill the revive if there is no player
			p.GetComponentInChildren<HealthBar_Player>().stopRevive();
		}
    }

    public void revive(PlayerController_Rewired p) {
		p.getUp();
        reviving = false;
    }

	// ----------------------- damage -----------------------
    public void takeDamage(float _damage) {
        float currTime = Time.time;
        //Invulnerability Frames (if we want to remove, set invulTime to 0 or get rid of if statement below)
        if (currTime >= lastHitTimeStamp + invulTime) {
            lastHitTimeStamp = Time.time;
            currentHealth -= _damage;
            healthRegenDelayCountdown = healthRegenDelay;

			myAudio.PlaySound_DamageByRaider();

            if (currentHealth <= 0)
            {
                goDown();
            }
        } else {
            //Debug.Log("Invulnerable Hit");
        }
    }

    public void RoadRash()
    {
        takeDamage(Constants.PLAYER_ROAD_DAMAGE);
        transform.position = GameObject.Find("player1Spawn").transform.position;
    }

	private void goDown() {
		pPlacement.SheathWeapon();
		pPlacement.dropItem();
		myAni.SetBool("downed", true);

		reviveHelpIcon.SetActive(true);

		state = playerStates.down;
		if (interacting) {
			objectInUse.Leave();
		}
		g.PlayerDowned();
	}

	public void getUp() {
		currentHealth = basehealth;
		backToOrigAnim();
		setState(playerStates.up);
		gameObject.GetComponentInChildren<HealthBar_Player>().stopRevive();

		reviveHelpIcon.SetActive(false);
	}

    // ------------------- misc health ---------------------
    private void HealthRegen() {
        if (state != playerStates.down) { 
            if (currentHealth < basehealth) {
                if (healthRegenDelayCountdown <= 0) {
                    currentHealth += ((hp5 / 5) * Time.deltaTime);

                    if (currentHealth > basehealth) {
                        currentHealth = basehealth;
                    }
                }
                else {
                    healthRegenDelayCountdown -= Time.deltaTime;
                }
            }
        }
	}
	#endregion

	#region Getters and Setters
	// ------------------ general player functions -------------------
	public void SetId(int id) {
        playerId = id;
        initialized = false;
    }

    public int GetId() {
        return playerId;
    }

    public Player GetPlayer() {
        return player;
    }

    public playerStates getState() {
        return state;
    }

    public void setState(playerStates s) {
        state = s;
    }

	// ---------------------- health ---------------------
    public float getMaxHealth() {
        return basehealth;
    }

    public float getHealth() {
        return currentHealth;
    }

	// ---------------------- interaction -----------------------
	// ---- setting flags ----
	public void setInteractingFlag() {
		interacting = true;
	}

	public void unsetInteractingFlag() {
		interacting = false;
	}

	// ---- adding/removing ----
	public void addInteractable(GameObject i) {
        if (!interactables.Contains(i)) {
            Debug.Log("added weapon");
            interactables.Add(i);
        }
    }

    public void removeInteractable(GameObject i) {
        Debug.Log("removed weapon");
        interactables.Remove(i);
    }

	public Interactable getFirstInteractable() {
		return interactables[0].GetComponent<Interactable>();
	}

	public void setObjectInUse(Interactable obj) {
		objectInUse = obj;
	}

    public void clearInteractable() {
        interactables.Clear();
        //Debug.Log("AHHHHHHHHHHHHHHH" + interactables.Count);
    }

    public void addDownedPlayer(GameObject p) {
        downedPlayers.Add(p);
    }

    public void removeDownedPlayer(GameObject p) {
        downedPlayers.Remove(p);
    }

	// ------------------------- animations -------------------------
	//Temporary anim function, needs features for full hand follows
	public void interactAnim(bool animStat) {
		myAni.SetBool("aimWeapon", animStat);
		myAni.SetFloat("speed", 0f);
	}

	public void backToOrigAnim() {
        myAni.SetBool("downed", false);
    }
    #endregion

    #region Other Methods
    public void Eject(float zSign) {
        takeDamage(Constants.PLAYER_ROAD_DAMAGE);

        float gravity = Physics.gravity.magnitude;

        //Positions of this object and the target on the same plane
        int rand = Random.Range(1, 5);
        Vector3 pos;
        switch (rand)
        {
            case 1:
                pos = GameObject.Find("player1Spawn").transform.position;
                break;

            case 2:
                pos = GameObject.Find("player2Spawn").transform.position;
                break;

            case 3:
                pos = GameObject.Find("player3Spawn").transform.position;
                break;

            default:
                pos = GameObject.Find("player4Spawn").transform.position;
                break;
        }
        Vector3 planePos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 planeTar = new Vector3(pos.x, 0, pos.z);

        //Selected angle in radians
        float angle = 60f *Mathf.Deg2Rad;

        //Planar distance between objects
        float distance = Vector3.Distance(planeTar, planePos);
        //Distance along the y axis between objects
        float yOffset = transform.position.y - pos.y;

        //Equation to get initial velocity
        // vi = (1/cos(theta)) * sqrt((g * d^2 /2)/(d*tan(theta)+y))
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));


        //Use positive velocity if vehicle is on left side, negative otherwise
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), zSign * initialVelocity * Mathf.Cos(angle));

        //Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, (planeTar - planePos) * zSign);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        rb.AddForce(finalVelocity * rb.mass, ForceMode.Impulse);
        jumped = true;
        exploded = true;
        animJumped = true;
        myAni.SetTrigger("jump");
        myAni.SetBool("land", false);
    }

	public void StopWalkingAudio() {
		myAudio.StopWalking();
	}
    #endregion
}