using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
	// user variables
	public int userPlayerId;
    protected GameObject playerUsing;
	protected PlayerController_Rewired user;

	protected bool inUse;
	public float cooldown;
	protected float cooldownTimer;

	public abstract void Interact(PlayerController_Rewired player);
	public abstract void Leave();

	public bool isOnCooldown() {
		if (cooldownTimer > 0)
			return true;
		else
			return false;
	}
}
