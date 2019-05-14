using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
	// ---------------- nonpublic variables -------------------
	// attacking
	[SerializeField] private AudioClip attackSwingSound;
	[SerializeField] private AudioClip attackHitSound;

	// movement
	[SerializeField] private List<AudioClip> walkSounds; 
}
