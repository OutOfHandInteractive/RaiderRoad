using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar_Player : HealthBar_I {

	// ---------------- public variables -------------------
	// references
	public PlayerController_Rewired player; //This lets us call the unit's health each frame
	public Sprite reviveSprite;

	// gameplay values
	public bool reviving = false;

	// ---------------- private variables ------------------
	// gameplay values
	private float reviveProgress = 0;
	private float reviveTime;

	private void Start() {
		base.start();
	}

	// Update is called once per frame
	void Update() {
		if (!reviving) {
			health_bar.fillAmount = findCurrentHealth() / maxHealth;//Update health bar
		}
		else {
			reviveProgress += Time.deltaTime;
			health_bar.fillAmount = reviveProgress / reviveTime;
		}

		base.updatePosition();
	}

	#region abstract implementations
	protected override float findMaxHealth() {
		return player.getMaxHealth();
	}

	protected override float findCurrentHealth() {
		return player.getHealth();
	}
	#endregion

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
