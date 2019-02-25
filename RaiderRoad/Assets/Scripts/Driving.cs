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
            moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * moveSpeed;

            if (player.GetButtonDown("Exit Interactable"))
            {
                Leave();
                playerUsing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            if (moveVector.x == 0.0f || moveVector.y == 0.0f)
            {
                accel = 0;
            }
        }
    }

    private void ProcessInput()
    {
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            rv.Translate(moveVector.x, 0, moveVector.y, Space.World);
            Vector3 clampedPosition = rv.transform.position;
            clampedPosition.x = Mathf.Clamp(rv.transform.position.x, -20f, 20f);
            rv.transform.position = clampedPosition;
        }
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
    }
}
