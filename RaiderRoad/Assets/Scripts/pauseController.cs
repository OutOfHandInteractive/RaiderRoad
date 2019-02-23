﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;

public class pauseController : MonoBehaviour
{
    public Transform pauseUI;
    public EventSystem myEventSystem;

    private int playerNum = 0;
    private Player rewiredPlayer1 = null;
    private Player rewiredPlayer2 = null;
    private Player rewiredPlayer3 = null;
    private Player rewiredPlayer4 = null;

    private bool isPaused;
    private GameManager g;

    //Temporary Win Screen
    public Transform endStateUI;
    public Text endStateText;

    public GameObject endRestartButton;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        g = GameManager.GameManagerInstance;

        rewiredPlayer1 = ReInput.players.GetPlayer("Player0");
        rewiredPlayer2 = ReInput.players.GetPlayer("Player1");
        rewiredPlayer3 = ReInput.players.GetPlayer("Player2");
        rewiredPlayer4 = ReInput.players.GetPlayer("Player3");
    }

    // Update is called once per frame
    void Update()
    {
        if((rewiredPlayer1.GetButtonDown("Start") || rewiredPlayer2.GetButtonDown("Start") || rewiredPlayer3.GetButtonDown("Start") || rewiredPlayer4.GetButtonDown("Start")) && !g.GetComponent<GameManager>().gameOver) {
            pauseToggle();
        }
    }


    public void pauseToggle()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            pauseUI.gameObject.SetActive(true);
            g.GetComponent<GameManager>().pauseInput = true;
            StartCoroutine("highlightBtn");

            //myControlMapper.SetActive(false); //bad

        } else {
            pauseUI.gameObject.SetActive(false);
            g.GetComponent<GameManager>().pauseInput = false;
            //myControlMapper.SetActive(true); //bad
            Time.timeScale = 1;
        }

        isPaused = !isPaused;
    }

    //Temporary End State Screen
    public void endState(string endText)
    {
        endStateText.text = endText;
        endStateUI.gameObject.SetActive(true);
        g.GetComponent<GameManager>().pauseInput = true;
        myEventSystem.SetSelectedGameObject(endRestartButton);
        //Time.timeScale = 0;
        //myControlMapper.SetActive(false); //bad
    }

    //USE ACTUAL SCENE MANAGER LATER
    public void restartGame()
    {
        Time.timeScale = 1;
        string myScene = SceneManager.GetActiveScene().name;
        GameManager g = GameManager.GameManagerInstance;
        g.restartMenu();
        SceneManager.LoadScene(myScene, LoadSceneMode.Single);
    }

    public void quitToLobby()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("playerLobby", LoadSceneMode.Single);
    }

    IEnumerator highlightBtn()
    {
        myEventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        myEventSystem.SetSelectedGameObject(myEventSystem.firstSelectedGameObject);
    }

    IEnumerator highlightEndGame()
    {
        myEventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        myEventSystem.SetSelectedGameObject(endRestartButton);
    }
}