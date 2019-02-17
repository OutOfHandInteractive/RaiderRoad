using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    //Temporary Win Screen
    public Transform endStateUI;
    public Text endStateText;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(rewiredPlayer1.GetButtonDown("Start") || rewiredPlayer2.GetButtonDown("Start") || rewiredPlayer3.GetButtonDown("Start") || rewiredPlayer4.GetButtonDown("Start")) {
            pauseToggle();
        }
    }

    public void getPlayerNumber(int pNum) {
        //Assign players based on how many players there are
        if(pNum > 0) {
            rewiredPlayer1 = ReInput.players.GetPlayer("Player0");
            if (pNum > 1) {
                rewiredPlayer2 = ReInput.players.GetPlayer("Player1");
                if (pNum > 2) {
                    rewiredPlayer3 = ReInput.players.GetPlayer("Player2");
                    if (pNum > 3) {
                        rewiredPlayer4 = ReInput.players.GetPlayer("Player3");
                    }
                }
            }
        }
    }

    private void pauseToggle()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            StartCoroutine("highlightBtn");
            pauseUI.gameObject.SetActive(true);

        } else {
            pauseUI.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        isPaused = !isPaused;
    }

    //Temporary End State Screen
    public void endState(string endText)
    {
        endStateText.text = endText;
        endStateUI.gameObject.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine("highlightEndGame");
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
        myEventSystem.SetSelectedGameObject(endStateUI.Find("RestartButton").gameObject);
    }
}
