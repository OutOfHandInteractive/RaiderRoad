using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attachment : DestructiblePart {
	protected abstract override float GetMaxHealth();

	[SerializeField] private ParticleSystem highDamageSmokeEffects;

	public void StartHighDamageSmokeEffects() {
		highDamageSmokeEffects.Play();
	}
}
