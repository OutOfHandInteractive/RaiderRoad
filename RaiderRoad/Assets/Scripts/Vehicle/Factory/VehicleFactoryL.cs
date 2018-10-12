using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryL : VehicleFactory_I {

	private GameObject vehicle, frame, cab, cargo, wheel, attachment;
	private static System.Random rand;

	public VehicleFactoryL() {
		rand = new System.Random();
	}

	public override void AssembleVehicle() {
		vehicle = Instantiate(VehicleBase, new Vector3(0, 0 ,0), Quaternion.identity);

		// set up frame
		frame = Instantiate(selectFrame());
		frame.transform.SetParent(vehicle.transform);
		frame.transform.position = Vector3.zero;

		// attach cab to frame
		cab = Instantiate(selectCab());
		cab.transform.SetParent(frame.GetComponent<FrameL>().cabNode.transform);
		cab.transform.position = cab.transform.parent.transform.position;

		// attach cargo to frame
		cargo = Instantiate(selectCargo());
		cargo.transform.SetParent(frame.GetComponent<FrameL>().cargoNode.transform);
		cargo.transform.position = cargo.transform.parent.transform.position;

		// attach attachment to frame
		attachment = Instantiate(selectAttachment());
		attachment.transform.SetParent(frame.GetComponent<FrameL>().attachmentNode.transform);
		attachment.transform.position = attachment.transform.parent.transform.position;

		// attach wheel to frame
		GameObject wheelToUse = selectWheel();
		for (int i=0; i<frame.GetComponent<FrameL>().getNumWheels(); i++) {
			wheel = Instantiate(wheelToUse);
			wheel.transform.SetParent(frame.GetComponent<FrameL>().wheelNodes[i].transform);
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
