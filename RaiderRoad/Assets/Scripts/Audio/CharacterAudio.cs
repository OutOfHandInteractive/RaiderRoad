using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class handles sounds common to players and enemies
/// </summary>
public class CharacterAudio : AudioManager
{
	// ----------------- nonpublic variables --------------------
	// ---- Gameplay ----
	/// <summary>
	/// The offset between the swing and the hit in seconds.
	/// </summary>
	[SerializeField] private float swingHitOffset = 0.1f;
	[SerializeField] private float timeBetweenSteps = 0.5f;
	private bool isWalking;

	// ---- Sounds ----
	// attacking
	/// <summary>
	/// The sound of swinging their weapon
	/// </summary>
	[SerializeField] private AudioClip attackSwing;

    /// <summary>
    /// The sound their weapon makes when it hits
    /// </summary>
    [SerializeField] private AudioClip attackHit;

	[SerializeField] private List<AudioClip> takeHitSounds;

	// movement
	[SerializeField] private float walkVolume;
	[SerializeField] private List<AudioClip> walkSounds;
	[SerializeField] private ParticleSystem walkParticles;


	#region Sound Functions
	#region Combat
	/// <summary>
	/// Plays the swing effect as a one shot then the hit sound if the flag is true.
	/// </summary>
	/// <param name="andHit">true if the hit sound should happen, false otherwise</param>
	public void PlaySound_Attack(bool andHit) {
        OneShot(attackSwing);
        if(andHit && attackHit != null) {
            audioSource.clip = attackHit;
            audioSource.PlayDelayed(swingHitOffset);
        }
    }

	public void PlaySound_DamageByRaider() {
        RandomOneShot(takeHitSounds);
	}
	#endregion

	#region Movement
	private void PlaySound_Walk() {
        RandomOneShot(walkSounds, walkVolume);
	}

	public void StartWalking() {
		if (!isWalking) {
			StartCoroutine("PlaySound_Walk_Continuous");
		}

		isWalking = true;
	}

	public void StopWalking() {
		if (isWalking) {
			StopCoroutine("PlaySound_Walk_Continuous");
		}

		isWalking = false;
	}

	#endregion
	#endregion

	#region Helper Functions
	private IEnumerator PlaySound_Walk_Continuous() {
		while (true) {
			PlaySound_Walk();
			walkParticles.Play();

			yield return new WaitForSeconds(timeBetweenSteps/2);
			walkParticles.Play();
			yield return new WaitForSeconds(timeBetweenSteps / 2);
		}
	}
	#endregion

	#region Getters and Setters
	public bool IsWalking() {
		return isWalking;
	}
	#endregion
}
