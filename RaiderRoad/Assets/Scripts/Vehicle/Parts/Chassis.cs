using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chassis : Part {

	public GameObject cabNode;
	public List<GameObject> wheelNodes;

	public float baseHealth;
	public float baseRamDamage;
	public float baseSpeed;
	public float baseThreat;


	// ---------- Getters and Setters ----------
	public int getNumWheels() {
		return wheelNodes.Count;
	}
}
