using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cab : DestructiblePart {

	// -------------------- public variables -----------------------

	// references
	public GameObject cargoNode, front_attachmentNode;

	// gameplay values
	public int armorStacks;
	public int speedStacks;
	public float threatModifier;

	protected abstract override float GetMaxHealth();
}
