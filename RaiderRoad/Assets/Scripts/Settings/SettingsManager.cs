using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    #region Declarations
    public static SettingsManager Instance;

    #region Global Game Options
    private int resolution;
    private bool fullscreen;
    private int qualitySetting;

    // file save
    private SettingsRecording recording;
    private string filename = "player.prefs";
    #endregion
    #endregion

    private void Awake()
    {
        // There can only be one
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject); // Don't destroy this object
            Instance = this;
            LoadPlayerPreferences();
        }
        else
        {
            Destroy(this);
        }
    }

    private void LoadPlayerPreferences()
    {
        //Debug.Log("Player Preferences load start");
        string filepath = GetFilePath();
        Debug.Log(filepath);

        try
        {
            if (!File.Exists(filepath))
            {
                throw new Exception("Preferences File Not Found");
            }
            BinaryFormatter binRead = new BinaryFormatter();
            FileStream file = File.Open(filepath, FileMode.Open);
            recording = (SettingsRecording)binRead.Deserialize(file);
            file.Close();
        }
        catch
        {
            recording = new SettingsRecording();
            recording.Initialize(RemoveExtraReso(Screen.resolutions).Length - 1);
            recording.qualitySetting = 6;
            recording.fullscreen = true;
        }


        Resolution[] supportedResolutions = RemoveExtraReso(Screen.resolutions);
        Screen.SetResolution(supportedResolutions[recording.resolution].width, supportedResolutions[recording.resolution].height, true);
        resolution = recording.resolution;
        recording.resolution = Screen.resolutions.Length - 1;
        qualitySetting = recording.qualitySetting;
        QualitySettings.SetQualityLevel(qualitySetting);
        fullscreen = recording.fullscreen;
        Screen.fullScreen = fullscreen;
        Debug.Log("fullscreen is " + fullscreen);

        //Debug.Log("Player Preferences load end");
    }

    public void SavePlayerPreferences()
    {
        //Debug.Log("Player Preferences save start");
        recording.resolution = resolution;
        recording.qualitySetting = qualitySetting;
        recording.fullscreen = fullscreen;

        string filepath = GetFilePath();
        FileStream save;

        if (!File.Exists(filepath))
        {
            save = new FileStream(filepath, FileMode.Create);
        }
        else
        {
            save = new FileStream(filepath, FileMode.Truncate);
        }

        BinaryFormatter binForm = new BinaryFormatter();
        try
        {
            binForm.Serialize(save, recording);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            save.Close();
        }

        //Debug.Log("Player Preferences save end");
    }

    private string GetFilePath()
    {
        // Create file path to stat saving location if it does not exist
        string directoryPath = Application.persistentDataPath.ToString() + Constants.settingsFilePath;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // create file name, then file path
        return Path.Combine(directoryPath, filename);
    }

    #region Getters and Setters
    public int GetResolution()
    {
        return resolution;
    }

    public void SetResolution(int _res)
    {
        resolution = _res;
    }

    public int GetQualityLevel()
    {
        return qualitySetting;
    }

    public void SetQualityLevel(int _quality)
    {
        qualitySetting = _quality;

    }

    public bool GetFullscreen()
    {
        return fullscreen;
    }

    public void SetFullscreen(bool _fullsc)
    {
        fullscreen = _fullsc;
    }
    #endregion

    //fuction for removing extra resolutions different refresh rates
    public Resolution[] RemoveExtraReso(Resolution[] myResos)
    {
        List<Resolution> newResos = new List<Resolution>();
        List<string> resoList = new List<string>();

        //for list of resolutions
        for (int i = 0; i < myResos.Length; i++)
        {
            //check if width and height combination already exist in resolution list
            if (!resoList.Contains(myResos[i].width + " x " + myResos[i].height))
            {
                newResos.Add(myResos[i]);
                resoList.Add(myResos[i].width + " x " + myResos[i].height);
            }
        }

        //Debug.Log("cull resolution options");

        return newResos.ToArray();
    }

}
