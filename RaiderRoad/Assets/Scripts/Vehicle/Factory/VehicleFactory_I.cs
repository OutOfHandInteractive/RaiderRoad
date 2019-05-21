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
	/// <summary>
	/// Randomly choose a chassis from the available pool
	/// </summary>
	/// <returns>A reference to the chosen chassis</returns>
	protected abstract GameObject SelectChassis();

	/// <summary>
	/// Select and attach a payload to the vehicle being created
	/// </summary>
	/// <param name="cargo">The cargo bed of the vehicle being built</param>
	/// <param name="wChance">The chance of spawning a weapon on the vehicle</param>
	/// <param name="batteries">The number of batteries to spawn on the vehicle</param>
	public abstract void AttachPayload(GameObject cargo, float wChance, int batteries);

	/// <summary>
	/// Select and attach wheels to the vehicle being assembled
	/// </summary>
	/// <param name="chassis">The chassis of the vehicle being built</param>
	/// <param name="v">The VehicleAI script of the vehicle being built</param>
	public abstract void AttachWheels(GameObject chassis, VehicleAI v);
	#endregion

	public VehicleFactory_I() {
		rand = new System.Random();
	}

	/// <summary>
	/// Call to assemble a vehicle object from component parts
	/// </summary>
	/// <param name="modifier">Desired threat rating of vehicle to be created</param>
	/// <param name="position">Location in 3D space at which the vehicle is to be spawned</param>
	/// <param name="wChance">Chance the vehicle will spawn carrying a mounted weapon</param>
	/// <param name="batteries">Number of batteries to place on spawned vehicle</param>
	/// <returns></returns>
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
			+ cab.GetComponent<DestructiblePart>().threatModifier
			+ cargo.GetComponent<DestructiblePart>().threatModifier
			+ attachment.GetComponent<DestructiblePart>().threatModifier);


        return vehicle;
	}

	#region Component Attaching
	/// <summary>
	/// Select and attach a chassis as the base of a vehicle
	/// </summary>
	/// <param name="vehicle">The vehicle the chassis is to be used for. It is used as a position at which to place the chassis</param>
	/// <param name="v">The VehicleAI of the vehicle the chassis will be used for</param>
	/// <returns>A reference to the chassis created</returns>
	public GameObject AttachChassis(GameObject vehicle, VehicleAI v) {
		GameObject chassis = Instantiate(SelectChassis(), vehicle.transform);
        chassis.transform.localPosition = Vector3.zero;

		Chassis chassisScript = chassis.GetComponent<Chassis>();

		//speedStacks += chassisScript.baseSpeed;

		return chassis;
	}

	/// <summary>
	/// Select and attach a cab for a vehicle
	/// </summary>
	/// <param name="chassis">The chassis of the vehicle being assembled</param>
	/// <param name="v">The VehicleAI script of the vehicle the cab will be used for</param>
	/// <param name="armorStacks">Reference variable to a general count of armor stacks used by the calling function</param>
	/// <param name="ramDamageStacks">Reference variable to a general count of ram damage stacks used by the calling function</param>
	/// <param name="speedStacks">Reference variable to a general count of speed stacks used by the calling function</param>
	/// <returns>A reference to the cab created</returns>
	public GameObject AttachCab(GameObject chassis, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject cab = Instantiate(SelectCab(), chassis.GetComponent<Chassis>().cabNode.transform);
        cab.transform.localPosition = Vector3.zero;
		Cab cabScript = cab.GetComponent<Cab>();

		armorStacks += cabScript.armorStacks;
		speedStacks += cabScript.speedStacks;
        ramDamageStacks += cabScript.ramDamageStacks;

        return cab;
	}

	/// <summary>
	/// Select and attach a cargo bed for a vehicle
	/// </summary>
	/// <param name="cab">The cab of the vehicle being assembled</param>
	/// <param name="v">The VehicleAI script of the vehicle the cab will be used for</param>
	/// <param name="armorStacks">Reference variable to a general count of armor stacks used by the calling function</param>
	/// <param name="ramDamageStacks">Reference variable to a general count of ram damage stacks used by the calling function</param>
	/// <param name="speedStacks">Reference variable to a general count of speed stacks used by the calling function</param>
	/// <returns>A reference to the cargo bed created</returns>
	public GameObject AttachCargo(GameObject cab, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject cargo = Instantiate(SelectCargo(), cab.GetComponent<Cab>().cargoNode.transform);
        cargo.transform.localPosition = Vector3.zero;
        Cargo cargoScript = cargo.GetComponent<Cargo>();

		armorStacks += cargoScript.armorStacks;
		speedStacks += cargoScript.speedStacks;
        ramDamageStacks += cargoScript.ramDamageStacks;

        return cargo;
	}

	/// <summary>
	/// Select and attach a hood/hood attachment for a vehicle
	/// </summary>
	/// <param name="cab">The cab of the vehicle being assembled</param>
	/// <param name="v">The VehicleAI script of the vehicle the cab will be used for</param>
	/// <param name="armorStacks">Reference variable to a general count of armor stacks used by the calling function</param>
	/// <param name="ramDamageStacks">Reference variable to a general count of ram damage stacks used by the calling function</param>
	/// <param name="speedStacks">Reference variable to a general count of speed stacks used by the calling function</param>
	/// <returns>A reference to the hood/hood attachment created</returns>
	public GameObject AttachAttachment(GameObject cab, VehicleAI v, ref float armorStacks, ref float ramDamageStacks, ref float speedStacks) {
		GameObject front_attachment = Instantiate(SelectAttachment(), cab.GetComponent<Cab>().front_attachmentNode.transform);
        front_attachment.transform.localPosition = Vector3.zero;
        Attachment attachmentScript = front_attachment.GetComponent<Attachment>();
        
		armorStacks += attachmentScript.armorStacks;
        speedStacks += attachmentScript.speedStacks;
        ramDamageStacks += attachmentScript.ramDamageStacks;

		return front_attachment;
	}
	#endregion

	#region Component Selectors
	/// <summary>
	/// Randomly chooses a cab from the available pool
	/// </summary>
	/// <returns>A reference to the chosen cab</returns>
	protected GameObject SelectCab() {
		int selectedIndex = rand.Next(0, Cab.Count);
		return Cab[selectedIndex];
	}

	/// <summary>
	/// Randomly chooses a cargo bed from the available pool
	/// </summary>
	/// <returns>A reference to the chosen cargo bed</returns>
	protected GameObject SelectCargo() {
		int selectedIndex = rand.Next(0, Cargo.Count);
		return Cargo[selectedIndex];
	}

	/// <summary>
	/// Randomly chooses a wheel from the available pool
	/// </summary>
	/// <returns>A reference to the wheel chosen</returns>
	protected GameObject SelectWheel() {
		int selectedIndex = rand.Next(0, Wheel.Count);
		return Wheel[selectedIndex];
	}

	/// <summary>
	/// Randomly chooses a hood/hood attachment from the available pool
	/// </summary>
	/// <returns>A reference to the chosen hood/hood attachment</returns>
	protected GameObject SelectAttachment() {
		int selectedIndex = rand.Next(0, Attachment.Count);
		return Attachment[selectedIndex];
	}

	/// <summary>
	/// Randomly chooses a payload from the available pool
	/// </summary>
	/// <returns>A reference to the chosen payload</returns>
	protected GameObject SelectPayload() {
		int selectedIndex = rand.Next(0, Payload.Count);
		return Payload[selectedIndex];
	}
	#endregion

}
