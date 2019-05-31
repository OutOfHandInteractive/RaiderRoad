using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyManager : MonoBehaviour
{
    public int joinedPlayers = 0;
    public Transform[] spawnPoints;
	[SerializeField] private ParticleSystem[] playerSpawnFX;

    private sceneManagerScript sceneManage;

    private List<int> chara1players = new List<int>();
    private List<int> chara2players = new List<int>();
    private List<int> chara3players = new List<int>();
    private List<int> chara4players = new List<int>();

    private bool sceneChanging = false;

    // Start is called before the first frame update
    void Start() { 
        sceneManage = sceneManagerScript.Instance;
	}

    public void SpawnPlayer(GameObject CharaSelector, int myId, int myChara)
    {
        joinedPlayers++;

        Transform newPlayer = null;
        //add id to correct player character
        if(myChara == 1) {
            chara1players.Add(myId);
            newPlayer = Instantiate(sceneManage.character1, spawnPoints[myId].position, sceneManage.character1.rotation * Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			Instantiate(playerSpawnFX[myId], spawnPoints[myId].position, Quaternion.identity);
			//pulling players and materials in scene manager to spawn for now
        }
		else if (myChara == 2) {
            chara2players.Add(myId);
            newPlayer = Instantiate(sceneManage.character2, spawnPoints[myId].position, sceneManage.character2.rotation * Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			Instantiate(playerSpawnFX[myId], spawnPoints[myId].position, Quaternion.identity);
		}
		else if (myChara == 3) {
            chara3players.Add(myId);
            newPlayer = Instantiate(sceneManage.character3, spawnPoints[myId].position, sceneManage.character3.rotation * Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			Instantiate(playerSpawnFX[myId], spawnPoints[myId].position, Quaternion.identity);
		}
		else if (myChara == 4) {
            chara4players.Add(myId);
            newPlayer = Instantiate(sceneManage.character4, spawnPoints[myId].position, sceneManage.character4.rotation * Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			Instantiate(playerSpawnFX[myId], spawnPoints[myId].position, Quaternion.identity);
		}

		//set ids
		newPlayer.gameObject.GetComponent<PlayerController_Rewired>().SetId(myId);
        newPlayer.Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>().SetId(myId);

        sceneManage.AssignPlayMat(newPlayer.gameObject, myId, myChara-1);
        CharaSelector.SetActive(false);
    }

    public void PlayersReady()
    {
        // Need the if guard to ensure that we don't call this twice by accident
        if (!sceneChanging)
        {
            sceneManage.OverrideNextScene("EnemyAI"); //Temporary Fix, Scene flow needs overhaul
            sceneManage.PlaySelDone(chara1players.ToArray(), chara2players.ToArray(), chara3players.ToArray(), chara4players.ToArray());
            sceneChanging = true;
        }
    }
}
