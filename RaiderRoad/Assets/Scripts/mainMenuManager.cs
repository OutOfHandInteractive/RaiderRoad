using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mainMenuManager : MonoBehaviour {

    private sceneManagerScript sceneManage;
    public string scene1;
    public string scene2;
    public string settingScene;
    public string creditsScene;

    private GameObject startButt;
    private GameObject howToButt;
    private GameObject settingButt;
    private GameObject quitButt;
    private GameObject classicButt;
    private GameObject infiniteButt;
    private GameObject backButt;

    private EventSystem eventSystem;

    private bool modeSelect = false;

    // Use this for initialization
    void Start () {
        sceneManage = sceneManagerScript.Instance;
        startButt = transform.Find("PlayButt").gameObject;
        howToButt = transform.Find("HowToPlayButt").gameObject;
        settingButt = transform.Find("SettingButt").gameObject;
        quitButt = transform.Find("QuitButt").gameObject;
        classicButt = transform.Find("PlayClassicButt").gameObject;
        infiniteButt = transform.Find("PlayInfiniteButt").gameObject;
        backButt = transform.Find("BackButt").gameObject;
        eventSystem = FindObjectOfType<EventSystem>();

        updateVisibility();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void updateVisibility()
    {
        classicButt.SetActive(modeSelect);
        infiniteButt.SetActive(modeSelect);
        backButt.SetActive(modeSelect);

        startButt.SetActive(!modeSelect);
        howToButt.SetActive(!modeSelect);
        settingButt.SetActive(!modeSelect);
        quitButt.SetActive(!modeSelect);
    }

    public void ClickStart()
    {
        modeSelect = true;
        updateVisibility();
        eventSystem.SetSelectedGameObject(classicButt);
    }

    public void PlayClassic()
    {
        sceneManage.InfiniteMode = false;
        MenuLoadScene1();
    }

    public void PlayInfinite()
    {
        sceneManage.InfiniteMode = true;
        MenuLoadScene1();
    }

    public void Back()
    {
        modeSelect = false;
        updateVisibility();
        eventSystem.SetSelectedGameObject(startButt);
    }

    public void LoadSettings()
    {
        sceneManage.LoadScene(settingScene);
    }

    public void LoadCredits()
    {
        sceneManage.LoadScene(creditsScene);
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
