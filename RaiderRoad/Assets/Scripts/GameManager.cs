using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager GameManagerInstance = null;

	private float timeElapsed;

	private void Awake() {
		if (GameManagerInstance == null) //if not, set instance to this
			GameManagerInstance = this; //If instance already exists and it's not this:
		else if (GameManagerInstance != this)
			Destroy(gameObject); //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		timeElapsed = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
	}


	// -------------------------- Getters and Setters -------------------------
	public float getGameTime() {
		return timeElapsed;
	}
}
