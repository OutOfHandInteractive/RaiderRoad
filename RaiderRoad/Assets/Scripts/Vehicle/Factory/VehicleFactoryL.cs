using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryL : VehicleFactory_I {

	private const int WHEEL_COUNT_L = 4;

	public List<GameObject> Chassis;

	protected override GameObject selectChassis() {
		int selectedIndex = rand.Next(0, Chassis.Count);
		return Chassis[selectedIndex];
	}

	//attach enemy to cab
	public override void AttachPayload(GameObject cargo) {
		GameObject payload = Instantiate(selectPayload(), cargo.GetComponent<Cargo>().payloadNode.transform);
		//payload.transform.SetParent(cargo.GetComponent<Cargo>().payloadNode.transform);
		//payload.transform.position = payload.transform.parent.transform.position;
		payload.GetComponent<PayloadL>().populate();
	}

	// attach wheel to frame
	public override void AttachWheels(GameObject chassis, VehicleAI v) {
		GameObject wheelToUse = selectWheel();
		GameObject wheel;
		for (int i = 0; i < WHEEL_COUNT_L; i++) {
			wheel = Instantiate(wheelToUse, chassis.GetComponent<ChassisL>().wheelNodes[i].transform);
			//wheel.transform.SetParent(chassis.GetComponent<ChassisL>().wheelNodes[i].transform);
			//wheel.transform.position = wheel.transform.parent.transform.position;
			if (i % 2 == 1) { // even-numbered wheels are driver-side, so odd need to be scaled to -1 in X
				wheel.transform.localScale = new Vector3(-1 * wheel.transform.localScale.x, 1 * wheel.transform.localScale.y, 1 * wheel.transform.localScale.z);
			}
		}

		Wheel wheelScript = wheelToUse.GetComponent<Wheel>();
	}
}
