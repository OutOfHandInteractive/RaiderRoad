using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar_Vehicle : HealthBar_I
{
	// ---------------- public variables -----------------
	// references
	public VehicleAI vehicle; //This lets us call the unit's health each frame

	private void Start() {
		base.start();
	}

	// Update is called once per frame
	void Update() {
		health_bar.fillAmount = findCurrentHealth() / maxHealth;//Update health bar

		base.updatePosition();
	}

	#region abstract implementations
	protected override float findMaxHealth() {
		return vehicle.getMaxHealth();
	}

	protected override float findCurrentHealth() {
		return vehicle.getHealth();
	}
	#endregion
}
