using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleFactory_I : MonoBehaviour {

	public GameObject VehicleBase;

	public List<GameObject> Frame;
	public List<GameObject> Cab;
	public List<GameObject> Cargo;
	public List<GameObject> Attachment;
	public List<GameObject> Wheel;

	public abstract void AssembleVehicle();
}
