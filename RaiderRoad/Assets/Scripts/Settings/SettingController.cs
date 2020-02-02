using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SettingController : MonoBehaviour
{
    [SerializeField] private Dropdown resoDropDn;
    [SerializeField] private Dropdown qualDropDn;
    [SerializeField] private Toggle fullScToggle;
    //[SerializeField] private Text resoTitle;
    private Resolution[] supportedResolutions;
    private int selectedResolution;
    int startResoIndex = 0;
    SettingsManager settingsManager;
    private sceneManagerScript sceneManage;

    // Start is called before the first frame update
    void Start()
    {
        settingsManager = SettingsManager.Instance;
        sceneManage = sceneManagerScript.Instance;

        //RESOLUTION
        supportedResolutions = Screen.resolutions;
        supportedResolutions = settingsManager.RemoveExtraReso(supportedResolutions);
        //List<Resolution> supportedResolutionsList = supportedResolutions.ToList();
        //supportedResolutionsList.RemoveAll(r => r.refreshRate < 60);

        List<string> resoOptions = DisplayReso();

        resoDropDn.ClearOptions();

        resoDropDn.AddOptions(resoOptions);
        //resoDropDn.value = startResoIndex;
        Debug.Log(settingsManager.GetResolution());
        resoDropDn.value = settingsManager.GetResolution();
        resoDropDn.RefreshShownValue();

        //QUALITY
        qualDropDn.value = settingsManager.GetQualityLevel();
        qualDropDn.RefreshShownValue();

        //FULLSCREEN
        bool fullScreen = settingsManager.GetFullscreen();
        Screen.fullScreen = fullScreen;
        fullScToggle.isOn = fullScreen;
        //fullScToggle.();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //add resolutions to display list and add formatting
    List<string> DisplayReso()
    {
        List<string> resoList = new List<string>();

        for(int i = 0; i < supportedResolutions.Length; i++)
        {
            string option = supportedResolutions[i].width + " x " + supportedResolutions[i].height; //+ " @ " + supportedResolutions[i].refreshRate;
            resoList.Add(option);

            if (supportedResolutions[i].width == Screen.currentResolution.width
                && supportedResolutions[i].height == Screen.currentResolution.height)
            {
                startResoIndex = i;
            }
        }
        Debug.Log("Display resolution options");
        return resoList;
    }

    public void SetResolution (int resolutionIndex)
    {
        Debug.Log("Set Resolution");
        Resolution resolution = supportedResolutions[resolutionIndex];
        settingsManager.SetResolution(resolutionIndex);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        settingsManager.SavePlayerPreferences();
    }

    public void SetQuality (int qualityIndex)
    {
        Debug.Log("Set Quality");
        QualitySettings.SetQualityLevel(qualityIndex);
        settingsManager.SetQualityLevel(qualityIndex);
        settingsManager.SavePlayerPreferences();
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Debug.Log("Set Fullscreen");
        Screen.fullScreen = isFullscreen;
        settingsManager.SetFullscreen(isFullscreen);
        settingsManager.SavePlayerPreferences();
    }

    public void LoadMainMenu()
    {
        sceneManage.LoadMainMenu();
    }

    /*void OnPointerEnter (PointerEventData eventData)
    {
        resoTitle.GetComponent<Text>().color = Color.yellow;
    }

    void OnPointerExit(PointerEventData eventData)
    {
        resoTitle.color = Color.white;
    }*/
}
