using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructiblePart : MonoBehaviour {

	// ---------------------- public variables -----------------------
	// references
	public GameObject drop;
    public Vector3 dropOffset = new Vector3(0, 0.75f, -1);
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
	private VehicleAI vAI;

	// attributes
	private float currentHealth;

	// abstract methods
	protected abstract float GetMaxHealth();

	private void Start() {
		maxHealth = GetMaxHealth() * (1 + armorStacks * Constants.ARMOR_PARTHEALTH_MODIFIER_PER_STACK);

		vAI = GetComponentInParent<VehicleAI>();

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
            if(drop != null)
            {
                GameObject item = Instantiate(drop, transform.position + dropOffset, Quaternion.identity, transform);
            }
            else
            {
                Debug.LogError("This DestructiblePart (of type "+this.GetType().FullName+") doesn't have a drop assigned to it. FIX THAT!");
            }
			//item.name = "Wall Drop";

			// change texture to show destroyed
			for (int i = 0; i<myMat.Count; i++) {
				myMat[i].color = destroyedColor;
			}

			vAI.destroyPart();
		}

		return currentHealth;
	}

	// ---------- Getters and Setters ----------
	public float getHealth() {
		return currentHealth;
	}
}
