using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiderAnimEventReceiver : MonoBehaviour
{
	private StatefulEnemyAI ai;

    // Start is called before the first frame update
    void Start() {
		ai = GetComponentInParent<StatefulEnemyAI>();
    }

    public void PlayDeath() {
		ai.PlayDeath();
	}

	public void DealDamageOnAttack() {
		ai.attack();
	}
}
