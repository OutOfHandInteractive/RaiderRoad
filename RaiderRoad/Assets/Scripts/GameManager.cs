﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	#region Declarations
	// Static Reference
	public static GameManager GameManagerInstance = null;

	// ------------------- Public Variables ---------------------
	public GameObject MyUICanvas;
    public GameObject PauseParent; //the parent of pause and end UI
	public bool pauseInput = false;
	public bool gameOver = false;
	public float FinishTime;
	public float markerBarOffset;
    public Text myCountdownText;
    public float endCountdownTime;
    public CameraShake MainVCamShake; //public variable for mainVCam so any object can get it even if disabled
    public bool InfiniteMode;
    public float speedMPH = 60f;

	// ----------------- Nonpublic Variables --------------------
	private RectTransform RVMarker;
    private Image dottedLine;
    private Text miles;
    private Text fractions;
    private float startYpos;
    private float finishYPos;
    private float myTimer;
    private float timeElapsed;
    private float endTimer;
    private bool timerDone = false;
    private bool timerRunning = false;
    private IEnumerator myCour;

    
    private List<Transform> playersInScene;
    private GameObject[] playerList;
    #endregion

    #region System Functions
    /// <summary>
    /// Singleton initialization
    /// </summary>
    private void Awake() {
		if (GameManagerInstance == null) //if not, set instance to this
			GameManagerInstance = this; //If instance already exists and it's not this:
		else if (GameManagerInstance != this)
			Destroy(gameObject); //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
	}

    /// <summary>
    /// Set initial references
    /// </summary>
    void Start () {
        //timeElapsed = 0f;
        gameOver = false;
        myTimer = FinishTime;
        //myCour = CountDownToEnd();
        InfiniteMode = sceneManagerScript.Instance.InfiniteMode;

        playerList = GameObject.FindGameObjectsWithTag("Player");
        var classic = MyUICanvas.transform.Find("Classic");
        var infinite = MyUICanvas.transform.Find("Infinite");

        classic.gameObject.SetActive(!InfiniteMode);
        infinite.gameObject.SetActive(InfiniteMode);

        startYpos = classic.Find("StartMarker").GetComponent<RectTransform>().anchoredPosition.y;
        finishYPos = classic.Find("EndMarker").GetComponent<RectTransform>().anchoredPosition.y;
        RVMarker = classic.Find("RVMarker").GetComponent<RectTransform>();
        dottedLine = classic.Find("DottedLine").GetComponent<Image>();

        miles = infinite.Find("Marker/Miles").GetComponent<Text>();
        fractions = infinite.Find("Marker/Fractions").GetComponent<Text>();
    }

    // Update is called once per frame
    /// <summary>
    /// Check whether the fail or win condtion has been met and 
    /// act accordingly
    /// </summary>
    void Update () {
        if (!gameOver) {
            EngineLoss();


            if (InfiniteMode)
            {
                timeElapsed += Time.deltaTime;
                float tmp = toMiles(timeElapsed) * 100;
                int mileCnt = (int)tmp;
                int frac = mileCnt % 100;
                mileCnt /= 100;
                miles.text = string.Format("{0:D3}", mileCnt);
                fractions.text = string.Format("{0:D2}", frac);
            }
            else
            {
                if (myTimer > 0f)
                {
                    myTimer -= Time.deltaTime;
                    UpdateRVMarker();
                }
                else if (myTimer <= 0f)
                {
                    WinGame();
                }
            }
        }
	}

    private float toMiles(float time)
    {
        return speedMPH / 60f / 60f * time;
    }
    #endregion

    #region Game End Functions
    /// <summary>
    /// Check for fail state when an  engine gets destroyed
    /// </summary>
    public void EngineLoss() {
        GameObject[] engines = GameObject.FindGameObjectsWithTag("Engine");
        if(engines.Length <= 0) {
            /*
            if (!timerRunning)
            {
                endTimer = endCountdownTime;
                timerRunning = true;
                StartCoroutine(myCour);
            }
            else{
                timerRunning = false;
                StopCoroutine(myCour);
                myCountdownText.text = "";
            }*/
            LossGame();
        }
    }

    /// <summary>
    /// Trigger for losing game
    /// </summary>
    public void LossGame() {
        if (!gameOver)
        {
            gameOver = true;
            string message = "Vacation Canceled";
            if (InfiniteMode)
            {
                message = string.Format("You made it {0:F2} miles!", toMiles(timeElapsed));
                if(timeElapsed > PlayerPrefs.GetFloat("highScore", 0f))
                {
                    PlayerPrefs.SetFloat("highScore", timeElapsed);
                    PlayerPrefs.Save();
                    message += " A new record!";
                }
            }
            PauseParent.GetComponent<pauseController>().endState(message);
            //Temporary "get rid of victory image code
            PauseParent.GetComponent<pauseController>().myVictoryImage.SetActive(false);
        }
    }

    /// <summary>
    /// Trigger for winning game
    /// </summary>
    public void WinGame() {
        if (!gameOver)
        {
            gameOver = true;
            PauseParent.GetComponent<pauseController>().endState("Arrived at Your Vacation");
        }
    }

    /// <summary>
    /// restarts the current game
    /// </summary>
    public void restartMenu() {
        gameOver = false;
        myTimer = FinishTime;
    }


    /* END TIMER RELIC
    IEnumerator CountDownToEnd()
    {
        timerRunning = true;
        while (!timerDone)
        {
            endTimer -= Time.deltaTime;

            myCountdownText.text = Mathf.Ceil(endTimer).ToString();

            if (endTimer <= 0f)
            {
                LossGame();
            }

            yield return null;
        }
        timerRunning = false;
    }
     */
    #endregion

    #region Game State Helpers
    //Function for calling players in "downed" state, called every time new player is downed
    /// <summary>
    /// Check if all players are downed, then trigger game loss
    /// </summary>
    public void PlayerDowned() {
		int downedPlayers = 0;
		foreach (GameObject i in playerList) {
			if (i.GetComponent<PlayerController_Rewired>().state == PlayerController_Rewired.playerStates.down) {
				downedPlayers++;
			}
		}

		if (downedPlayers >= playerList.Length) {
			LossGame();     //If all players are downed, game is over
		}
	}

    /// <summary>
    /// For controlling the progress meter and its related UI
    /// </summary>
	private void UpdateRVMarker() {
		Vector2 TempMarkPos = RVMarker.anchoredPosition;
		TempMarkPos.y = Mathf.Lerp(finishYPos - markerBarOffset, startYpos + markerBarOffset, myTimer / FinishTime); //offSet added to not cover cards
		RVMarker.anchoredPosition = TempMarkPos;

		dottedLine.fillAmount = myTimer / FinishTime;
	}
	#endregion

	#region Getters and Setters
	public int GetPlayerCount() {
		return playersInScene.Count;
	}

	public float GetGameTime() {
		return FinishTime - myTimer;
	}

    public List<Transform> GetPlayers()
    {
        return playersInScene;
    }

	public void SetPlayers(List<Transform> playersList) {
		playersInScene = playersList;
	}
	#endregion
}
