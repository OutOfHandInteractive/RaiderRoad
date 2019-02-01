using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chassis : MonoBehaviour {

	public GameObject cabNode;
	public List<GameObject> wheelNodes;

	public float baseSpeed;
	public int baseThreat;

	public abstract float GetBaseHealth();

	// ---------- Getters and Setters ----------
	public int getNumWheels() {
		return wheelNodes.Count;
	}
}
