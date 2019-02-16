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

    //--------------------
    // Private Variables
    //--------------------
    private bool paused = false;
    private Player player;
    private Vector2 moveVector;
    private Rigidbody rb;

    [System.NonSerialized]
    private bool initialized;

    private void Start()
    {
        inUse = false;
        user = null;
        userPlayerId = -1;
        cooldownTimer = cooldown;

        rb = rv.GetComponent<Rigidbody>();
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

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb.AddForce(movement * moveSpeed);

            if (player.GetButtonDown("Exit Interactable"))
            {
                Leave();
                playerUsing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

    private void ProcessInput()
    {
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            rv.Translate(moveVector.x, 0, moveVector.y, Space.World);
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
