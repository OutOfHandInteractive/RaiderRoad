using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
	// --------------------- Enumerations -------------------------
	public enum vehicleClass { light, medium, heavy };

	// --------------------- Dictionaries -------------------------
	public static Dictionary<vehicleClass, int> WallDropsByVehicleClass = new Dictionary<vehicleClass, int> {
		{ vehicleClass.light, 1 },
		{ vehicleClass.medium, 2 },
		{ vehicleClass.heavy, 3 }
	};

	// ------------------- Gameplay Values ------------------------

	// stats
	public const float VEHICLE_HEAVY_BASE_HEALTH = 300f;
	public const float VEHICLE_MEDIUM_BASE_HEALTH = 200f;
	public const float VEHICLE_LIGHT_BASE_HEALTH = 100f;
	public const float VEHICLE_HEAVY_PART_BASE_HEALTH = 90f;
	public const float VEHICLE_MEDIUM_PART_BASE_HEALTH = 60f;
	public const float VEHICLE_LIGHT_PART_BASE_HEALTH = 30f;
	public const float SPEED_MOVEMENT_MODIFIER_PER_STACK = 0.2f;
	public const float SPEED_LOCATIONCHANGE_MODIFIER_PER_STACK = 0.2f;
	public const float ARMOR_TOTALHEALTH_MODIFIER_PER_STACK = .20f;
	public const float ARMOR_PARTHEALTH_MODIFIER_PER_STACK = .10f;

    public const float DESTRUCTABLE_PART_DAMAGE_PER_HIT = 10f;

	// difficulty ratings
	public const int SMALL_OBSTACLE_BASE_THREAT = 1;
	public const int LIGHT_VEHICLE_BASE_THREAT = 1;
	public const int MEDIUM_VEHICLE_BASE_THREAT = 3;
    public const int HEAVY_VEHICLE_BASE_THREAT = 6;

	// misc
	public const float PLAYER_ROAD_DAMAGE = 25f;

	// player facing angles
	public const float FACING_VERTICAL_MINIMUM = 45f;
	public const float FACING_VERTICAL_MAXIMUM = 135f;
}
