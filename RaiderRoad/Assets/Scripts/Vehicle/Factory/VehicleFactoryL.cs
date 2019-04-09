using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryL : VehicleFactory_I {

	private const int WHEEL_COUNT_L = 4;

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
        payload.GetComponent<PayloadL>().SetWChance(wChance);
        payload.GetComponent<PayloadL>().Populate();
	}

	// attach wheel to frame
	public override void AttachWheels(GameObject chassis, VehicleAI v) {
		GameObject wheelToUse = SelectWheel();
		GameObject wheel;
		wheel = Instantiate(wheelToUse, chassis.GetComponent<Chassis>().wheelNodes[0].transform);
		wheel.transform.localPosition = Vector3.zero;

		Wheel wheelScript = wheelToUse.GetComponent<Wheel>();
	}
}
