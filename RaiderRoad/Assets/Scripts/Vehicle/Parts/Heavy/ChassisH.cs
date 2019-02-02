using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChassisH : Chassis {
	public override float GetBaseHealth() {
		return Constants.VEHICLE_HEAVY_BASE_HEALTH;
	}
}
