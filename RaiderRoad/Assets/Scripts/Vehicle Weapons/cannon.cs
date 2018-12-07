using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class cannon : Interactable {

	// ------------------------- public variables ---------------------------
	// references
	public cannonball munitions;
	public GameObject reticule;
	public GameObject barrel;
	public GameObject weapon;
	public GameObject smokeBurst;
	public Animator myAni;

	// gameplay values
	public float reticuleMoveSpeed;
	public float firingCooldown;
	public float coneAngle;
	private float maxRange;

	// ----------------------------------------------------------------------

	// -------------------------- Private variables -------------------------
	private bool paused = false;
	private Player player;
	private Vector2 moveVector;
	private Vector3 rotateVector;
	private GameObject proj;
	private Vector3 forwardDir;
	private Vector3 dist;
	private bool interacting = false;

	// updating variables
	private float firingCooldownTimer;
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

		//forwardDir = transform.forward;
		maxRange = getMaxRange();
	}

	// Update is called once per frame
	void Update () {
		//weapon.transform.LookAt(reticule.transform);
		if (isOnCooldown()) {
			cooldownTimer -= Time.deltaTime;
		}
		if (isOnFiringCooldown()) {
			firingCooldownTimer -= Time.deltaTime;
		}

		dist = reticule.transform.position - barrel.transform.position;
		dist = new Vector3(dist.x, 0, dist.z);

		GetInput();
		ProcessInput();
	}

	private void GetInput() {
		if (!paused && inUse) {
			moveVector.x = player.GetAxis("Move Horizontal") * Time.deltaTime * reticuleMoveSpeed;
			moveVector.y = player.GetAxis("Move Vertical") * Time.deltaTime * reticuleMoveSpeed;

			if (player.GetButtonDown("Exit Interactable") && interacting) {
				Leave();
                playerUsing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                interacting = false;
			}

			if (player.GetButtonDown("Shoot Weapon") && !isOnFiringCooldown()) {
				// Fire the cannon
				StartCoroutine(fireCannon());				

				firingCooldownTimer = firingCooldown;
			}

			if (reticule.activeSelf == true) {
				interacting = true;
                playerUsing.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

                weapon.transform.LookAt(reticule.transform);
            }
		}
	}

	private void ProcessInput() {
		// If the player has given input, move the reticule accordingly
		if (moveVector.x != 0.0f || moveVector.y != 0.0f) {
			reticule.transform.Translate(moveVector.x, 0, moveVector.y, Space.World);
			newAngle = Mathf.Atan((reticule.transform.localPosition.x) / (reticule.transform.localPosition.z));
		}
		//Debug.Log("x: " + reticule.transform.localPosition.x + " z: " + reticule.transform.localPosition.z + " angle: " + newAngle);

		// Clamp x (opposite leg) transform between -tan(angle)*z and tan(angle)*z
		// Clamp z (adj. leg) between 0 and maxRange - tan(pi/2 - (pi - (pi/2 + newAngle)))*reticuleX
		reticule.transform.localPosition = new Vector3(
			Mathf.Clamp(reticule.transform.localPosition.x, reticule.transform.localPosition.z * Mathf.Tan(-coneAngle * Mathf.Deg2Rad), reticule.transform.localPosition.z * Mathf.Tan(coneAngle * Mathf.Deg2Rad)),
			0, 
			Mathf.Clamp(reticule.transform.localPosition.z, 1, maxRange - Mathf.Tan((Mathf.PI/2) - (Mathf.PI - ((Mathf.PI/2) + newAngle)))* reticule.transform.localPosition.x));
	}

	private float getMaxRange() {
		return munitions.getMaxRange();
	}

	// ------------------- Interaction Methods ---------------------

	public override void Interact(PlayerController_Rewired pController) {
		user = pController;
		player = user.GetPlayer();
		userPlayerId = user.playerId;
        playerUsing = user.gameObject;
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

	IEnumerator fireCannon() {
		myAni.SetTrigger("Fire");

		// wait time so shot happens at right point in animation
		yield return new WaitForSecondsRealtime(7f / 24f);

		proj = Instantiate(munitions.gameObject, barrel.transform.position, Quaternion.identity);
		proj.GetComponent<cannonball>().launch(reticule.transform.position, barrel.transform.position, weapon.transform.forward);
		GameObject tempFx = Instantiate(smokeBurst, barrel.transform.position, Quaternion.identity);
		tempFx.gameObject.transform.LookAt(reticule.transform);

		yield return new WaitUntil(delegate { return !tempFx.GetComponentInChildren<ParticleSystem>().IsAlive(); });

		Destroy(tempFx);
	}
}
