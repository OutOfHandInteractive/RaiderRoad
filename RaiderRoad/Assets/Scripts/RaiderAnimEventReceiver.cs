using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiderAnimEventReceiver : MonoBehaviour
{
	private StatefulEnemyAI ai;

    void Start() {
		ai = GetComponentInParent<StatefulEnemyAI>();
    }

	/// <summary>
	/// Trigger function to initiate raider death behavior on death animation completion
	/// </summary>
    public void PlayDeath() {
		ai.PlayDeath();
	}

	/// <summary>
	/// Trigger function to deal damage to players at "contact point" of attack animation
	/// </summary>
	public void DealDamageOnAttack() {
		ai.attack();
	}
}
