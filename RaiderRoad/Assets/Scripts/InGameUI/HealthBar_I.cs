using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthBar_I : MonoBehaviour
{
	// ------------------ public variables -------------------
	// references
	public UnityEngine.UI.Image health_bar;
	public UnityEngine.UI.Image backdrop;
	public Transform t;

	// gameplay values
	public float heightAbove;

	// ------------------ private variables -------------------
	// references
	[SerializeField] protected Sprite backdropSprite, healthSprite;

	// gameplayValues
	protected float maxHealth;


	// ------------------- abstract methods -------------------
	protected abstract float findMaxHealth();
	protected abstract float findCurrentHealth();

	// Start is called before the first frame update
	protected void start()
    {
		// Set intial health as max health
		maxHealth = findMaxHealth();

		// assign proper sprites
		health_bar.sprite = healthSprite;
		backdrop.sprite = backdropSprite;
	}

    // Update is called once per frame
    protected void updatePosition()
    {
		transform.position = t.position + new Vector3(0, heightAbove, 0); ; //Move canvas with unit
		transform.rotation = Camera.main.transform.rotation; //Rotate canvas to face camera or else the units turning will mess with it
	}
}
