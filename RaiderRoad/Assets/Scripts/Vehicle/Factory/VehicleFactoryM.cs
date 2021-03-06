﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryM : VehicleFactory_I {

	private const int WHEEL_COUNT_M = 4;

	public List<GameObject> Chassis;

	protected override GameObject SelectChassis() {
		int selectedIndex = rand.Next(0, Chassis.Count);

		return Chassis[selectedIndex];
	}

	//attach enemy to cab
	public override void AttachPayload(GameObject cargo, float wChance) {
		GameObject payload = Instantiate(SelectPayload(), cargo.GetComponent<Cargo>().payloadNode.transform);
        payload.transform.localPosition = Vector3.zero;
        //payload.transform.SetParent(cargo.GetComponent<Cargo>().payloadNode.transform);
        //payload.transform.position = payload.transform.parent.transform.position;
        payload.GetComponent<PayloadM>().SetWChance(wChance);
        payload.GetComponent<PayloadM>().Populate();
	}

	// attach wheel to frame
	public override void AttachWheels(GameObject chassis, VehicleAI v) {
		GameObject wheelToUse = SelectWheel();
		GameObject wheel;
		for (int i = 0; i < WHEEL_COUNT_M; i++) {
			wheel = Instantiate(wheelToUse, chassis.GetComponent<Chassis>().wheelNodes[i].transform);
            wheel.transform.localPosition = Vector3.zero;
            //wheel.transform.SetParent(chassis.GetComponent<Chassis>().wheelNodes[i].transform);
            //wheel.transform.position = wheel.transform.parent.transform.position;
            if (i % 2 == 1) { // even-numbered wheels are driver-side, so odd need to be scaled to -1 in X
				wheel.transform.localScale = new Vector3(1 * wheel.transform.localScale.x, -1 * wheel.transform.localScale.y, 1 * wheel.transform.localScale.z);
			}
		}

		Wheel wheelScript = wheelToUse.GetComponent<Wheel>();
	}
}
