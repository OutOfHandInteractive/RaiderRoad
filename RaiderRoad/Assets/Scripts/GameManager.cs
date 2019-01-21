using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager GameManagerInstance = null;

    public GameObject EndGameText;
    public GameObject RestartButton;
    public GameObject RVHealthText;
    private int RVhealth;

    public bool gameOver = false;
    public float FinishTime;
    private float myTimer;
    public GameObject GameTimer;
    //private float timeElapsed;
    private List<Transform> playersInScene;

    private void Awake() {
		if (GameManagerInstance == null) //if not, set instance to this
			GameManagerInstance = this; //If instance already exists and it's not this:
		else if (GameManagerInstance != this)
			Destroy(gameObject); //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
        //timeElapsed = 0f;
        gameOver = false;
        myTimer = FinishTime;
    }
	
	// Update is called once per frame
	void Update () {
        EngineLoss();
        if (!gameOver)
        {
            if (myTimer > 0f)
            {
                myTimer -= Time.deltaTime;
                GameTimer.GetComponent<Text>().text = Mathf.Ceil(myTimer).ToString();
            }
            else if (myTimer <= 0f)
            {
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
            LossGame();
        }
    }

    public void updateRVHealth(float newHealth)
    {
        if (newHealth <= 0f) newHealth = 0;
        RVHealthText.GetComponent<Text>().text = "RV Health: " + Mathf.Ceil(newHealth);
    }

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

        EndGameText.GetComponent<Text>().text = "Lose";
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
        GameTimer.SetActive(true);  //make sure UI is on
        RVHealthText.SetActive(true);

        gameOver = false;
        RestartButton.SetActive(false);
        EndGameText.SetActive(false);
        myTimer = FinishTime;
        RVHealthText.GetComponent<Text>().text = "RV Health: 6";
    }

    public void clearMenu()
    {
        gameOver = false;
        RestartButton.SetActive(false);
        EndGameText.SetActive(false);
        myTimer = FinishTime;
        GameTimer.SetActive(false);
        RVHealthText.SetActive(false);
    }
}
