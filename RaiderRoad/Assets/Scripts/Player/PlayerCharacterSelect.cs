using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerCharacterSelect : MonoBehaviour {
    //This file is changed

    //--------------------
    // Public Variables
    //--------------------
    public int playerId = 0;
    public float cooldown = 0.5f;
    public GameObject characterSelect;

    //--------------------
    // Private Variables
    //--------------------
    private Player player;

    private bool moveLeft = false;
    private bool moveRight = false;

    private float count = 0.0f;

    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);

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
        if (count <= 0.0)
        {
            if (player.GetAxis("Move Horizontal") < 0)
            {
                moveLeft = true;
                count = cooldown;
            }
            else if (player.GetAxis("Move Horizontal") > 0)
            {
                moveRight = true;
                count = cooldown;
            }
        }
        else
        {
            count -= Time.deltaTime;
        }
       
        if (player.GetButtonDown("Start"))
        {
            characterSelect.GetComponent<CharacterSelect>().SpawnPlayers();
        }
    }

    private void ProcessInput()
    {
        if (moveLeft)
        {
            gameObject.transform.SetParent(characterSelect.GetComponent<CharacterSelect>().MoveLeft(gameObject.transform.parent));
            moveLeft = false;
        }
        else if (moveRight)
        {
            gameObject.transform.SetParent(characterSelect.GetComponent<CharacterSelect>().MoveRight(gameObject.transform.parent));
            moveRight = false;
        }
    }

    public int GetId()
    {
        return playerId;
    }
}
