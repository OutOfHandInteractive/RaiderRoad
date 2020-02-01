using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class resolutionSelector : menuOptionSelector
{
	private Resolution[] supportedResolutions;
	private int selectedResolution;
	private int selectionIndex, maxIndex, minIndex;

    new void Awake() {
		base.Awake();
	}

	new void Update() {
		base.Update();
	}

	public override void Initialize() {
		supportedResolutions = Screen.resolutions;

		List<Resolution> supportedResolutionsList = supportedResolutions.ToList();
		//supportedResolutionsList.RemoveAll(r => r.refreshRate < 60);

		selectionIndex = supportedResolutionsList.IndexOf(Screen.currentResolution);
		selectedResolution = selectionIndex;

		supportedResolutions = supportedResolutionsList.ToArray();

		UpdateText();
	}

	public override void IncrementOption() {
		if(selectionIndex + 1 <= (supportedResolutions.Length - 1) && CanSwitchOptions()) {
			selectionIndex++;
			selectedResolution = selectionIndex;
			UpdateText();

			StartCoroutine(arrowScaler(rightArrow));
			ResetSwitchCooldown();
		}
	}

	public override void DecrementOption() {
		if (selectionIndex - 1 >= 0 && CanSwitchOptions()) {
			selectionIndex--;
			selectedResolution = selectionIndex;
			UpdateText();

			StartCoroutine(arrowScaler(leftArrow));
			ResetSwitchCooldown();
		}
	}

	public void UpdateText() {
		displayText.text = supportedResolutions[selectionIndex].width.ToString() + "x" + supportedResolutions[selectionIndex].height.ToString() 
			+ " @ " + supportedResolutions[selectionIndex].refreshRate.ToString() + "fps";
	}

	public override T GetValue<T>() {
		return (T)Convert.ChangeType(selectedResolution, typeof(T));
	}
}
