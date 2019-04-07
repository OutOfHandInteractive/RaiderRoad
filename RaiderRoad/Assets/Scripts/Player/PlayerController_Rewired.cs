using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController_Rewired : MonoBehaviour
{
    public enum playerStates { up, down };

    // ----------------------- Public Variables ---------------------------
    public float basehealth;

    public int playerId = 0;

    public float moveSpeed = 10f;
	public bool isFacingVertical;

	public GameObject view;
    public GameObject jumpIndicator;

    public float jumpIndicatorScaling = 10f;

    public float jumpForce;
    public float distToGround = 0.9f;
    public bool isOccupied = false;

    //particles
    public ParticleSystem landingPart;

    // --------------------------------------------------------------------


    // ----------------------- Private Variables --------------------------
    private Player player;
	private PlayerPlacement_Rewired pPlacement;
    private Vector2 moveVector;
	private float angle;

    private Vector3 rotateVector;

    [SerializeField] private float reviveTime;

    private Rigidbody rb;
    //Animator
    public Animator myAni;
    public float prevMoveVal; //used to polish movement transitions (in animations)
    private GameManager g;
    private bool myPauseInput = false;

    public float currentHealth;
    private float baseJumpInidicatorScale;
    private float baseJumpIndicatorDist;
    public float reviveCountdown;

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
    private bool animJumped = false; //Jumped can be called at weird points, animJumped is a more accurate version for animation/particle purposes

    // ----------------------------------------------------------------------


    [System.NonSerialized]
    private bool initialized;

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

	}

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        pPlacement.SetId(playerId);

        initialized = true;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            state = playerStates.down;
        }
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor

        if (g != null)
        {
            myPauseInput = g.GetComponent<GameManager>().pauseInput;
        }

        if (!interacting && state == playerStates.up && !myPauseInput)
        {
            GetInput();
            ProcessInput();
        }

        /*
        if (!IsGrounded())
        {
            rb.isKinematic = false;
        }
        */
        ScaleJumpIndicator();
    }

    private void GetInput()
    {
        // main input
        if (!paused && !interacting && !reviving)
        {
			// get movement directions and angles
            moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * moveSpeed;
            moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * moveSpeed;
            myAni.SetFloat("speed", moveVector.magnitude/ Time.deltaTime);

			// adjust player facing angle if they are moving
			if (moveVector.magnitude > 0) {
				angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
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
			
            //Twin Stick Rotation
            //rotateVector = Vector3.right * player.GetAxis("Rotate Horizontal") + Vector3.forward * player.GetAxis("Rotate Vertical");

            //Single Stick Rotation
            rotateVector = Vector3.right * player.GetAxis("Move Horizontal") + Vector3.forward * player.GetAxis("Move Vertical");

            if (player.GetButtonDown("Use"))
            {
                Debug.Log("pressing button");
                if (downedPlayers.Count > 0)
                {
                    pPlacement.SheathWeapon();
                    startRevive(downedPlayers[0].GetComponent<PlayerController_Rewired>());
                }
                else if (interactables.Count > 0 && !interactables[0].GetComponent<Interactable>().isOnCooldown())
                {
                    pPlacement.SheathWeapon();
                    if (!interactables[0].GetComponent<Interactable>().Occupied())
                    {
                        interactables[0].GetComponent<Interactable>().Interact(this);
						Debug.Log("trying to interact");
                    }
                }
            }

            Debug.DrawRay(transform.position + Vector3.up, -Vector3.up * (distToGround + 0.1f), Color.red);
            if (player.GetButtonDown("Jump") && IsGrounded() && jumped == false)
            {
                //rb.isKinematic = false;
                rb.AddForce(transform.up * jumpForce);
                myAni.SetTrigger("jump");
                myAni.SetBool("land", false);
                jumped = true;
                animJumped = true;
            }
        }

        // reviving functions
        if (player.GetButton("Use") && reviving)
        {
            reviveCountdown -= Time.deltaTime;
            if (reviveCountdown <= 0)
            {
                revive(downedPlayers[0].GetComponent<PlayerController_Rewired>());
                removeDownedPlayer(downedPlayers[0]);
            }
        }
        else if (player.GetButtonUp("Use") && reviving)
        {
            stopRevive(downedPlayers[0].GetComponent<PlayerController_Rewired>());
        }

        /*
        if (player.GetButton("Start"))
        {
            paused = true;
            player.controllers.maps.SetMapsEnabled(false, "Default");
            player.controllers.maps.SetMapsEnabled(true, "UI");
        }
        else if (player.GetButton("UIStart"))
        {
            paused = false;
            player.controllers.maps.SetMapsEnabled(true, "Default");
            player.controllers.maps.SetMapsEnabled(false, "UI");
        }
        */
    }

    private void ProcessInput()
    {
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            transform.Translate(moveVector.x, 0, moveVector.y, Space.World);
        }

        if (rotateVector.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(rotateVector, Vector3.up);
        }
    }

    private float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void ScaleJumpIndicator()
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (IsGrounded() && animJumped == true){
            animJumped = false;
            myAni.SetBool("land", true);
            Instantiate(landingPart, transform.position, landingPart.gameObject.transform.rotation);
        }
        jumped = false;
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "RV" || collision.gameObject.tag == "eVehicle")
        {
            //Debug.Log("Can jump");
            transform.parent = collision.transform.root;
            jumped = false;
            //rb.isKinematic = true;
        }
        if (collision.gameObject.tag == "road")
        {
            takeDamage(Constants.PLAYER_ROAD_DAMAGE);
            transform.position = GameObject.Find("player1Spawn").transform.position;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "eVehicle")
        {
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

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up, -Vector3.up, distToGround + 0.1f);
    }

    // ------------------------- reviving and damage --------------------------------
    public void startRevive(PlayerController_Rewired p)
    {
        reviving = true;
        reviveCountdown = reviveTime;
        p.GetComponentInChildren<HealthBar_Player>().startRevive(reviveTime);

        moveVector.x = 0f;  //zero movement so you don't keep walking while revive
        moveVector.y = 0f;
        myAni.SetFloat("speed", moveVector.magnitude);
    }

    public void stopRevive(PlayerController_Rewired p)
    {
        reviving = false;
        p.GetComponentInChildren<HealthBar_Player>().stopRevive();

    }

    public void revive(PlayerController_Rewired p)
    {
        p.currentHealth = basehealth;
        p.backToOrigAnim();
        p.setState(playerStates.up); ;
        reviving = false;
        p.GetComponentInChildren<HealthBar_Player>().stopRevive();
    }

    public void takeDamage(float _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
			goDown();
        }
    }

    public void RoadRash()
    {
        takeDamage(Constants.PLAYER_ROAD_DAMAGE);
        transform.position = GameObject.Find("player1Spawn").transform.position;
    }

	private void goDown() {
		//Color deathColor = myOrigColor * 0.5f;        //Replace with proper death feedback
		//myMat.color = deathColor;
		pPlacement.SheathWeapon();
		pPlacement.dropItem();
		myAni.SetBool("downed", true);


		state = playerStates.down;
		if (interacting) {
			objectInUse.Leave();
		}
		g.playerDowned();
	}

    // --------------------- Getters / Setters ----------------------
    public void SetId(int id)
    {
        playerId = id;
        initialized = false;
    }

    public int GetId()
    {
        return playerId;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public playerStates getState()
    {
        return state;
    }

    public void setState(playerStates s)
    {
        state = s;
    }

    public void setInteractingFlag()
    {
        interacting = true;
    }

    public void unsetInteractingFlag()
    {
        interacting = false;
    }

    //Temporary anim function, needs features for full hand follows
    public void interactAnim(bool animStat)
    {
        myAni.SetBool("aimWeapon", animStat);
        myAni.SetFloat("speed", 0f);
    }

    public float getMaxHealth()
    {
        return basehealth;
    }

    public float getHealth()
    {
        return currentHealth;
    }

    public void addInteractable(GameObject i)
    {
        if (!interactables.Contains(i))
        {
            Debug.Log("added weapon");
            interactables.Add(i);
        }
    }

    public void removeInteractable(GameObject i)
    {
        Debug.Log("removed weapon");
        interactables.Remove(i);
    }

	public Interactable getFirstInteractable() {
		return interactables[0].GetComponent<Interactable>();
	}

	public void setObjectInUse(Interactable obj) {
		objectInUse = obj;
	}

    public void clearInteractable()
    {
        interactables.Clear();
        //Debug.Log("AHHHHHHHHHHHHHHH" + interactables.Count);
    }

    public void addDownedPlayer(GameObject p)
    {
        downedPlayers.Add(p);
    }

    public void removeDownedPlayer(GameObject p)
    {
        downedPlayers.Remove(p);
    }

    public void backToOrigAnim()
    {
        myAni.SetBool("downed", false);
    }
}