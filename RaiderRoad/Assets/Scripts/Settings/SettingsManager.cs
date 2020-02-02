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
        }

        Screen.SetResolution(Screen.resolutions[recording.resolution].width, Screen.resolutions[recording.resolution].height, true);
        recording.resolution = Screen.resolutions.Length - 1;
        qualitySetting = recording.qualitySetting;
        QualitySettings.SetQualityLevel(qualitySetting);
    }

    public void SavePlayerPreferences()
    {
        recording.resolution = resolution;
        recording.qualitySetting = qualitySetting;
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
    #endregion
}
