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

    public float accel;
    public float maxSpeed;
    public float change;
    public float leftClamp, rightClamp;
    public float enemyCountL, enemyCountR;

    //--------------------
    // Private Variables
    //--------------------
    private bool paused = false;
    private Player player;
    private Vector2 moveVector;

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
            leftClamp = -21;
        }

        if (enemyCountR > 0)
        {
            rightClamp = 17;
        }
        else
        {
            rightClamp = 21;
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

            moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * moveSpeed * accel;
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
            if ((moveVector.x == 0.0f && moveVector.y == 0.0f) && (accel >= 0))
            {
                accel -= Time.deltaTime * (change * 5);
            }
        }
    }

    IEnumerator smoothSteppin(float a, float dur)
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
