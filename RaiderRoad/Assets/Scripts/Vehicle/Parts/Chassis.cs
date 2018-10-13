using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chassis : Part {

	protected int numWheels;
	public GameObject cabNode;

	public float baseHealth;
	public float baseRamDamage;
	public float baseSpeed;
}
