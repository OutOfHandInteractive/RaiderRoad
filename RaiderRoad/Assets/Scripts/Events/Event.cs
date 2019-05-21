﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A wrapper to create a raider vehicle and used to control spawn rate
/// </summary>
public class Event : MonoBehaviour {

	//public enum EventTypes { vehicle, obstacle, fork };
    //public enum vehicleTypes { light, medium, heavy };

	// ---------------- public variables ------------------
	// gameplay values
    [Header("public gameplay values")]
	public int difficultyRating;
	public float postDelay;
	public EventManager.eventTypes _etype;

	// -------------- nonpublic variables -----------------
	// references
    [Header("References")]
	private GameObject e;
	[SerializeField] private List<Transform> spawnPoints;

	// gameplay values
    [Header("nonpublic gameplay values")]
	private VehicleFactoryManager.vehicleTypes _vtype;
    private int numPoints;
    private int _mod;

<<<<<<< HEAD
    /// <summary>
    /// Sets values for the object to be spawned, including vehicle type, difficulty, and potential spawn points (event type not used)
    /// </summary>
    /// /// <param name="dif">The difficulty of the event</param>
    /// /// <param name="vtype">The type of vehicle</param>
    /// /// <param name="etype">The type of event (not used)</param>
    /// /// <param name="spawns">The list of spawn point coordinates</param>
=======
	/// <summary>
	/// Initialize creation of event
	/// </summary>
	/// <param name="dif">Desired difficulty of vehicle</param>
	/// <param name="vtype">Type of vehicle to spawn</param>
	/// <param name="etype">Type of event that is to be spawned (vehicle or obstacle)</param>
	/// <param name="spawns">Possible spawn locations for event</param>
>>>>>>> c4e7ac87ec7f93590454cf07e34a85c8aab7a6f1
    public void initialize(int dif, VehicleFactoryManager.vehicleTypes vtype,  EventManager.eventTypes etype, List<Transform> spawns){    //constructor work-around
        difficultyRating = dif;
        _vtype = vtype;
        _etype = etype;
        spawnPoints = spawns;
    }
<<<<<<< HEAD
    
    /// <summary>
    /// Sets the modifier variable in the given object (currently unused/has no effect)
    /// </summary>
    /// /// <param name="mod">The value of the modifier</param>
    public void setMod(int mod)
    {
=======

	/// <summary>
	/// Set difficulty modifier of event - Curently unused
	/// </summary>
	/// <param name="mod">Desired difficulty modifier</param>
    public void setMod(int mod) {
>>>>>>> c4e7ac87ec7f93590454cf07e34a85c8aab7a6f1
        _mod = mod;
    }

	/// <summary>
	/// Deprecated - Spawn obstacle into the game
	/// </summary>
	/// <param name="obstacle">The obstacle prefab to spawn</param>
    public void oSpawn(GameObject obstacle) {
        int i = Random.Range(0, 5);
        Vector3 spawnPoint = spawnPoints[i].transform.position;
        if(obstacle != null)
        {
            GameObject newObstacle = Instantiate(obstacle, spawnPoint, Quaternion.identity);    /////need obstacle prefab
            newObstacle.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
            newObstacle.transform.Rotate(0f, 90f, 0f);    //kinda a bullshit fix for now - i'll explain and fix better at testing
        }
        else
        {
            Debug.LogError("No obstacle object assigned! Did someone fuck up the prefab?");
        }
    }
<<<<<<< HEAD
    
    /// <summary>
    /// Picks a random spawn point, assembles a vehicle, and initializes it
    /// </summary>
    /// /// <param name="factory">The factory object that will create the vehicle objcet</param>
    /// /// <param name="wepFreq">The likelihood of a vehicle having a weapon attached</param>
    public void spawn(VehicleFactoryManager factory, float wepFreq)
    {
=======

	/// <summary>
	/// Spawn vehicle into the game
	/// </summary>
	/// <param name="factory">Reference to factory to build the vehicle</param>
	/// <param name="wepFreq">Likelihood of spawning weapon on the vehicle</param>
    public void spawn(VehicleFactoryManager factory, float wepFreq) {
>>>>>>> c4e7ac87ec7f93590454cf07e34a85c8aab7a6f1
        numPoints = Random.Range(1, spawnPoints.Count);

        //based on type, call proper function - for now just creates light vehicle
        Vector3 pos = spawnPoints[numPoints].transform.position;
        e = factory.NewConstructVehicle(_vtype,_mod, pos, wepFreq);
        Radio.GetRadio().AddVehicle(e);

        e.GetComponent<VehicleAI>().setSide(spawnPoints[numPoints].name);
        //e.transform.position = spawnPoints[numPoints].transform.position;
        e.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
		difficultyRating = e.GetComponentInChildren<eventObject>().getDifficulty();
    }
}
