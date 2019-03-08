using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryH : VehicleFactory_I {

	private const int WHEEL_COUNT_H = 6;

	public List<GameObject> Chassis;

	protected override GameObject selectChassis() {
		int selectedIndex = rand.Next(0, Chassis.Count);
		return Chassis[selectedIndex];
	}

	//attach enemy to cab
	public override void AttachPayload(GameObject cargo) {
		GameObject payload = Instantiate(selectPayload(), cargo.GetComponent<Cargo>().payloadNode.transform);
        payload.transform.localPosition = Vector3.zero;
        //payload.transform.SetParent(cargo.GetComponent<Cargo>().payloadNode.transform);
        //payload.transform.position = payload.transform.parent.transform.position;
        payload.GetComponent<PayloadH>().populate();
	}

	// attach wheel to frame
	public override void AttachWheels(GameObject chassis, VehicleAI v) {
		GameObject wheelToUse = selectWheel();
		GameObject wheel;
		wheel = Instantiate(wheelToUse, chassis.GetComponent<Chassis>().wheelNodes[0].transform);
        wheel.transform.localPosition = Vector3.zero;
        //wheel.transform.SetParent(chassis.GetComponent<Chassis>().wheelNodes[0].transform);
        //wheel.transform.position = wheel.transform.parent.transform.position;

        Wheel wheelScript = wheelToUse.GetComponent<Wheel>();
	}
}
