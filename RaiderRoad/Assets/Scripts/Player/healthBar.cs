using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour {

	public Transform playerTransform;
	public float heightAbove;
	public UnityEngine.UI.Image health_bar;
	public UnityEngine.UI.Image backdrop;
	public PlayerController_Rewired player; //This lets us call the unit's health each frame
	public bool reviving = false;

	private float reviveProgress = 0;
	private float reviveTime;

	float maxHealth;

	public Sprite backdropSprite, healthSprite, reviveSprite;

	// Use this for initialization
	void Start() {
		//Set intial health as max health
		maxHealth = player.getMaxHealth();

		health_bar.sprite = healthSprite;
		backdrop.sprite = backdropSprite;
	}

	// Update is called once per frame
	void Update() {
		if (!reviving) {
			health_bar.fillAmount = player.getHealth() / maxHealth;//Update health bar
		}
		else {
			reviveProgress += Time.deltaTime;
			health_bar.fillAmount = reviveProgress / reviveTime;
		}
		transform.position = playerTransform.position + new Vector3(0, heightAbove, 0); ; //Move canvas with unit
		transform.rotation = Camera.main.transform.rotation; //Rotate canvas to face camera or else the units turning will mess with it
	}

	public void startRevive(float _reviveTime) {
		reviving = true;
		reviveTime = _reviveTime;
		health_bar.sprite = reviveSprite;
	}

	public void stopRevive() {
		reviving = false;
		reviveProgress = 0;
		health_bar.sprite = healthSprite;
	}
}
