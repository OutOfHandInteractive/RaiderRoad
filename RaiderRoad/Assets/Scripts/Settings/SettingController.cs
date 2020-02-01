using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SettingController : MonoBehaviour
{
    [SerializeField] private Dropdown resoDropDn;
    //[SerializeField] private Text resoTitle;
    private Resolution[] supportedResolutions;
    private int selectedResolution;
    int startResoIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //RESOLUTION
        supportedResolutions = Screen.resolutions;
        supportedResolutions = RemoveExtraReso(supportedResolutions);
        //List<Resolution> supportedResolutionsList = supportedResolutions.ToList();
        //supportedResolutionsList.RemoveAll(r => r.refreshRate < 60);

        List<string> resoOptions = DisplayReso();

        resoDropDn.ClearOptions();

        resoDropDn.AddOptions(resoOptions);
        resoDropDn.value = startResoIndex;
        resoDropDn.RefreshShownValue();

        //QUALITY

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    Resolution[] RemoveExtraReso(Resolution[] myResos)
    {
        //find highest refresh rate
        int myHz = 0;
        for (int i = 0; i < myResos.Length; i++)
        {
            if(myResos[i].refreshRate > myHz)
            {
                myHz = myResos[i].refreshRate;
            }
        }

        List<Resolution> newResos = new List<Resolution>();
        
        //only add resolutions for highest refresh rate
        for (int i = 0; i < myResos.Length; i++)
        {
            if (myResos[i].refreshRate >= myHz)
            {
                newResos.Add(myResos[i]);
            }
        }

        return newResos.ToArray();
    }

    List<string> DisplayReso()
    {
        List<string> resoList = new List<string>();

        for(int i = 0; i < supportedResolutions.Length; i++)
        {
            string option = supportedResolutions[i].width + " x " + supportedResolutions[i].height;
            resoList.Add(option);

            if (supportedResolutions[i].width == Screen.currentResolution.width
                && supportedResolutions[i].height == Screen.currentResolution.height)
            {
                startResoIndex = i;
            }
        }

        return resoList;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = supportedResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
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
