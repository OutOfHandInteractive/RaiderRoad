using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flamethrowerDamage : MonoBehaviour {
	
	private float tickDamage;
	private float tickTime; // time in seconds between damage ticks
	private float tickTimeCountdown;

	private List<EnemyAI> enemyTargets;
	private List<VehicleAI> vehicleTargets; 
	
	// Update is called once per frame
	void Update () {
		if (tickTimeCountdown <= 0) {
			tickTimeCountdown = tickTime;

			for (int i=0; i<vehicleTargets.Count; i++) {
				if (!vehicleTargets[i])
					vehicleTargets.RemoveAt(i);
				else {
					vehicleTargets[i].takeDamage(tickDamage);
				}
			}

			for (int i = 0; i < enemyTargets.Count; i++) {
				if (!enemyTargets[i])
					enemyTargets.RemoveAt(i);
				else {
					enemyTargets[i].takeDamage(tickDamage);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		GameObject directTarget = other.gameObject;
		if (directTarget.CompareTag("eVehicle")) {
			vehicleTargets.Add(directTarget.GetComponentInParent<VehicleAI>());
		}
		else if (directTarget.CompareTag("Enemy")) {
			enemyTargets.Add(directTarget.GetComponent<EnemyAI>());
		}
	}

	private void OnTriggerExit(Collider other) {
		GameObject directTarget = other.gameObject;
		if (directTarget.CompareTag("eVehicle")) {
			vehicleTargets.Remove(directTarget.GetComponentInParent<VehicleAI>());
		}
		else if (directTarget.CompareTag("Enemy")) {
			enemyTargets.Remove(directTarget.GetComponent<EnemyAI>());
		}
	}

	private void setTickDamage(float _tickDamage) {
		tickDamage = _tickDamage;
	}

	private void setTickTime(float _tick) {
		tickTime = _tick;
		tickTimeCountdown = tickTime;
	}
}
