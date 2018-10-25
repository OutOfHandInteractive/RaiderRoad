using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManagerScript : MonoBehaviour {

    public string scene1;
    public string scene2;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("escape")) SceneManager.LoadScene("SceneMenu",LoadSceneMode.Single);
	}

    public void LoadScene1() {
        SceneManager.LoadScene(scene1, LoadSceneMode.Single);
    }

    public void LoadScene2() {
        SceneManager.LoadScene(scene2, LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
