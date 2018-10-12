using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Part : MonoBehaviour {

	private float health;

	// ---------- Modifiers ----------
	public void damage(float damageDone) {
		health -= damageDone;
	}


	// ---------- Getters and Setters ----------
	public float getHealth() {
		return health;
	}
}
