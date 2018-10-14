using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryL : VehicleFactory_I {

	private GameObject vehicle, chassis, cab, cargo, wheel, front_attachment;
	private static System.Random rand;

	private float vehicleHealth = 0f;
	private float vehicleRamDamage = 0f;
	private float vehicleSpeed = 0f;

	public VehicleFactoryL() {
		rand = new System.Random();
	}

	public override void AssembleVehicle() {
		vehicle = Instantiate(VehicleBase, new Vector3(0, 0 ,0), Quaternion.identity);

		// set up chassis
		chassis = Instantiate(selectChassis());
		chassis.transform.SetParent(vehicle.transform);
		chassis.transform.position = Vector3.zero;
		ChassisL chassisScript = chassis.GetComponent<ChassisL>();
		vehicleHealth += chassisScript.baseHealth;
		vehicleRamDamage += chassisScript.baseRamDamage;
		vehicleSpeed += chassisScript.baseSpeed;

		// attach cab to chassis
		cab = Instantiate(selectCab());
		cab.transform.SetParent(chassis.GetComponent<ChassisL>().cabNode.transform);
		cab.transform.position = cab.transform.parent.transform.position;

		// attach cargo to cab
		cargo = Instantiate(selectCargo());
		cargo.transform.SetParent(cab.GetComponent<CabL>().cargoNode.transform);
		cargo.transform.position = cargo.transform.parent.transform.position;

		// attach attachment to cab
		front_attachment = Instantiate(selectAttachment());
		front_attachment.transform.SetParent(cab.GetComponent<CabL>().front_attachmentNode.transform);
		front_attachment.transform.position = front_attachment.transform.parent.transform.position;
		AttachmentL attachmentScript = front_attachment.GetComponent<AttachmentL>();
		vehicleHealth += attachmentScript.healthModifier;
		vehicleRamDamage += attachmentScript.ramDamageModifier;

		// attach wheel to frame
		GameObject wheelToUse = selectWheel();
		for (int i=0; i<chassis.GetComponent<ChassisL>().getNumWheels(); i++) {
			if (i%2 == 1) { // even-numbered wheels are driver-side, so odd need to be scaled to -1 in X
				wheel = Instantiate(wheelToUse);
				wheel.transform.localScale = new Vector3(-1, 1, 1);
				wheel.transform.SetParent(chassis.GetComponent<ChassisL>().wheelNodes[i].transform);
				wheel.transform.position = wheel.transform.parent.transform.position;
			}
			else {
				wheel = Instantiate(wheelToUse);
				wheel.transform.SetParent(chassis.GetComponent<ChassisL>().wheelNodes[i].transform);
				wheel.transform.position = wheel.transform.parent.transform.position;
			}		
		}

		VehicleAI vAI = vehicle.GetComponent<VehicleAI>();
		vAI.setMaxHealth(vehicleHealth);
		vAI.setRamDamage(vehicleRamDamage);
		vAI.setSpeed(vehicleSpeed);
	}

	private GameObject selectChassis() {
		int selectedIndex = rand.Next(0, Chassis.Count);
		return Chassis[selectedIndex];
	}

	private GameObject selectCab() {
		int selectedIndex = rand.Next(0, Cab.Count);
		return Cab[selectedIndex];
	}

	private GameObject selectCargo() {
		int selectedIndex = rand.Next(0, Cargo.Count);
		return Cargo[selectedIndex];
	}

	private GameObject selectWheel() {
		int selectedIndex = rand.Next(0, Wheel.Count);
		return Wheel[selectedIndex];
	}

	private GameObject selectAttachment() {
		int selectedIndex = rand.Next(0, Attachment.Count);
		return Attachment[selectedIndex];
	}
}
