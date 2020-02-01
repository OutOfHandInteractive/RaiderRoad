#if !UNITY_XBOXONE
using System;
using UnityEngine;

public class QualitySelector : menuOptionSelector {
	private int selectedQualityIndex;
	private int selectionIndex, maxIndex, minIndex;

	new void Awake() {
		base.Awake();
	}

	new void Update() {
		base.Update();
	}

	public override void Initialize() {
		selectedQualityIndex = QualitySettings.GetQualityLevel();

		UpdateText();
	}

	public override void IncrementOption() {
		if (CanSwitchOptions()) {
			QualitySettings.IncreaseLevel(true);
			selectedQualityIndex = QualitySettings.GetQualityLevel();

			StartCoroutine(arrowScaler(rightArrow));
			ResetSwitchCooldown();

			UpdateText();
		}
	}

	public override void DecrementOption() {
		if (CanSwitchOptions()) {
			QualitySettings.DecreaseLevel(true);
			selectedQualityIndex = QualitySettings.GetQualityLevel();

			StartCoroutine(arrowScaler(leftArrow));
			ResetSwitchCooldown();

			UpdateText();
		}
	}

	public void UpdateText() {
		displayText.text = QualitySettings.GetQualityLevel().ToString();
	}

	public override T GetValue<T>() {
		return (T)Convert.ChangeType(selectedQualityIndex, typeof(T));
	}
}
#endif