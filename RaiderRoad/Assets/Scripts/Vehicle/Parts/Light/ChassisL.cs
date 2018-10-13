using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChassisL : Chassis {

	public List<GameObject> wheelNodes;

	public ChassisL() {
	}

	void Start() {
		numWheels = wheelNodes.Capacity;	
	}

	// ---------- Getters and Setters ----------
	public int getNumWheels() {
		return numWheels;
	}
}
