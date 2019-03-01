using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flamethrowerDamageEnemy : MonoBehaviour {

	private float tickDamage;
	private float tickTime; // time in seconds between damage ticks
	private float tickTimeCountdown;

	private List<PlayerController_Rewired> playerTargets = new List<PlayerController_Rewired>();

	// Update is called once per frame
	void Update() {
		tickTimeCountdown -= Time.deltaTime;
		//Debug.Log(tickTimeCountdown);
		if (tickTimeCountdown <= 0) {
			tickTimeCountdown = tickTime;

			for (int i = 0; i < playerTargets.Count; i++) {
				if (playerTargets[i] == null) {
					playerTargets.RemoveAt(i);
					i--;
				}
				else {
					playerTargets[i].takeDamage(tickDamage);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log("adding target");
		GameObject directTarget = other.gameObject;
		if (directTarget.CompareTag("Player")) {
			playerTargets.Add(directTarget.GetComponentInParent<PlayerController_Rewired>());
		}
	}

	private void OnTriggerExit(Collider other) {
		GameObject directTarget = other.gameObject;
		if (directTarget.CompareTag("Player")) {
			playerTargets.Remove(directTarget.GetComponentInParent<PlayerController_Rewired>());
		}
	}

	public void setTickDamage(float _tickDamage) {
		tickDamage = _tickDamage;
	}

	public void setTickTime(float _tick) {
		tickTime = _tick;
		tickTimeCountdown = tickTime;
	}
}
