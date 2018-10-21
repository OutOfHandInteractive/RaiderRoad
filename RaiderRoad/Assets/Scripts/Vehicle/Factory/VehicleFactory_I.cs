using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleFactory_I : MonoBehaviour {

	public GameObject VehicleBase;
	protected static System.Random rand;
	private float vehicleHealth = 0f;
	private float vehicleRamDamage = 0f;
	private float vehicleSpeed = 0f;

	public List<GameObject> Cab;
	public List<GameObject> Cargo;
	public List<GameObject> Attachment;
	public List<GameObject> Wheel;
	public List<GameObject> Enemy;

	protected abstract GameObject selectChassis();
	public abstract void AttachEnemy(GameObject cab);
	public abstract void AttachWheels(GameObject chassis);

	public VehicleFactory_I() {
		rand = new System.Random();
	}

	public void AssembleVehicle() {
		GameObject vehicle, chassis, cab, cargo;
		vehicle = Instantiate(VehicleBase, new Vector3(0, 0, 0), Quaternion.identity);
		VehicleAI vAI = vehicle.GetComponent<VehicleAI>();

		chassis = AttachChassis(vehicle, vAI);
		cab = AttachCab(chassis);
		cargo = AttachCargo(cab);
		AttachAttachment(cab, vAI);
		AttachEnemy(cab);
		AttachWheels(chassis);
	}

	// set up chassis
	public GameObject AttachChassis(GameObject vehicle, VehicleAI v) {
		GameObject chassis = Instantiate(selectChassis());
		chassis.transform.SetParent(vehicle.transform);
		chassis.transform.position = Vector3.zero;

		ChassisL chassisScript = chassis.GetComponent<ChassisL>();
		v.setMaxHealth(v.getMaxHealth() + chassisScript.baseHealth);
		v.setRamDamage(v.getRamDamage() + chassisScript.baseRamDamage);
		v.setSpeed(v.getSpeed() + chassisScript.baseSpeed);

		return chassis;
	}

	// attach cab to chassis
	public GameObject AttachCab(GameObject chassis) {
		GameObject cab = Instantiate(selectCab());
		cab.transform.SetParent(chassis.GetComponent<ChassisL>().cabNode.transform);
		cab.transform.position = cab.transform.parent.transform.position;

		return cab;
	}

	// attach cargo to cab
	public GameObject AttachCargo(GameObject cab) {
		GameObject cargo = Instantiate(selectCargo());
		cargo.transform.SetParent(cab.GetComponent<CabL>().cargoNode.transform);
		cargo.transform.position = cargo.transform.parent.transform.position;

		return cargo;
	}

	// attach attachment to cab
	public void AttachAttachment(GameObject cab, VehicleAI v) {
		GameObject front_attachment = Instantiate(selectAttachment());
		front_attachment.transform.SetParent(cab.GetComponent<CabL>().front_attachmentNode.transform);
		front_attachment.transform.position = front_attachment.transform.parent.transform.position;
		AttachmentL attachmentScript = front_attachment.GetComponent<AttachmentL>();

		v.setMaxHealth(v.getMaxHealth() + attachmentScript.healthModifier);
		v.setRamDamage(v.getRamDamage() + attachmentScript.ramDamageModifier);
	}

	#region Component Selectors
	protected GameObject selectCab() {
		int selectedIndex = rand.Next(0, Cab.Count);
		return Cab[selectedIndex];
	}

	protected GameObject selectCargo() {
		int selectedIndex = rand.Next(0, Cargo.Count);
		return Cargo[selectedIndex];
	}

	protected GameObject selectWheel() {
		int selectedIndex = rand.Next(0, Wheel.Count);
		return Wheel[selectedIndex];
	}

	protected GameObject selectAttachment() {
		int selectedIndex = rand.Next(0, Attachment.Count);
		return Attachment[selectedIndex];
	}

	protected GameObject selectEnemy() {
		int selectedIndex = rand.Next(0, Enemy.Count);
		return Enemy[selectedIndex];
	}
	#endregion
}
