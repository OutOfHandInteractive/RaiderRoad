using System.Collections;
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

	// ----------------- Nonpublic Variables --------------------
	private RectTransform RVMarker;
    private Image dottedLine;
    private float startYpos;
    private float finishYPos;
    private float myTimer;

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

        playerList = GameObject.FindGameObjectsWithTag("Player");
        startYpos = MyUICanvas.transform.Find("StartMarker").GetComponent<RectTransform>().anchoredPosition.y;
        finishYPos = MyUICanvas.transform.Find("EndMarker").GetComponent<RectTransform>().anchoredPosition.y;
        RVMarker = MyUICanvas.transform.Find("RVMarker").GetComponent<RectTransform>();
        dottedLine = MyUICanvas.transform.Find("DottedLine").GetComponent<Image>();
    }

    // Update is called once per frame
    /// <summary>
    /// Check whether the fail or win condtion has been met and 
    /// act accordingly
    /// </summary>
    void Update () {
        if (!gameOver) {
            EngineLoss();

            if (myTimer > 0f){
                myTimer -= Time.deltaTime;
                UpdateRVMarker();
            }
			else if (myTimer <= 0f) {
                WinGame();
            }
        }
	}
    #endregion

    #region Game End Functions
    /// <summary>
    /// Check for fail state when an  engine gets destroyed
    /// </summary>
    public void EngineLoss() {
        GameObject[] engines = GameObject.FindGameObjectsWithTag("Engine");
        if(engines.Length <= 0) {
            LossGame();
        }
    }

    /// <summary>
    /// Trigger for losing game
    /// </summary>
    public void LossGame() {
        gameOver = true;
        PauseParent.GetComponent<pauseController>().endState("Vacation Canceled");
        //Temporary "get rid of victory image code
        PauseParent.GetComponent<pauseController>().myVictoryImage.SetActive(false);
    }

    /// <summary>
    /// Trigger for winning game
    /// </summary>
    public void WinGame() {
        gameOver = true;
        PauseParent.GetComponent<pauseController>().endState("Arrived at Your Vacation");
    }

    /// <summary>
    /// restarts the current game
    /// </summary>
    public void restartMenu() {
        gameOver = false;
        myTimer = FinishTime;
    }
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

	public void SetPlayers(List<Transform> playersList) {
		playersInScene = playersList;
	}
	#endregion
}
