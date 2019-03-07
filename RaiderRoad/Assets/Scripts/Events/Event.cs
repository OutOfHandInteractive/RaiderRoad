using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class Event : MonoBehaviour {
public class Event : MonoBehaviour {

	//public enum EventTypes { vehicle, obstacle, fork };
    //public enum vehicleTypes { light, medium, heavy };

	// ---------------- public variables ------------------
	// gameplay values
	public int difficultyRating;
	public float postDelay;
	public EventManager.eventTypes _etype;

	// -------------- nonpublic variables -----------------
	// references
	private GameObject e;
	[SerializeField] private List<Transform> spawnPoints;

	// gameplay values
	private VehicleFactoryManager.vehicleTypes _vtype;
    private int numPoints;
    private int _mod;

    /*public Event(int dif, VehicleFactoryManager.vehicleTypes type)       //add game object to constructor for spawning
    {
        Debug.Log("Event Created");
        difficultyRating = dif;
        _type = type;
    }*/

    public void initialize(int dif, VehicleFactoryManager.vehicleTypes vtype,  EventManager.eventTypes etype, List<Transform> spawns){    //constructor work-around
        difficultyRating = dif;
        _vtype = vtype;
        _etype = etype;
        spawnPoints = spawns;
    }

    public void setMod(int mod)
    {
        _mod = mod;
    }

    void Start()
    {
        Debug.Log("Event Created");
    }

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

    public void spawn(VehicleFactoryManager factory)
    {
        numPoints = Random.Range(1, spawnPoints.Count);
        Debug.Log("spawn = " + numPoints);
        //Debug.Log("spawn called");
        //based on type, call proper function - for now just creates light vehicle
        Vector3 pos = spawnPoints[numPoints].transform.position;
        Debug.LogWarning(pos);
        e = factory.newConstructVehicle(_vtype,_mod, pos);
        if (e.transform.position != pos)
        {
            Debug.LogError("WTF");
        }
        e.GetComponent<VehicleAI>().setSide(spawnPoints[numPoints].name);
        //e.transform.position = spawnPoints[numPoints].transform.position;
        e.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
		difficultyRating = e.GetComponentInChildren<eventObject>().getDifficulty();
        //GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

}
