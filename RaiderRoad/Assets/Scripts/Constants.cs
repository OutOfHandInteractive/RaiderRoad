﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
	// ------------------- Gameplay Values ------------------------
	// stats
	public static float VEHICLE_HEAVY_BASE_HEALTH = 300f;
	public static float VEHICLE_MEDIUM_BASE_HEALTH = 200f;
	public static float VEHICLE_LIGHT_BASE_HEALTH = 100f;
	public static float VEHICLE_HEAVY_PART_BASE_HEALTH = 90f;
	public static float VEHICLE_MEDIUM_PART_BASE_HEALTH = 60f;
	public static float VEHICLE_LIGHT_PART_BASE_HEALTH = 30f;
	public static float SPEED_MOVEMENT_MODIFIER_PER_STACK = 0.2f;
	public static float SPEED_LOCATIONCHANGE_MODIFIER_PER_STACK = 0.2f;
	public static float ARMOR_TOTALHEALTH_MODIFIER_PER_STACK = .20f;
	public static float ARMOR_PARTHEALTH_MODIFIER_PER_STACK = .10f;

	// difficulty ratings
	public static int SMALL_OBSTACLE_BASE_THREAT = 1;
	public static int LIGHT_VEHICLE_BASE_THREAT = 1;
	public static int MEDIUM_VEHICLE_BASE_THREAT = 6;
    public static int HEAVY_VEHICLE_BASE_THREAT = 12;

	// misc
	public static float PLAYER_ROAD_DAMAGE = 50f;

    // ------------------- GameObject Tags ------------------------
    public static string PLAYER_TAG = "Player";
    public static string ENEMY_TAG = "Enemy";
    public static string RV_TAG = "RV";
    public static string VEHICLE_TAG = "eVehicle";
    public static string CANNON_TAG = "Cannon";
    public static string FIRE_TAG = "Fire";
    public static string WALL_TAG = "Wall";
    public static string ENGINE_TAG = "Engine";
    public static string ROAD_TAG = "road";

}
