using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour {
    //--------------------
    // Public Variables
    //--------------------

    public Transform noCharaPanel;
    public Transform chara1Panel;
    public Transform chara2Panel;
    public Transform chara3Panel;
    public Transform chara4Panel;

    public Transform rv;
    private sceneManagerScript sceneManage;

    //--------------------
    // Private Variables
    //--------------------

    private void Start()
    {
        sceneManage = sceneManagerScript.Instance;
    }

    public Transform MoveLeft(Transform pos)
    {
        if (pos == chara4Panel)
        {
            return chara3Panel;
        }
        else if (pos == chara3Panel)
        {
            return noCharaPanel;
        }
        else if (pos == noCharaPanel)
        {
            return chara2Panel;
        }
        else if (pos == chara2Panel)
        {
            return chara1Panel;
        }
        else
        {
            return chara1Panel;
        }
    }

    public Transform MoveRight(Transform pos)
    {
        if (pos == chara1Panel)
        {
            return chara2Panel;
        }
        else if (pos == chara2Panel)
        {
            return noCharaPanel;
        }
        else if (pos == noCharaPanel)
        {
            return chara3Panel;
        }
        else if (pos == chara3Panel)
        {
            return chara4Panel;
        }
        else
        {
            return chara4Panel;
        }
    }

    public void PassPlayerId()
    {
        int[] c1Array = new int[chara1Panel.childCount];
        for (int i = 0; i < chara1Panel.childCount; i++)
        {
            c1Array[i] = chara1Panel.GetChild(i).GetComponent<PlayerCharacterSelect>().GetId();
        }

        int[] c2Array = new int[chara2Panel.childCount];
        for (int i = 0; i < chara2Panel.childCount; i++)
        {
            c2Array[i] = chara2Panel.GetChild(i).GetComponent<PlayerCharacterSelect>().GetId();
        }

        int[] c3Array = new int[chara3Panel.childCount];
        for (int i = 0; i < chara3Panel.childCount; i++)
        {
            c3Array[i] = chara3Panel.GetChild(i).GetComponent<PlayerCharacterSelect>().GetId();
        }

        int[] c4Array = new int[chara4Panel.childCount];
        for (int i = 0; i < chara4Panel.childCount; i++)
        {
            c4Array[i] = chara4Panel.GetChild(i).GetComponent<PlayerCharacterSelect>().GetId();
        }

        sceneManage.PlaySelDone(c1Array, c2Array, c3Array, c4Array);
    }


    /*
    public void SpawnPlayers()
    {
        //rv = Instantiate(rv);
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
            Transform player = Instantiate(player1, rv.Find("player1Spawn").position, player1.rotation, rv);
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
            Transform player = Instantiate(player2, rv.Find("player2Spawn").position, player2.rotation, rv);
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
            Transform player = Instantiate(player3, rv.Find("player3Spawn").position, player3.rotation, rv);
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
            Transform player = Instantiate(player4, rv.Find("player4Spawn").position, player4.rotation, rv);
            Transform controller = player4Panel.GetChild(0);
            int id = controller.gameObject.GetComponent<PlayerCharacterSelect>().GetId();
            player.gameObject.GetComponent<PlayerController_Rewired>().SetId(id);
            player.Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(id);
        }
    }
    */
}
