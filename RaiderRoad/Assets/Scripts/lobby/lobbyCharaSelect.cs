using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class lobbyCharaSelect : MonoBehaviour
{
    //--------------------
    // Public Variables
    //--------------------
    public int playerId = 0;
    public float cooldown = 0.25f;
    public Sprite[] polaroidSprites;
    public lobbyManager myLobbyManager;

    //--------------------
    // Private Variables
    //--------------------
    private Player player;

    private GameObject myCharaPolaroid;
    private GameObject myJoinText;

    private float count = 0.0f;
    private int mySelChara = 1; //currently selected character
    private bool joined = false; //have they joined game yet?
    private bool newInput = false;

    [System.NonSerialized]
    private bool initialized;

    void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);

        myJoinText = transform.Find("joinText").gameObject;
        myCharaPolaroid = transform.Find("CharaSelect").gameObject;
        myCharaPolaroid.SetActive(false);

        initialized = true;
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor

        GetInput();
        if(newInput) ProcessInput();
    }

    private void GetInput()
    {
        if (count <= 0.0 && joined)
        {
            if (player.GetAxis("Move Horizontal") < 0)
            {
                mySelChara--;
                if(mySelChara < 1) //wrap around to last character
                {
                    mySelChara = 4;
                }
                count = cooldown;
                newInput = true;
            }
            else if (player.GetAxis("Move Horizontal") > 0)
            {
                mySelChara++;
                if (mySelChara > 4) //wrap around to first character
                {
                    mySelChara = 1;
                }
                count = cooldown;
                newInput = true;
            }
        }
        else
        {
            count -= Time.deltaTime;
        }
       
        if (player.GetButtonDown("Jump"))
        {
            if (!joined) { //pressing A to join game
                myCharaPolaroid.SetActive(true);
                myJoinText.SetActive(false);
                joined = true;
            } else {
                myLobbyManager.GetComponent<lobbyManager>().SpawnPlayer(transform.gameObject, playerId, mySelChara);
            }
        }
    }

    private void ProcessInput()
    {
       myCharaPolaroid.GetComponent<Image>().sprite = polaroidSprites[mySelChara - 1];

        newInput = false;
    }

}
