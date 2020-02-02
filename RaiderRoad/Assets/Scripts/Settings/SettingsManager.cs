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
            recording.Initialize(Screen.resolutions.Length - 1);
            recording.fullscreen = true;
        }

        //Debug.Log(recording.qualitySetting);
        //Debug.Log(recording.resolution);
        //Debug.Log(Screen.resolutions[recording.resolution].width);
        Resolution[] supportedResolutions = RemoveExtraReso(Screen.resolutions);
        Screen.SetResolution(supportedResolutions[recording.resolution].width, supportedResolutions[recording.resolution].height, true);
        resolution = recording.resolution;
        recording.resolution = Screen.resolutions.Length - 1;
        qualitySetting = recording.qualitySetting;
        QualitySettings.SetQualityLevel(qualitySetting);
        Screen.fullScreen = recording.fullscreen;
        fullscreen = recording.fullscreen;

    }

    public void SavePlayerPreferences()
    {
        recording.resolution = resolution;
        recording.qualitySetting = qualitySetting;
        recording.fullscreen = fullscreen;
        //Debug.Log(qualitySetting);
        //Debug.Log(resolution);
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
}
