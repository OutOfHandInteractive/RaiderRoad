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
        //Resolution[] tempResolutions = Screen.resolutions;
        //supportedResolutions = RemoveExtraReso(tempResolutions);
        supportedResolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct();

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
        List<Resolution> newResos = new List<Resolution>();

        for (int i = myResos.Length - 1; i < 0; i--)
        {
            if()
            newResos.Add(myResos[i]);
            int skipAmt = 0;

            for (int j = 1; j < i; j++)
            {
                if (myResos[i -= 1].width == myResos[i].width && myResos[i -= 1].width == myResos[i].width && myResos[i -= 1].refreshRate <= myResos[i].refreshRate)
                {
                    skipAmt++;
                }
                else
                {
                    j = i;
                }
            }
            i -= skipAmt;
        }

        return newResos.ToArray();
    }

    /*
    Resolution[] RemoveExtraReso(Resolution[] myResos)
    {
        List<Resolution> newResos = new List<Resolution>();

        for(int i = myResos.Length - 1; i < 0; i--)
        {

            newResos.Add(myResos[i]);
            int skipAmt = 0;

            for (int j = 1; j < i; j++)
            {
                if (myResos[i -= 1].width == myResos[i].width && myResos[i -= 1].width == myResos[i].width && myResos[i -= 1].refreshRate <= myResos[i].refreshRate)
                {
                    skipAmt++;
                }
                else
                {
                    j = i;
                }
            }
            i -= skipAmt;
        }

        return newResos.ToArray();
    }*/

    List<string> DisplayReso()
    {
        List<string> resoList = new List<string>();

        for(int i = 0; i < supportedResolutions.Length; i++)
        {
            string option = supportedResolutions[i].width + " x " + supportedResolutions[i].height + supportedResolutions[i].refreshRate;
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
