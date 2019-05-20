using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleFactory_I : MonoBehaviour {
	#region variable declarations
	// --------------- public variables ------------------
	public GameObject VehicleBase;
	public List<GameObject> Cab;
	public List<GameObject> Cargo;
	public List<GameObject> Attachment;
	public List<GameObject> Wheel;
	public List<GameObject> Payload;

	// -------------- nonpublic variables ----------------
	protected static System.Random rand;
	#endregion

	#region abstract methods
	protected abstract GameObject SelectChassis();
	public abstract void AttachPayload(GameObject cargo, float wChance, int batteries);
	public abstract void AttachWheels(GameObject chassis, VehicleAI v);
	#endregion

	public VehicleFactory_I() {
		rand = new System.Random();
	}

	public GameObject AssembleVehicle(int modifier, Vector3 position, float wChance, int batteries) {
		float armorStacks = 0f;
		float ramDamageStacks = 0f;
		float speedStacks = 0f;
        int threatMod = modifier;


		// Assemble vehicle from selected parts
		GameObject vehicle, chassis, cab, cargo, attachment;
		vehicle = Instantiate<GameObject>(VehicleBase, position, Quaternion.identity);
        if(vehicle.transform.position != position)
        {
            Debug.LogError("WTF");
        }
		VehicleAI vAI = vehicle.GetComponent<VehicleAI>();

		chassis = AttachChassis(vehicle, vAI);
		cab = AttachCab(chassis, vAI, ref armorStacks, ref ramDamageStacks, ref speedStacks);
		cargo = AttachCargo(cab, vAI, ref armorStacks, ref ramDamageStacks, ref speedStacks);
		attachment = AttachAttachment(cab, vAI, ref armorStacks, ref ramDamageStacks, ref speedStacks);
		AttachPayload(cargo,wChance,batteries);
		AttachWheels(chassis, vAI);


		// set stats of newly created vehicle
		Chassis chassisScript = chassis.GetComponent<Chassis>();

		vAI.SetMaxHealth(chassisScript.GetBaseHealth() * (1 + armorStacks * Constants.ARMOR_TOTALHEALTH_MODIFIER_PER_STACK));
		vAI.SetRamDamage(ramDamageStacks);
		vAI.SetSpeed(chassisScript.baseSpeed * (1 + speedStacks * Constants.SPEED_MOVEMENT_MODIFIER_PER_STACK));
		vAI.SetMovementChance(speedStacks * Constants.SPEED_LOCATIONCHANGE_MODIFIER_PER_STACK);

		vehicle.GetComponent<eventObject>().setDifficulty(chassis.GetComponent<Chassis>().baseThreat 
			+ (int)cab.GetComponent<DestructiblePart>().threatModifier
			+ (int)cargo.GetComponent<DestructiblePart>().threatModifier
			+ (int)attachment.GetComponent<DestructiblePart>().threatModifier);


        return vehicle;
	}

	#region Component Attaching
	// set up chassis
	public GameObject AttachChassis(GameObject vehicle, VehicleAI v) {
		GameObject chassis = Instantiate(SelectChassis(), vehicle.transform);
        chassis.transform.localPosition = Vector3.zero;

		Chassis chassisScript = chassis.GetComponent<Chassis>();

		//speedStacks += chassisScript.baseSpeed;

		return chassis;
	}

	// attach cab to chassis
	public GameObject AttachCab(GameObject chassis, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject cab = Instantiate(SelectCab(), chassis.GetComponent<Chassis>().cabNode.transform);
        cab.transform.localPosition = Vector3.zero;
		Cab cabScript = cab.GetComponent<Cab>();

		armorStacks += cabScript.armorStacks;
		speedStacks += cabScript.speedStacks;
        ramDamageStacks += cabScript.ramDamageStacks;

        return cab;
	}

	// attach cargo to cab
	public GameObject AttachCargo(GameObject cab, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject cargo = Instantiate(SelectCargo(), cab.GetComponent<Cab>().cargoNode.transform);
        cargo.transform.localPosition = Vector3.zero;
        Cargo cargoScript = cargo.GetComponent<Cargo>();

		armorStacks += cargoScript.armorStacks;
		speedStacks += cargoScript.speedStacks;
        ramDamageStacks += cargoScript.ramDamageStacks;

        return cargo;
	}

	// attach attachment to cab
	public GameObject AttachAttachment(GameObject cab, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject front_attachment = Instantiate(SelectAttachment(), cab.GetComponent<Cab>().front_attachmentNode.transform);
        front_attachment.transform.localPosition = Vector3.zero;
        Attachment attachmentScript = front_attachment.GetComponent<Attachment>();
        
		armorStacks += attachmentScript.armorStacks;
<<<<<<< HEAD
        speedStacks += attachmentScript.speedStacks;
        ramDamageStacks += attachmentScript.ramDamageStacks;
=======
		ramDamageStacks += attachmentScript.ramDamageStacks;

		return front_attachment;
>>>>>>> Dev
	}
	#endregion

	#region Component Selectors
	protected GameObject SelectCab() {
		int selectedIndex = rand.Next(0, Cab.Count);
		return Cab[selectedIndex];
	}

	protected GameObject SelectCargo() {
		int selectedIndex = rand.Next(0, Cargo.Count);
		return Cargo[selectedIndex];
	}

	protected GameObject SelectWheel() {
		int selectedIndex = rand.Next(0, Wheel.Count);
		return Wheel[selectedIndex];
	}

	protected GameObject SelectAttachment() {
		int selectedIndex = rand.Next(0, Attachment.Count);
		return Attachment[selectedIndex];
	}

	protected GameObject SelectPayload() {
		int selectedIndex = rand.Next(0, Payload.Count);
		return Payload[selectedIndex];
	}
	#endregion

}
