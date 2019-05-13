using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentAudio : AudioManager
{
	#region Declarations
	public static EnvironmentAudio Instance = null;

	[SerializeField] private float wallBreakSoundVolume;
	#endregion
	// Start is called before the first frame update
	void Start() {
		if (Instance == null) //if not, set instance to this
			Instance = this; //If instance already exists and it's not this:
		else if (Instance != this)
			Destroy(gameObject); //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.

		audioSource = GetComponent<AudioSource>();
	}

    public void PlaySound_ConstructableDestroy(AudioClip sound) {
		OneShot(sound, wallBreakSoundVolume);
	}
}
