using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructiblePart : MonoBehaviour {
	#region variable declarations
	// ---------------------- public variables -----------------------
	// references
	public Constants.vehicleClass vehicleClass;
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
	#endregion

	private void Start() {
		maxHealth = GetMaxHealth() + (armorStacks * Constants.ARMOR_PARTHEALTH_MODIFIER_PER_STACK);
		currentHealth = maxHealth;

		vAI = GetComponentInParent<VehicleAI>();

		for (int i = 0; i<objWithMat.Count; i++) {
            if(objWithMat[i] == null)
            {
                Debug.LogWarning("Null in objWithMat! Look into it! Path: " + Util.FullObjectPath(gameObject));
                continue;
            }
            Renderer renderer = objWithMat[i].GetComponentInChildren<Renderer>();
            if(renderer == null)
            {
                Debug.LogWarning("Object has no Renderer! Path: " + Util.FullObjectPath(objWithMat[i]));
                continue;
            }
			myMat.Add(renderer.material);
		}
		if (myMat.Count != 0) {
			Debug.Log("got here");
			myOrigColor = myMat[0].color;
			destroyedColor = myOrigColor * 0.5f;
		}
	}

	// ---------- Modifiers ----------
	public float TakeDamage(float damageDone) {
		currentHealth -= damageDone;
		Debug.Log("TAKING DAMAGE");

		if (currentHealth <= 0) {
			isIntact = false;
            if(drop != null) {
				for (int i = 0; i < Constants.WallDropsByVehicleClass[vehicleClass]; i++) {
					Instantiate(drop, transform.position + dropOffset, Quaternion.identity, transform);
				}
            }
            else {
                Debug.LogError("This DestructiblePart (of type "+this.GetType().FullName+") doesn't have a drop assigned to it. FIX THAT!");
            }

			// change texture to show destroyed
			for (int i = 0; i<myMat.Count; i++) {
				myMat[i].color = destroyedColor;
			}

			vAI.DestroyPart();
		}

		return currentHealth;
	}

	// ---------- Getters and Setters ----------
	public float GetHealth() {
		return currentHealth;
	}
}
