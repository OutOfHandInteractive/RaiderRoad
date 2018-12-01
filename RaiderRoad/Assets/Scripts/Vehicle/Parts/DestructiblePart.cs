using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructiblePart : MonoBehaviour {

	public float maxHealth;
	public bool isIntact = true;
	public GameObject wallDrop;

	private float currentHealth;


	// ---------- Modifiers ----------
	public float takeDamage(float damageDone) {
		currentHealth -= damageDone;
		if (currentHealth <= 0) {
			isIntact = false;
			GameObject item = Instantiate(wallDrop, transform.position + new Vector3(0, 2f, 0), Quaternion.identity, transform);
			item.name = "Wall Drop";
		}

		return currentHealth;
	}

	// ---------- Getters and Setters ----------
	public float getHealth() {
		return currentHealth;
	}
}
