using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChassisL : Chassis {

	public override float GetBaseHealth() {
		return Constants.VEHICLE_LIGHT_BASE_HEALTH;
	}

}
