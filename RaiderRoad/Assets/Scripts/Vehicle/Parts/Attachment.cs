using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class Attachment : MonoBehaviour {

	public float healthModifier;
	public float ramDamageModifier;
	public float speedModifier;
	public float threatModifier;
=======
public abstract class Attachment : DestructiblePart {
	protected abstract override float GetMaxHealth();
>>>>>>> Dev
}
