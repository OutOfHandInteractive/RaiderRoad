using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rvHealth : MonoBehaviour {

	//-------------------- Public Variables --------------------
	// references
	public ParticleSystem collision;

	// gameplay values
	public float maxHealth;

	// ------------------- Private Variables -------------------
	public float currentHealth;

	// ------------------- Unity Functions ---------------------
	private void Start() {
		currentHealth = maxHealth;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals("Obstacle")) {
			Debug.Log("You Hit an Obstacle");
			takeDamage(1);
			Instantiate(collision, other.gameObject.transform.position, Quaternion.identity, gameObject.transform);
			Destroy(other.gameObject);
		}
	}

	// -------------------- Getters and Setters --------------------
	public void takeDamage(float damage) {
		currentHealth -= damage;
	}

	public float getHealth() {
		return currentHealth;
	}
}
