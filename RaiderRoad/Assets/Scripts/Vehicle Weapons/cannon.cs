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

	// ----------------------------------------------------------------------

	// ----------------------
	// Private variables
	// ----------------------
	private bool paused = false;
	private Player player;
	private Vector2 moveVector;
	private Vector3 rotateVector;
	private GameObject proj;
	public float firingCooldownTimer;

	[System.NonSerialized]
	private bool initialized;

	// Use this for initialization
	void Start () {
		inUse = false;
		user = null;
		userPlayerId = -1;
		cooldownTimer = cooldown;
		firingCooldownTimer = firingCooldown;
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

			//Twin Stick Rotation
			//rotateVector = Vector3.right * player.GetAxis("Rotate Horizontal") + Vector3.forward * player.GetAxis("Rotate Vertical");

			//Single Stick Rotation
			//rotateVector = Vector3.right * player.GetAxis("Move Horizontal") + Vector3.forward * player.GetAxis("Move Vertical");

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
		if (moveVector.x != 0.0f || moveVector.y != 0.0f) {
			reticule.transform.Translate(moveVector.x, 0, moveVector.y, Space.World);
		}

		if (rotateVector.sqrMagnitude > 0.0f) {
			reticule.transform.rotation = Quaternion.LookRotation(rotateVector, Vector3.up);
		}
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