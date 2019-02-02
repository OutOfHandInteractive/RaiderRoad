using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoH : Cargo {
	protected override float GetMaxHealth() {
		return Constants.VEHICLE_HEAVY_PART_BASE_HEALTH;
	}
}
