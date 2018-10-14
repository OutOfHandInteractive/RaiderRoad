using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour {

	public enum EventTypes { vehicle, obstacle, fork };

	public int difficultyRating;
	public float postDelay;

	public abstract void spawn();
}
