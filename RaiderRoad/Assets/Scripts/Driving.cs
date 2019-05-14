using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Driving : Interactable
{
    //-------------------- Public Variables --------------------
    // gameplay values
    public int playerId = 0;
    public Transform rv;
    public float moveSpeed = 10f;
    public enum direction { left, right };
    public float accel;
    public float maxSpeed;
    public float change;
    public float leftClamp, rightClamp;
    public float enemyCountL, enemyCountR;
    public direction prevDir;
    public direction newDir;

    //Skid
    public Skid leftSkidNode;
    public Skid rightSkidNode;
    public Skid leftSkidNodeFront;
    public Skid rightSkidNodeFront;
    public float skidDuration = 1f;
    public float skidIntensity = 1f;

    // Side Damage
    public float sideDamageTick = .5f;
    public float sideDamagePerTick = 1f;
    public ParticleSystem rightSpark;
    public ParticleSystem leftSpark;

    // Rotation
    public float rotationMod = 15f;

    //--------------------
    // Private Variables
    //--------------------
    private bool paused = false;
    private Player player;
    [SerializeField]
    private Vector2 moveVector;
    private bool takeSideDamage = false;
    private float count = 0f;

    [System.NonSerialized]
    private bool initialized;

    private void Start()
    {
        inUse = false;
        user = null;
        userPlayerId = -1;
        cooldownTimer = cooldown;
    }

    void Update()
    {
        if (isOnCooldown())
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (enemyCountL > 0)
        {
            leftClamp = -17;
        }
        else
        {
            leftClamp = -20;
        }

        if (enemyCountR > 0)
        {
            rightClamp = 17;
        }
        else
        {
            rightClamp = 20;
        }

        // Side damage
        if (takeSideDamage)
        {
            if (count <= 0f)
            {
                rv.GetComponent<rvHealth>().damagePOI(sideDamagePerTick);
                count = sideDamageTick;
            }
            else
            {
                count -= Time.deltaTime;
            }
        }

        // Rotation
        if (moveVector.x != 0)
        {
            if (newDir == direction.left)
            {
                rv.transform.localEulerAngles = new Vector3(rv.transform.localEulerAngles.x, accel * -rotationMod, rv.transform.localEulerAngles.z);
            }
            else
            {
                rv.transform.localEulerAngles = new Vector3(rv.transform.localEulerAngles.x, accel * rotationMod, rv.transform.localEulerAngles.z);
            }
        }
            
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        if (!paused && inUse)
        {
            if ((moveVector.x != 0.0f || moveVector.y != 0.0f) && (accel < maxSpeed))
            {
                accel += Time.deltaTime * change;
            }

            playerUsing.transform.position = transform.position;
            playerUsing.transform.rotation = transform.rotation;

            if(player.GetAxis("Move Horizontal") != 0)
            {
                leftSkidNode.TireSkid(skidDuration, skidIntensity);
                rightSkidNode.TireSkid(skidDuration, skidIntensity);
                leftSkidNodeFront.TireSkid(skidDuration, skidIntensity);
                rightSkidNodeFront.TireSkid(skidDuration, skidIntensity);
                moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * moveSpeed * accel;
            }
            else if(moveVector.x >= 0)
            {
                moveVector.x -= 0.001f; //MAGIC NUMBERS
            }
            else if(moveVector.x <= 0)
            {
                moveVector.x += 0.001f; //Fuck all these hacks and whoever wrote them (me)
            }
            
            moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * moveSpeed * accel;

            if (player.GetButtonDown("Exit Interactable") || Input.GetKeyDown("k"))
            {
                Leave();
                accel = 0;
                playerUsing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            if (player.GetButtonDown("Jump"))
            {
                CarAudio audio = rv.GetComponentInChildren<CarAudio>();
                if (audio != null)
                {
                    audio.Honk();
                }
            }

			if (player.GetButtonDown("Attack")) {
				EnvironmentAudio.Instance.PlaySound_RVHonk();
			}

            if ((player.GetAxis("Move Horizontal") == 0  && player.GetAxis("Move Vertical") == 0) && (accel >= 0))
            {
                accel -= Time.deltaTime * (change * 5);
            }

            if(moveVector.x > 0)
            {
                newDir = direction.right;
            }
            else
            {
                newDir = direction.left;
            }

            if (newDir != prevDir) //slow down when changing direction
            {
                moveVector.x *= 0;
                accel = accel * 0.20f;
            }

            prevDir = newDir;
        }
    }

    IEnumerator SmoothSteppin(float a, float dur)
    {
        float timer = 10f;
        float timeToTake = 10f;
        while (timer >= 0)
        {
            accel = Mathf.SmoothStep(a, 0, timer / timeToTake);
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private void ProcessInput()
    {
        rv.Translate(moveVector.x, 0, moveVector.y, Space.World);
        Vector3 clampedPosition = rv.transform.position;

        // Side wall dmg
        if (clampedPosition.x <= leftClamp)
        {
            takeSideDamage = true;
            leftSpark.Play();
        }
        else if (clampedPosition.x >= rightClamp)
        {
            takeSideDamage = true;
            rightSpark.Play();
        }
        else
        {
            takeSideDamage = false;
            rightSpark.Stop();
            leftSpark.Stop();
        }

        clampedPosition.x = Mathf.Clamp(rv.transform.position.x, leftClamp, rightClamp);
        clampedPosition.z = Mathf.Clamp(rv.transform.position.z, -10f, 10f);
        rv.transform.position = clampedPosition;
    }

    public float VerticalAxis()
    {
        return player.GetAxis("Move Vertical");
    }

    public float HorizontalAxis()
    {
        return player.GetAxis("Move Horizontal");
    }

    public void SetId(int id)
    {
        playerId = id;
        initialized = false;
    }

    // ------------------- Interaction Methods ---------------------

    public override void Interact(PlayerController_Rewired pController)
    {
        user = pController;
        player = user.GetPlayer();
        userPlayerId = user.playerId;
        playerUsing = user.gameObject;
        user.setInteractingFlag();
        user.interactAnim(true); //start animation
		user.setObjectInUse(this);

		playerUsing.GetComponent<Rigidbody>().isKinematic = true;
		playerUsing.GetComponent<PlayerController_Rewired>().StopWalkingAudio();

		inUse = true;
    }

    public override bool Occupied()
    {
        return inUse;
    }

    public override void Leave()
    {
        cooldownTimer = cooldown;
        user.unsetInteractingFlag();
        inUse = false;
        user.interactAnim(false); //stop animation
        moveVector = Vector3.zero;
		user.setObjectInUse(null);

		playerUsing.GetComponent<Rigidbody>().isKinematic = false;
	}
}
