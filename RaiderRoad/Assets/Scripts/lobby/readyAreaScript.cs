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

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            inReadyArea++;
            CheckReadyArea();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            inReadyArea--;
            CheckReadyArea();
        }
    }

    void CheckReadyArea()
    {
        if (inReadyArea >= myLobbyManager.joinedPlayers && inReadyArea > 0)
        {
            myTimer = countdownTime;
            StartCoroutine(myCour);
        } else{
            StopCoroutine(myCour);
            myCountdownText.text = "";
        }
    }

    IEnumerator CountDownToPlay()
    {
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
    }
}
