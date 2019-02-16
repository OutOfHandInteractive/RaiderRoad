using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager GameManagerInstance = null;

    public GameObject MyUICanvas;
    public GameObject EndGameText;
    public GameObject RestartButton;
    //public GameObject RVHealthText;
    //private int RVhealth;
    public RectTransform RVMarker;
    public Image dottedLine;
    public float startYpos;
    public float finishYPos;

    public bool gameOver = false;
    public float FinishTime;
    private float myTimer;
    //public GameObject GameTimer;
    //private float timeElapsed;
    private List<Transform> playersInScene;

    private void Awake() {
		if (GameManagerInstance == null) //if not, set instance to this
			GameManagerInstance = this; //If instance already exists and it's not this:
		else if (GameManagerInstance != this)
			Destroy(gameObject); //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.

		//Sets this to not be destroyed when reloading scene (WHY?)
		//DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
        //timeElapsed = 0f;
        gameOver = false;
        myTimer = FinishTime;

        startYpos = MyUICanvas.transform.Find("StartMarker").GetComponent<RectTransform>().anchoredPosition.y;
        finishYPos = MyUICanvas.transform.Find("EndMarker").GetComponent<RectTransform>().anchoredPosition.y;
        RVMarker = MyUICanvas.transform.Find("RVMarker").GetComponent<RectTransform>();
        dottedLine = MyUICanvas.transform.Find("DottedLine").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        EngineLoss();
        if (!gameOver)
        {
            if (myTimer > 0f){
                myTimer -= Time.deltaTime;
                UpdateRVMarker();

            } else if (myTimer <= 0f) {
                WinGame();
            }
        }
	}


	// -------------------------- Getters and Setters -------------------------
	public float getGameTime() {
		return FinishTime;
	}

    public void getPlayers(List<Transform> playersList)
    {
        playersInScene = playersList;

    }

    private void UpdateRVMarker() {
        Vector2 TempMarkPos = RVMarker.anchoredPosition;
        TempMarkPos.y = Mathf.Lerp(finishYPos, startYpos, myTimer / FinishTime);
        RVMarker.anchoredPosition = TempMarkPos;

        dottedLine.fillAmount = myTimer / FinishTime;
    }

    //Function for calling players in "downed" state, called every time new player is downed
    public void playerDowned() {
        int downedPlayers = 0;
        foreach (Transform i in playersInScene)
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAA" + i.name);
            if(i.GetComponent<PlayerController_Rewired>().state == PlayerController_Rewired.playerStates.down)
            {
                downedPlayers++;
            }
        }

        if (downedPlayers >= playersInScene.Count)
        {
            LossGame();     //If all players are downed, game is over
        }
    }

    /*  OLD
    public void updateRVHealth(float newHealth)
    {
        if (newHealth <= 0f) newHealth = 0;
        RVHealthText.GetComponent<Text>().text = "RV Health: " + Mathf.Ceil(newHealth);
    }*/

    public void EngineLoss()
    {
        GameObject[] engines = GameObject.FindGameObjectsWithTag("Engine");
        if(engines.Length <= 0)
        {
            LossGame();
        }
    }

    public void LossGame()
    {
        RestartButton.SetActive(true);
        EndGameText.SetActive(true);
        gameOver = true;

        EndGameText.GetComponent<Text>().text = "Vacation Canceled";
    }

    public void WinGame()
    {
        gameOver = true;
        RestartButton.SetActive(true);
        EndGameText.SetActive(true);

        EndGameText.GetComponent<Text>().text = "Victory";
    }

    public void restartMenu()
    {
        //GameTimer.SetActive(true);  //make sure UI is on
        //RVHealthText.SetActive(true);

        gameOver = false;
        RestartButton.SetActive(false);
        EndGameText.SetActive(false);
        myTimer = FinishTime;
        //RVHealthText.GetComponent<Text>().text = "RV Health: 6";
    }

    /* MAYBE OLD, FIND WHERE CALLED
    public void clearMenu()
    {
        gameOver = false;
        RestartButton.SetActive(false);
        EndGameText.SetActive(false);
        myTimer = FinishTime;
        GameTimer.SetActive(false);
        RVHealthText.SetActive(false);
    }*/
}
