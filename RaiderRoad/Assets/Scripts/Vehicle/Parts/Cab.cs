using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cab : DestructiblePart {

	// -------------------- public variables -----------------------

	// references
	public GameObject cargoNode, front_attachmentNode;

	// gameplay values
	public float healthModifier;
	public float ramDamageModifier;
	public float speedModifier;
	public float threatModifier;
}
