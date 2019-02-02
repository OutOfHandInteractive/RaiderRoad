using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuManager : MonoBehaviour {

    private sceneManagerScript sceneManage;
    public string scene1;
    public string scene2;

    // Use this for initialization
    void Start () {
        sceneManage = sceneManagerScript.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MenuLoadScene1()
    {
        sceneManage.LoadScene(scene1);
    }

    public void MenuLoadScene2()
    {
        sceneManage.LoadScene(scene2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
