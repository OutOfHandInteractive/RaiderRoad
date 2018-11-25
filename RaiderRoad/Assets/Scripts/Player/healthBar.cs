using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour {

	public Transform playerTransform;
	public float heightAbove;
	public UnityEngine.UI.Image health_bar;
	public UnityEngine.UI.Image backdrop;
	public PlayerController_Rewired player; //This lets us call the unit's health each frame

	float maxHealth;

	public Sprite backdropSprite, healthSprite;

	// Use this for initialization
	void Start() {
		//Set intial health as max health
		maxHealth = player.getMaxHealth();

		health_bar.sprite = healthSprite;
		backdrop.sprite = backdropSprite;
	}

	// Update is called once per frame
	void Update() {
		health_bar.fillAmount = player.getHealth() / maxHealth;//Update health bar
		transform.position = playerTransform.position + new Vector3(0, heightAbove, 0); ; //Move canvas with unit
		transform.rotation = Camera.main.transform.rotation; //Rotate canvas to face camera or else the units turning will mess with it
	}
}
