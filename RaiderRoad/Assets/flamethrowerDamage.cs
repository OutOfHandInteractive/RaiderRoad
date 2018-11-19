using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flamethrowerDamage : MonoBehaviour {
	
	private float tickDamage;
	private float tickTime; // time in seconds between damage ticks
	private float tickTimeCountdown;

	private List<EnemyAI> enemyTargets = new List<EnemyAI>();
	private List<VehicleAI> vehicleTargets = new List<VehicleAI>();

	// Update is called once per frame
	void Update () {
		tickTimeCountdown -= Time.deltaTime;
		Debug.Log(tickTimeCountdown);
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
		Debug.Log("adding target");
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

	public void setTickDamage(float _tickDamage) {
		tickDamage = _tickDamage;
	}

	public void setTickTime(float _tick) {
		tickTime = _tick;
		tickTimeCountdown = tickTime;
	}
}
