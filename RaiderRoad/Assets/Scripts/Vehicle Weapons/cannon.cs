﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class cannon : Interactable {

	// ------------------------- public variables ---------------------------
	// references
	public cannonball munitions;
	public GameObject reticule;
	public GameObject barrel;

	// gameplay values
	public float reticuleMoveSpeed;
	public float firingCooldown;
	public float coneAngle;
	public float correctionSpeed;
	public float maxRange;

	// ----------------------------------------------------------------------

	// -------------------------- Private variables -------------------------
	private bool paused = false;
	private Player player;
	private Vector2 moveVector;
	private Vector3 rotateVector;
	private GameObject proj;
	public Vector3 forwardDir;

	// updating variables
	public float firingCooldownTimer;
	private float newAngle;

	// ----------------------------------------------------------------------

	[System.NonSerialized]
	private bool initialized;

	// Use this for initialization
	void Start () {
		inUse = false;
		user = null;
		userPlayerId = -1;
		cooldownTimer = cooldown;
		firingCooldownTimer = firingCooldown;

		forwardDir = transform.forward;
	}

	// Update is called once per frame
	void Update () {
		if (isOnCooldown()) {
			cooldownTimer -= Time.deltaTime;
		}
		if (isOnFiringCooldown()) {
			firingCooldownTimer -= Time.deltaTime;
		}

		GetInput();
		ProcessInput();
	}

	private void GetInput() {
		if (!paused && inUse) {
			moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * reticuleMoveSpeed;
			moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * reticuleMoveSpeed;

			if (player.GetButtonDown("Exit Interactable")) {
				Leave();
			}

			if (player.GetButtonDown("Shoot Weapon") && !isOnFiringCooldown()) {
				proj = Instantiate(munitions.gameObject, barrel.transform.position, Quaternion.identity);
				proj.GetComponent<cannonball>().launch(reticule.transform.position, barrel.transform.position);

				firingCooldownTimer = firingCooldown;
			}
		}
	}

	private void ProcessInput() {
		// If the player has given input, move the reticule accordingly
		if (moveVector.x != 0.0f || moveVector.y != 0.0f) {
			reticule.transform.Translate(moveVector.x, 0, moveVector.y, Space.World);
			newAngle = Mathf.Atan((reticule.transform.localPosition.x) / (reticule.transform.localPosition.z)) * Mathf.Rad2Deg;
		}
		//Debug.Log("x: " + reticule.transform.localPosition.x + " z: " + reticule.transform.localPosition.z + " angle: " + newAngle);

		// Clamp x (opposite leg) transform between -tan(angle)*z and tan(angle)*z, and clamp z (adj. leg) between 0 and max range
		reticule.transform.localPosition = new Vector3(Mathf.Clamp(reticule.transform.localPosition.x, reticule.transform.localPosition.z * Mathf.Tan(-coneAngle * Mathf.Deg2Rad), reticule.transform.localPosition.z * Mathf.Tan(coneAngle * Mathf.Deg2Rad)), 
			0, Mathf.Clamp(reticule.transform.localPosition.z, 0, maxRange));
	}

	private float getMaxRange() {
		return munitions.getMaxRange(barrel.transform.position.y);
	}

	// ------------------- Interaction Methods ---------------------

	public override void Interact(PlayerController_Rewired pController) {
		user = pController;
		player = user.GetPlayer();
		userPlayerId = user.playerId;
		user.setInteractingFlag();

		inUse = true;
		reticule.SetActive(true);
	}

	public override void Leave() {
		cooldownTimer = cooldown;
		user.unsetInteractingFlag();
		inUse = false;
		reticule.SetActive(false);
	}

	private bool isOnFiringCooldown() {
		if (firingCooldownTimer > 0)
			return true;
		else
			return false;
	}
}
