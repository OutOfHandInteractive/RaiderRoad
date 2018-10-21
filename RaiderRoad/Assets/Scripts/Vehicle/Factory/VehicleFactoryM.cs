using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryM : VehicleFactory_I {

	public List<GameObject> Chassis;

	protected override GameObject selectChassis() {
		int selectedIndex = rand.Next(0, Chassis.Count);
		return Chassis[selectedIndex];
	}

	//attach enemy to cab
	public override void AttachEnemy(GameObject cab) {
		GameObject enemy = Instantiate(selectEnemy());
		enemy.transform.SetParent(cab.GetComponent<CabL>().cargoNode.transform);
		enemy.transform.position = enemy.transform.parent.transform.position;
	}

	// attach wheel to frame
	public override void AttachWheels(GameObject chassis) {
		GameObject wheelToUse = selectWheel();
		GameObject wheel;
		for (int i = 0; i < chassis.GetComponent<ChassisL>().getNumWheels(); i++) {
			wheel = Instantiate(wheelToUse);
			wheel.transform.SetParent(chassis.GetComponent<ChassisL>().wheelNodes[i].transform);
			wheel.transform.position = wheel.transform.parent.transform.position;
			if (i % 2 == 1) { // even-numbered wheels are driver-side, so odd need to be scaled to -1 in X
				wheel.transform.localScale = new Vector3(-1 * wheel.transform.localScale.x, 1 * wheel.transform.localScale.y, 1 * wheel.transform.localScale.z);
			}
		}
	}
}
