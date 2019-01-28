using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cargo : DestructiblePart {

	public int armorStacks;
	public int speedStacks;
	public float threatModifier;

	public GameObject payloadNode;

	protected abstract override float GetMaxHealth();
}
