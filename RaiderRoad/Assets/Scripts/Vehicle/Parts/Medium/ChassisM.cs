using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChassisM : Chassis {

	public override float GetBaseHealth() {
		return Constants.VEHICLE_MEDIUM_BASE_HEALTH;
	}

}
