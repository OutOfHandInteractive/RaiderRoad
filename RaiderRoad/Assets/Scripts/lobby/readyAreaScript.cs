using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class readyAreaScript : MonoBehaviour
{
    public lobbyManager myLobbyManager;
    public float countdownTime;
    public Text myCountdownText;

    private IEnumerator myCour;
    private float myTimer;
    private bool timerDone = false;
    private bool timerRunning = false;
    public int inReadyArea = 0;
    // Start is called before the first frame update
    void Start()
    {
        myTimer = countdownTime;
        myCour = CountDownToPlay();
    }

    // Update is called once per frame
    void Update()
    {
        CheckReadyArea();
    }

    void OnTriggerEnter(Collider other)
    {
        if(Util.isPlayer(other.gameObject))
        {
            inReadyArea++;
            CheckReadyArea();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(Util.isPlayer(other.gameObject))
        {
            inReadyArea--;
            CheckReadyArea();
        }
    }

    void CheckReadyArea()
    {
        if (inReadyArea >= myLobbyManager.joinedPlayers && inReadyArea > 0)
        {
            if (!timerRunning)
            {
                myTimer = countdownTime;
                timerRunning = true;
                StartCoroutine(myCour);
            }
        } else{
            timerRunning = false;
            StopCoroutine(myCour);
            myCountdownText.text = "";
        }
    }

    IEnumerator CountDownToPlay()
    {
        timerRunning = true;
        while (!timerDone)
        {
            myTimer -= Time.deltaTime;
            myCountdownText.text = Mathf.Ceil(myTimer).ToString();

            if (myTimer <= 0f)
            {
                myLobbyManager.PlayersReady();
            }

            yield return null;
        }
        timerRunning = false;
    }
}
