using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryL : VehicleFactory_I {

	private GameObject vehicle, chassis, cab, cargo, wheel, front_attachment;
	private static System.Random rand;

	public VehicleFactoryL() {
		rand = new System.Random();
	}

	public override void AssembleVehicle() {
		vehicle = Instantiate(VehicleBase, new Vector3(0, 0 ,0), Quaternion.identity);

		// set up chassis
		chassis = Instantiate(selectFrame());
		chassis.transform.SetParent(vehicle.transform);
		chassis.transform.position = Vector3.zero;

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

		// attach wheel to frame
		GameObject wheelToUse = selectWheel();
		for (int i=0; i<chassis.GetComponent<ChassisL>().getNumWheels(); i++) {
			wheel = Instantiate(wheelToUse);
			wheel.transform.SetParent(chassis.GetComponent<ChassisL>().wheelNodes[i].transform);
			wheel.transform.position = wheel.transform.parent.transform.position;
		}
	}

	private GameObject selectFrame() {
		int selectedIndex = rand.Next(0, Frame.Count);
		return Frame[selectedIndex];
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
