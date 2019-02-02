using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructiblePart : MonoBehaviour {

	// ---------------------- public variables -----------------------
	// references
	public GameObject drop;
    public Vector3 dropOffset = new Vector3(0, 2, -1);
	public List<GameObject> objWithMat;

	// gameplay values
	public float maxHealth;
	public float ramDamageStacks;
	public int armorStacks;
	public int speedStacks;
	public float threatModifier;
	public bool isIntact = true;

	// ---------------------- private variables ----------------------
	// references
	private List<Material> myMat = new List<Material>();
	private Color myOrigColor;
	private Color destroyedColor;

	// attributes
	private float currentHealth;

	// abstract methods
	protected abstract float GetMaxHealth();

	private void Start() {
		maxHealth = GetMaxHealth() * (1 + armorStacks * Constants.ARMOR_PARTHEALTH_MODIFIER_PER_STACK);

		for (int i = 0; i<objWithMat.Count; i++) {
			myMat.Add(objWithMat[i].GetComponent<Renderer>().material);
		}
		if (myMat.Count != 0) {
			Debug.Log("got here");
			myOrigColor = myMat[0].color;
			destroyedColor = myOrigColor * 0.5f;
		}
	}

	// ---------- Modifiers ----------
	public float takeDamage(float damageDone) {
		currentHealth -= damageDone;
		if (currentHealth <= 0) {
			isIntact = false;
			GameObject item = Instantiate(drop, transform.position + dropOffset, Quaternion.identity, transform);
			//item.name = "Wall Drop";

			// change texture to show destroyed
			for (int i = 0; i<myMat.Count; i++) {
				myMat[i].color = destroyedColor;
			}
		}

		return currentHealth;
	}

	// ---------- Getters and Setters ----------
	public float getHealth() {
		return currentHealth;
	}
}
