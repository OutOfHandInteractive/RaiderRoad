using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cab : DestructiblePart {

	// -------------------- public variables -----------------------

	// references
	public GameObject cargoNode, front_attachmentNode;

	protected abstract override float GetMaxHealth();
}
