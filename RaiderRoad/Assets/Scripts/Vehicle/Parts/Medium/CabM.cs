using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabM : Cab {
	protected override float GetMaxHealth() {
		return Constants.VEHICLE_MEDIUM_PART_BASE_HEALTH;
	}
}
