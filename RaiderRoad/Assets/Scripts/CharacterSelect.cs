using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour {
    //This file is changed

    //--------------------
    // Public Variables
    //--------------------
    public Transform player1;
    public Transform player2;
    public Transform player3;
    public Transform player4;

    public Transform noPlayerPanel;
    public Transform player1Panel;
    public Transform player2Panel;
    public Transform player3Panel;
    public Transform player4Panel;

    public Transform rv;

    //--------------------
    // Private Variables
    //--------------------

    public Transform MoveLeft(Transform pos)
    {
        if (pos == player4Panel)
        {
            return player3Panel;
        }
        else if (pos == player3Panel)
        {
            return noPlayerPanel;
        }
        else if (pos == noPlayerPanel)
        {
            return player2Panel;
        }
        else if (pos == player2Panel)
        {
            return player1Panel;
        }
        else
        {
            return player1Panel;
        }
    }

    public Transform MoveRight(Transform pos)
    {
        if (pos == player1Panel)
        {
            return player2Panel;
        }
        else if (pos == player2Panel)
        {
            return noPlayerPanel;
        }
        else if (pos == noPlayerPanel)
        {
            return player3Panel;
        }
        else if (pos == player3Panel)
        {
            return player4Panel;
        }
        else
        {
            return player4Panel;
        }
    }

    public void SpawnPlayers()
    {
        rv = Instantiate(rv);
        spawnPlayer1();
        spawnPlayer2();
        spawnPlayer3();
        spawnPlayer4();

        gameObject.SetActive(false);
    }

    void spawnPlayer1()
    {
        if (player1Panel.childCount > 0)
        {
            Transform player = Instantiate(player1, rv.Find("Player1 Spawn").position, player1.rotation, rv);
            Transform controller = player1Panel.GetChild(0);
            int id = controller.gameObject.GetComponent<PlayerCharacterSelect>().GetId();
            player.gameObject.GetComponent<PlayerController_Rewired>().SetId(id);
            player.Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(id);
        }
    }

    void spawnPlayer2()
    {
        if (player2Panel.childCount > 0)
        {
            Transform player = Instantiate(player2, rv.Find("Player2 Spawn").position, player2.rotation, rv);
            Transform controller = player2Panel.GetChild(0);
            int id = controller.gameObject.GetComponent<PlayerCharacterSelect>().GetId();
            player.gameObject.GetComponent<PlayerController_Rewired>().SetId(id);
            player.Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(id);
        }
    }

    void spawnPlayer3()
    {
        if (player3Panel.childCount > 0)
        {
            Transform player = Instantiate(player3, rv.Find("Player3 Spawn").position, player3.rotation, rv);
            Transform controller = player3Panel.GetChild(0);
            int id = controller.gameObject.GetComponent<PlayerCharacterSelect>().GetId();
            player.gameObject.GetComponent<PlayerController_Rewired>().SetId(id);
            player.Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(id);
        }
    }

    void spawnPlayer4()
    {
        if (player4Panel.childCount > 0)
        {
            Transform player = Instantiate(player4, rv.Find("Player4 Spawn").position, player4.rotation, rv);
            Transform controller = player4Panel.GetChild(0);
            int id = controller.gameObject.GetComponent<PlayerCharacterSelect>().GetId();
            player.gameObject.GetComponent<PlayerController_Rewired>().SetId(id);
            player.Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(id);
        }
    }
}
