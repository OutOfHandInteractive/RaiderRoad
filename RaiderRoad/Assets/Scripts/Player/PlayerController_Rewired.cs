﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController_Rewired : MonoBehaviour {
    //--------------------
    // Public Variables
    //--------------------
    public int playerId = 0;

    public float moveSpeed = 10f;

    public GameObject view;

    //--------------------
    // Private Variables
    //--------------------
    private Player player;
    private Vector2 moveVector;

    private Vector3 rotateVector;


    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        view.GetComponent<PlayerPlacement_Rewired>().SetId(playerId);

        initialized = true;
    }

    void Update()
    {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor

        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * moveSpeed;
        moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * moveSpeed;

        rotateVector = Vector3.right * player.GetAxis("Rotate Horizontal") + Vector3.forward * player.GetAxis("Rotate Vertical");
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

    public void SetId(int id)
    {
        playerId = id;
        initialized = false;
    }

    public int GetId()
    {
        return playerId;
    }
}