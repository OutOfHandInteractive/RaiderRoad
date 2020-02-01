using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameSettingsSelector : MonoBehaviour {
	#region Declarations
	public enum settingOptions { musicVolume, fxVolume, resolution };

	// ---------------- public variables ---------------------
	// references
	public Player p;

	// --------------- nonpublic variables -------------------
	// references
	private GameManager g;

	private EventSystem myEventSystem;
	//[SerializeField] private Image upArrow, downArrow;

	// Setting UI displays
	[Header("UI Elements")]
	[SerializeField] private List<Button> menuOptions;
	[SerializeField] private GameObject upArrow, downArrow;
	[SerializeField] private List<Slider> settingSliders;
	[SerializeField] private menuOptionSelector resolutionOptionSelector;
	[SerializeField] private menuOptionSelector qualityOptionSelector;
	[SerializeField] private Text keyboardUserSelection;
	private Slider selectedSlider = null;
	private menuOptionSelector selectedSelector = null;
	[SerializeField] private GameObject settingConfirmation;

	// gameplay values
	[Header("'Gameplay' Values")]
	[SerializeField] private float moveSensitivity = 0.8f;
	[SerializeField] private float optionSwitchCooldown;
	[SerializeField] private float arrowMaxScale;
	[SerializeField] private float slowSliderIntervalTime, fastSliderIntervalTime;
	[SerializeField] private int movesBeforeFastSlider;
	private int slowSliderMoveCount = 0;
	private float sliderMoveCounter = float.MaxValue;
	private bool displayConfirmPrompt = false;

	private float vertMove = 0f;
	private float horzMove = 0f;
	private int currentIndex = 0;
	private int optionCount;
	private float optionSwitchCooldownTimer;
	private bool isSettingSelected = false;
	#endregion

	#region System Functions
	// Use this for initialization
	void Start() {
		//p = ReInput.players.GetPlayer(PlayerIDs.player1);

		optionCount = menuOptions.Count;

		myEventSystem = EventSystem.current;

		InitializeStats();

		// Deactivate and remove graphics settings buttons if on xbox
#if UNITY_XBOXONE
		for (int i=2; i<menuOptions.Count; i++) {
			menuOptions[i].gameObject.transform.parent.gameObject.SetActive(false);
		}
		menuOptions.RemoveRange(2, 3);
#else
		if (!p.controllers.hasMouse) {
			myEventSystem.SetSelectedGameObject(menuOptions[currentIndex].gameObject);
			Cursor.lockState = CursorLockMode.Locked;
		}
		else {
			myEventSystem.SetSelectedGameObject(null);
			currentIndex = -1;
			Cursor.lockState = CursorLockMode.None;
		}
#endif

		//pInput = GetComponent<PlayerDirectionalInput>();
	}

	// Update is called once per frame
	void Update() {
		//vertMove = pInput.GetVertNonRadialInput(p);
		//horzMove = pInput.GetHorizNonRadialInput(p);
		switchOnCooldown();

		// Control A button and vertical switching if player 1 is using mouse
		if (!p.controllers.hasMouse) {
			if (vertMove < moveSensitivity * (-1) && !switchOnCooldown() && !isSettingSelected) {
				if (currentIndex > 0) {
					currentIndex--;
				}

				StartCoroutine(arrowScaler(upArrow));

				AdjustSelectedForInteractability(true);

				optionSwitchCooldownTimer = optionSwitchCooldown;
			}
			else if (vertMove > moveSensitivity && !switchOnCooldown() && !isSettingSelected) {
				if (currentIndex < optionCount - 1) {
					currentIndex++;
				}

				StartCoroutine(arrowScaler(downArrow));

				AdjustSelectedForInteractability(false);

				optionSwitchCooldownTimer = optionSwitchCooldown;
			}
			else if (p.GetButtonDown(RewiredConsts.Action.ButtonBottom)) {
				menuOptions[currentIndex].onClick.Invoke();
			}

			//Debug.Log(currentIndex);
			if (currentIndex != -1) {
				myEventSystem.SetSelectedGameObject(menuOptions[currentIndex].gameObject);
			}
		}
		else {
			myEventSystem.SetSelectedGameObject(null);
		}

		// Non mouse-specific functionality
		if (p.GetButtonDown(RewiredConsts.Action.ButtonRight) && !settingConfirmation.activeSelf) {
			if (isSettingSelected) {
				if (displayConfirmPrompt) {
					settingConfirmation.SetActive(true);
				}
				else {
					ReinableNonSelectedButtons(currentIndex);
					SaveSettings();
					isSettingSelected = false;
					if (selectedSelector) {
						selectedSelector.DeactivateArrows();
					}
					selectedSlider = null;
					selectedSelector = null;
				}
			}
			else {
				ReturnToMainMenu();
			}
		}
		else if (settingConfirmation.activeSelf) {
			if (p.GetButtonDown(RewiredConsts.Action.ButtonRight)) {
				InitializeStats();
				settingConfirmation.SetActive(false);

				ReinableNonSelectedButtons(currentIndex);
				SaveSettings();
				isSettingSelected = false;
				if (selectedSelector) {
					selectedSelector.DeactivateArrows();
				}
				selectedSlider = null;
				selectedSelector = null;
			}
			else if (p.GetButtonDown(RewiredConsts.Action.ButtonLeft)) {
				settingConfirmation.SetActive(false);

				ReinableNonSelectedButtons(currentIndex);
				SaveSettings();
				isSettingSelected = false;
				if (selectedSelector) {
					selectedSelector.DeactivateArrows();
				}
				selectedSlider = null;
				selectedSelector = null;
			}
		}
		else if ((horzMove > moveSensitivity || horzMove < moveSensitivity * (-1)) && !switchOnCooldown() && isSettingSelected) {
			if (selectedSelector) {
				AdjustSelectorWithStick();
			}
		}

		if (horzMove < moveSensitivity && horzMove > moveSensitivity * (-1)) {
			sliderMoveCounter = float.MaxValue;
			slowSliderMoveCount = 0;
		}
	}
	#endregion

	private void AdjustSelectorWithStick() {
		if (horzMove > 0) {
			selectedSelector.IncrementOption();
		}
		else if (horzMove < 0) {
			selectedSelector.DecrementOption();
		}
	}

	private void AdjustSelectedForInteractability(bool goingUp) {
		if (menuOptions[currentIndex].GetComponent<Button>().interactable == false) {
			if (goingUp) {
				if (currentIndex > 0) {
					currentIndex--;
				}
				else {
					currentIndex++;
				}
			}
			else {
				if (currentIndex < optionCount - 1) {
					currentIndex++;
				}
				else {
					currentIndex--;
				}
			}
		}
	}

	private bool switchOnCooldown() {
		if (optionSwitchCooldownTimer > 0) {
			optionSwitchCooldownTimer -= Time.deltaTime;
			return true;
		}
		else
			return false;
	}

	//public void ReturnToMainMenu() {
	//	sc.setNextSceneName(Constants.sceneNames[Constants.gameScenes.mainMenu]);
	//	sc.LoadNextScene();
	//}

#region Display Functions
	public void DisableNonSelectedButtons(int selectedButton) {
		for (int i = 0; i < menuOptions.Count; i++) {
			if (i != selectedButton) {
				menuOptions[i].GetComponent<Button>().interactable = false;
			}
		}

		isSettingSelected = true;
	}

	public void ReinableNonSelectedButtons(int selectedButton) {
		for (int i = 0; i < menuOptions.Count; i++) {
			if (i != selectedButton) {
				menuOptions[i].GetComponent<Button>().interactable = true;
			}
		}
	}

#region Arrows
	private IEnumerator arrowScaler(GameObject arrow) {
		arrow.transform.localScale = new Vector3(arrowMaxScale, arrowMaxScale, arrowMaxScale);

		float timer = 0;
		float scaleDelta = arrowMaxScale - 1;
		float newScale;

		while (timer < optionSwitchCooldown / 2) {
			newScale = arrowMaxScale - (scaleDelta * (timer / (optionSwitchCooldown / 2)));
			arrow.transform.localScale = new Vector3(newScale, newScale, newScale);

			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
		}
		arrow.transform.localScale = new Vector3(1f, 1f, 1f);
	}
#endregion

#endregion

#region Setting OnClick Functions
	public void SelectResolutionSelector(int index) {
		selectedSelector = resolutionOptionSelector;
		selectedSelector.ActivateArrows();
		currentIndex = 2;
		DisableNonSelectedButtons(index);
		displayConfirmPrompt = selectedSelector.getPromptConfirmation();
	}

	public void SelectQualitySelector(int index) {
		selectedSelector = qualityOptionSelector;
		selectedSelector.ActivateArrows();
		currentIndex = 3;
		DisableNonSelectedButtons(index);
		displayConfirmPrompt = selectedSelector.getPromptConfirmation();
	}
	#endregion

#region Settings Manager Interaction
	private void InitializeStats() {
		resolutionOptionSelector.Initialize();
		qualityOptionSelector.Initialize();
	}

	private void SaveSettings() {
		int res = resolutionOptionSelector.GetValue<int>();
		SettingsManager.Instance.SetResolution(res);
		SettingsManager.Instance.SetQualityLevel(qualityOptionSelector.GetValue<int>());
		Screen.SetResolution(Screen.resolutions[res].width, Screen.resolutions[res].height, true);

		SettingsManager.Instance.SavePlayerPreferences();
	}
#endregion
}
