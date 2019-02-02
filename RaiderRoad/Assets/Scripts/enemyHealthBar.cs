using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealthBar : MonoBehaviour {

	public Transform enemyTransform;
	public float heightAbove;
	public UnityEngine.UI.Image health_bar;
	public UnityEngine.UI.Image backdrop;
    public UnityEngine.UI.Image destroy_bar;
    public UnityEngine.UI.Image destroy_backdrop;
    public StatefulEnemyAI enemy; //This lets us call the unit's health each frame

	float maxHealth;

	public Sprite backdropSprite, healthSprite, destroySprite, destroybackdropSprite;

	// Use this for initialization
	void Start() {
		//Set intial health as max health
		maxHealth = enemy.getMaxHealth();

		health_bar.sprite = healthSprite;
		backdrop.sprite = backdropSprite;
        destroy_bar.sprite = destroySprite;
        destroy_backdrop.sprite = destroybackdropSprite;
    }

	// Update is called once per frame
	void Update() {
		health_bar.fillAmount = enemy.getHealth() / maxHealth;//Update health bar
        destroy_bar.fillAmount = enemy.damageMeter / 100;
        transform.position = enemyTransform.position + new Vector3(0, heightAbove, 0); ; //Move canvas with unit
		transform.rotation = Camera.main.transform.rotation; //Rotate canvas to face camera or else the units turning will mess with it
	}
}
