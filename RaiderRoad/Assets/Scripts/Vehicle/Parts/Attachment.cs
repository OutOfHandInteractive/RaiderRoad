using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attachment : DestructiblePart {

	public float ramDamageStacks;
	public int armorStacks;
	public int speedStacks;
	public float threatModifier;

	protected abstract override float GetMaxHealth();
}
