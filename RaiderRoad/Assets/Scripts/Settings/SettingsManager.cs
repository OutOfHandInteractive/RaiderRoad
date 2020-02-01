using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
	#region Declarations
	public static SettingsManager Instance;

	public bool DemoMode = false;
	[SerializeField] private bool ExpoMode = false;

	#region Global Game Options
	private float musicVolume;
	private float fxVolume;
	private int resolution;
	private int qualitySetting;
	private int keyboardUser;

	// file save
	private SettingsRecording recording;
	private string filename = "playerID.prefs";
	private int keyboardUserIndex;
	#endregion

	#region Custom Game Options
	// default values
	private bool defaultUseTimer = true;
	private int defaultRoundTimeMinutes = 7;
	private int defaultKeepHealth = 6;
	private bool defaultMatchPlay = false;
	private int defaultBestOf = 3;

	// editable values
	private bool useTimer = true;
	private int matchLengthMinutes = 7;
	private int keepHealth = 6;
	private bool matchPlay = false;
	private int bestOf = 3;
	#endregion

	#region Mins, Maxes, Values
	// Mins and Maxes
	private int minMatchLength = 5;
	private int maxMatchLength = 15;
	private int minKeepHealth = 1;
	private int maxKeepHealth = 20;
	private int minBestOf = 3;
	private int maxBestOf = 9;
	private float minVolume = 0f;
	private float maxVolume = 2f;
	string[] keyboardUserOptionsArray = { "Off", PlayerIDs.player1, PlayerIDs.player2 };
	#endregion
	#endregion

	private void Awake() {
		// There can only be one
		if (Instance == null) {
			DontDestroyOnLoad(gameObject); // Don't destroy this object
			Instance = this;
			LoadPlayerPreferences();
		}
		else {
			Destroy(this);
		}
	}

	private void LoadPlayerPreferences() {
		string filepath = GetFilePath();

		try {
			if (!File.Exists(filepath)) {
				throw new Exception("Preferences File Not Found");
			}
			BinaryFormatter binRead = new BinaryFormatter();
			FileStream file = File.Open(filepath, FileMode.Open);		
			recording = (SettingsRecording)binRead.Deserialize(file);
			file.Close();
		}
		catch {
			recording = new SettingsRecording();
			recording.Initialize(Screen.resolutions.Length - 1);
		}

		musicVolume = recording.musicVolume;
		fxVolume = recording.fxVolume;
		Screen.SetResolution(Screen.resolutions[recording.resolution].width, Screen.resolutions[recording.resolution].height, true);
		recording.resolution = Screen.resolutions.Length - 1;
		qualitySetting = recording.qualitySetting;
		QualitySettings.SetQualityLevel(qualitySetting);
		keyboardUserIndex = recording.keyboardUser;
	}

	public void SavePlayerPreferences() {
		recording.musicVolume = musicVolume;
		recording.fxVolume = fxVolume;

#if !UNITY_XBOXONE
		recording.resolution = resolution;
		recording.qualitySetting = qualitySetting;
		recording.keyboardUser = keyboardUserIndex;
#endif
		string filepath = GetFilePath();
		FileStream save;

		if (!File.Exists(filepath)) {
			save = new FileStream(filepath, FileMode.Create);
		}
		else {
			save = new FileStream(filepath, FileMode.Truncate);
		}

		BinaryFormatter binForm = new BinaryFormatter();
		try {
			binForm.Serialize(save, recording);
		}
		catch (SerializationException e) {
			Debug.Log("Failed to serialize. Reason: " + e.Message);
			throw;
		}
		finally {
			save.Close();
		}

		ControllerManager.Instance.AssignControllers();
	}

	private string GetFilePath() {
		// Create file path to stat saving location if it does not exist
		string directoryPath = Application.persistentDataPath.ToString() + Constants.settingsFilePath;
		if (!Directory.Exists(directoryPath)) {
			Directory.CreateDirectory(directoryPath);
		}

		// create file name, then file path
		return Path.Combine(directoryPath, filename);
	}

	#region Getters and Setters
	public bool GetUseTimer() {
		return useTimer;
	}

	public bool GetDefaultUseTimer() {
		return defaultUseTimer;
	}

	public void SetUseTimer(bool _tf) {
		useTimer = _tf;
	}

	public int GetRoundTimeMinutes() {
		return matchLengthMinutes;
	}

	public int GetDefaultRoundTimeMinutes() {
		return defaultRoundTimeMinutes;
	}

	public void SetRoundTimeMinutes(int min) {
		matchLengthMinutes = min;
	}

	public int GetKeepHealth() {
		return keepHealth;
	}

	public int GetDefaultKeepHealth() {
		return defaultKeepHealth;
	}

	public void SetKeepHealth(int health) {
		keepHealth = health;
	}

	public bool GetMatchPlay() {
		return matchPlay;
	}

	public bool GetDefaultMatchPlay() {
		return defaultMatchPlay;
	}

	public void SetMatchPlay(bool _matchPlay) {
		matchPlay = _matchPlay;
	}

	public int GetBestOf() {
		return bestOf;
	}

	public int GetDefaultBestOf() {
		return defaultBestOf;
	}

	public void SetBestOf(int _bestOf) {
		bestOf = _bestOf;
	}

	public float GetMusicVolume() {
		return musicVolume;
	}

	public float GetDefaultMusicVolume() {
		return 1.00f;
	}

	public void SetMusicVolume(float _musicVolume) {
		musicVolume = _musicVolume;
	}

	public float GetFXVolume() {
		return fxVolume;
	}

	public float GetDefaultFXVolume() {
		return 1.00f;
	}

	public void SetFXVolume(float _fxVolume) {
		fxVolume = _fxVolume;
	}

	public int GetResolution() {
		return resolution;
	}

	public void SetResolution(int _res) {
		resolution = _res;
	}

	public int GetQualityLevel() {
		return qualitySetting;
	}

	public void SetQualityLevel(int _quality) {
		qualitySetting = _quality;
	}

	public string GetKeyboardUserFromIndexPretty(int _index) {
		if (_index == 1) {
			return "Player 1";
		}
		else if (_index == 2) {
			return "Player 2";
		}
		else {
			return keyboardUserOptionsArray[_index];
		}
	}

	public string GetKeyboardUserFromIndex(int _index) {
		return keyboardUserOptionsArray[_index];
	}

	public int GetKeyboardUserIndex() {
		return keyboardUserIndex;
	}

	public int GetKeyboardPlayer() {
		return keyboardUserIndex - 1;
	}

	public void SetKeyboardUser(string _userId) {
		if (_userId == "Player 1") {
			keyboardUserIndex = Array.IndexOf(keyboardUserOptionsArray, PlayerIDs.player1);
		}
		else if (_userId == "Player 2") {
			keyboardUserIndex = Array.IndexOf(keyboardUserOptionsArray, PlayerIDs.player2);
		}
		else {
			keyboardUserIndex = Array.IndexOf(keyboardUserOptionsArray, _userId);
		}
	}
	#endregion

	#region Global Setting Getters and Setters
	public bool getIsExpoMode() {
		return ExpoMode;
	}

	public bool getIsDemoMode() {
		return DemoMode;
	}
	#endregion

	#region Get Mins and Maxes
	public int GetMinMatchLength() {
		return minMatchLength;
	}

	public int GetMaxMatchLength() {
		return maxMatchLength;
	}

	public int GetMinKeepHealth() {
		return minKeepHealth;
	}

	public int GetMaxKeepHealth() {
		return maxKeepHealth;
	}

	public int GetMinBestOf() {
		return minBestOf;
	}

	public int GetMaxBestOf() {
		return maxBestOf;
	}

	public float GetMinVolume() {
		return minVolume;
	}

	public float GetMaxVolume() {
		return maxVolume;
	}
	#endregion
}
