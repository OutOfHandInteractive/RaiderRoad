using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restartFunction : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void restartGame()
    {
        string myScene = SceneManager.GetActiveScene().name;
        GameManager g = GameManager.GameManagerInstance;
        g.restartMenu();
        SceneManager.LoadScene(myScene, LoadSceneMode.Single);
    }
}
