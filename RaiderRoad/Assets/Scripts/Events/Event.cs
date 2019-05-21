using System.Collections;
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

    /// <summary>
    /// Sets values for the object to be spawned, including vehicle type, difficulty, and potential spawn points (event type not used)
    /// </summary>
    /// /// <param name="dif">The difficulty of the event</param>
    /// /// <param name="vtype">The type of vehicle</param>
    /// /// <param name="etype">The type of event (not used)</param>
    /// /// <param name="spawns">The list of spawn point coordinates</param>
    public void initialize(int dif, VehicleFactoryManager.vehicleTypes vtype,  EventManager.eventTypes etype, List<Transform> spawns){    //constructor work-around
        difficultyRating = dif;
        _vtype = vtype;
        _etype = etype;
        spawnPoints = spawns;
    }
    
    /// <summary>
    /// Sets the modifier variable in the given object (currently unused/has no effect)
    /// </summary>
    /// /// <param name="mod">The value of the modifier</param>
    public void setMod(int mod)
    {
        _mod = mod;
    }

    void Start()
    {
        //Debug.Log("Event Created");
    }

    /// <summary>
    /// OUTDATED - spawns an obstacle at a given point
    /// </summary>
    /// /// <param name="obstacle">The obstacle object to be created</param>
    public void oSpawn(GameObject obstacle)
    {
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
    
    /// <summary>
    /// Picks a random spawn point, assembles a vehicle, and initializes it
    /// </summary>
    /// /// <param name="factory">The factory object that will create the vehicle objcet</param>
    /// /// <param name="wepFreq">The likelihood of a vehicle having a weapon attached</param>
    public void spawn(VehicleFactoryManager factory, float wepFreq)
    {
        numPoints = Random.Range(1, spawnPoints.Count);
        //Debug.Log("spawn = " + numPoints);
        //Debug.Log("spawn called");
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
