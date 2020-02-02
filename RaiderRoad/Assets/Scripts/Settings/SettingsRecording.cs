using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class SettingsRecording
{
	public int resolution;
	public int qualitySetting;
    public bool fullscreen;

    public void Initialize(int index) {
		resolution = index;
		qualitySetting = QualitySettings.GetQualityLevel();
	}
}
