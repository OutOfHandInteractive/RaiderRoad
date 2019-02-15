using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar_Enemy : HealthBar_I {

	// ---------------- public variables -----------------
	// references
	public UnityEngine.UI.Image destroy_bar;
	public UnityEngine.UI.Image destroy_backdrop;
	public StatefulEnemyAI enemy; //This lets us call the unit's health each frame
	public Sprite destroySprite, destroybackdropSprite;

	// Use this for initialization
	void Start() {
        destroy_bar.sprite = destroySprite;
        destroy_backdrop.sprite = destroybackdropSprite;

		base.start();
    }

	// Update is called once per frame
	void Update() {
		health_bar.fillAmount = enemy.getHealth() / maxHealth;//Update health bar
        destroy_bar.fillAmount = enemy.damageMeter / 100;

		base.updatePosition();
	}

	#region abstract implementations
	protected override float findMaxHealth() {
		return enemy.getMaxHealth();
	}

	protected override float findCurrentHealth() {
		return enemy.getHealth();
	}
	#endregion
}
