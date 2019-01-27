using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class Event : MonoBehaviour {
public class Event : MonoBehaviour {

	//public enum EventTypes { vehicle, obstacle, fork };
    //public enum vehicleTypes { light, medium, heavy };


	public int difficultyRating;
	public float postDelay;
    private VehicleFactoryManager.vehicleTypes _vtype;
    public EventManager.eventTypes _etype;
    private int numPoints;
    private GameObject e;
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private GameObject obstacle;

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

    void Start()
    {
        Debug.Log("Event Created");
    }

    public void oSpawn()
    {
        int i = Random.Range(1, 6);
        Vector3 spawnPoint = spawnPoints[i].transform.position;
        GameObject newObstacle = Instantiate(obstacle,spawnPoint,Quaternion.identity);    /////need obstacle prefab

    }

    public void spawn(VehicleFactoryManager factory)
    {
        numPoints = Random.Range(1, spawnPoints.Count);
        Debug.Log("spawn = " + numPoints);
        //Debug.Log("spawn called");
        //based on type, call proper function - for now just creates light vehicle
        e = factory.newConstructVehicle(_vtype);
        e.GetComponent<VehicleAI>().setSide(spawnPoints[numPoints].name);
        e.transform.position = spawnPoints[numPoints].transform.position;
        e.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
		difficultyRating = e.GetComponentInChildren<eventObject>().getDifficulty();
        //GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

}
