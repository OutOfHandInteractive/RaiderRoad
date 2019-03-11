using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleFactory_I : MonoBehaviour {

	public GameObject VehicleBase;
	protected static System.Random rand;

	public List<GameObject> Cab;
	public List<GameObject> Cargo;
	public List<GameObject> Attachment;
	public List<GameObject> Wheel;
	public List<GameObject> Payload;

	protected abstract GameObject selectChassis();
	public abstract void AttachPayload(GameObject cargo);
	public abstract void AttachWheels(GameObject chassis, VehicleAI v);

	public VehicleFactory_I() {
		rand = new System.Random();
	}

	public GameObject AssembleVehicle(int modifier, Vector3 position) {
		float armorStacks = 0f;
		float ramDamageStacks = 0f;
		float speedStacks = 0f;
        int threatMod = modifier;

		GameObject vehicle, chassis, cab, cargo;
		vehicle = Instantiate<GameObject>(VehicleBase, position, Quaternion.identity);
        if(vehicle.transform.position != position)
        {
            Debug.LogError("WTF");
        }
		VehicleAI vAI = vehicle.GetComponent<VehicleAI>();

		chassis = AttachChassis(vehicle, vAI);
		cab = AttachCab(chassis, vAI, ref armorStacks, ref ramDamageStacks, ref speedStacks);
		cargo = AttachCargo(cab, vAI, ref armorStacks, ref ramDamageStacks, ref speedStacks);
		AttachAttachment(cab, vAI, ref armorStacks, ref ramDamageStacks, ref speedStacks);
		AttachPayload(cargo);
		AttachWheels(chassis, vAI);

		Chassis chassisScript = chassis.GetComponent<Chassis>();

		vAI.setMaxHealth(chassisScript.GetBaseHealth() * (1 + armorStacks * Constants.ARMOR_TOTALHEALTH_MODIFIER_PER_STACK));
		vAI.setRamDamage(ramDamageStacks);
		vAI.setSpeed(chassisScript.baseSpeed * (1 + speedStacks * Constants.SPEED_MOVEMENT_MODIFIER_PER_STACK));
		vAI.setMovementChance(speedStacks * Constants.SPEED_LOCATIONCHANGE_MODIFIER_PER_STACK);

		vehicle.GetComponent<eventObject>().setDifficulty(chassis.GetComponent<Chassis>().baseThreat);

        return vehicle;
	}


	// set up chassis
	public GameObject AttachChassis(GameObject vehicle, VehicleAI v) {
		GameObject chassis = Instantiate(selectChassis(), vehicle.transform);
        chassis.transform.localPosition = Vector3.zero;
		//chassis.transform.SetParent(vehicle.transform);
		//chassis.transform.position = Vector3.zero;

		Chassis chassisScript = chassis.GetComponent<Chassis>();

		//speedStacks += chassisScript.baseSpeed;

		return chassis;
	}

	// attach cab to chassis
	public GameObject AttachCab(GameObject chassis, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject cab = Instantiate(selectCab(), chassis.GetComponent<Chassis>().cabNode.transform);
        cab.transform.localPosition = Vector3.zero;
		//cab.transform.SetParent(chassis.GetComponent<Chassis>().cabNode.transform);
		//cab.transform.position = cab.transform.parent.transform.position;
		Cab cabScript = cab.GetComponent<Cab>();

		armorStacks += cabScript.armorStacks;
		speedStacks += cabScript.speedStacks;

		return cab;
	}

	// attach cargo to cab
	public GameObject AttachCargo(GameObject cab, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject cargo = Instantiate(selectCargo(), cab.GetComponent<Cab>().cargoNode.transform);
        cargo.transform.localPosition = Vector3.zero;
        //cargo.transform.SetParent(cab.GetComponent<Cab>().cargoNode.transform);
        //cargo.transform.position = cargo.transform.parent.transform.position;
        Cargo cargoScript = cargo.GetComponent<Cargo>();

		armorStacks += cargoScript.armorStacks;
		speedStacks += cargoScript.speedStacks;

		return cargo;
	}

	// attach attachment to cab
	public void AttachAttachment(GameObject cab, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject front_attachment = Instantiate(selectAttachment(), cab.GetComponent<Cab>().front_attachmentNode.transform);
        front_attachment.transform.localPosition = Vector3.zero;
        //front_attachment.transform.SetParent(cab.GetComponent<Cab>().front_attachmentNode.transform);
        //front_attachment.transform.position = front_attachment.transform.parent.transform.position;
        Attachment attachmentScript = front_attachment.GetComponent<Attachment>();

		ramDamageStacks += attachmentScript.ramDamageStacks;
		armorStacks += attachmentScript.armorStacks;
		ramDamageStacks += attachmentScript.ramDamageStacks;
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

	protected GameObject selectPayload() {
		int selectedIndex = rand.Next(0, Payload.Count);
		return Payload[selectedIndex];
	}
	#endregion
}
