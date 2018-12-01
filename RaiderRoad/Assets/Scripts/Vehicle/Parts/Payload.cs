using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Payload : MonoBehaviour {
	public enum payloadTypes { enemy, weapon }
	public abstract void populate();
	public List<payloadTypes> payloadCode;

	protected abstract StatefulEnemyAI SelectEnemies();
    protected abstract Weapon SelectInteractable();
}