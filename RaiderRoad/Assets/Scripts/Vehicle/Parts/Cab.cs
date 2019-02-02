using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cab : DestructiblePart {

	// -------------------- public variables -----------------------

	// references
	public GameObject cargoNode, front_attachmentNode;

<<<<<<< HEAD
	// gameplay values
	public float healthModifier;
	public float ramDamageModifier;
	public float speedModifier;
	public float threatModifier;
=======
	protected abstract override float GetMaxHealth();
>>>>>>> Dev
}
