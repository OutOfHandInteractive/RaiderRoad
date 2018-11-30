using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class Event : MonoBehaviour {
public class Event : MonoBehaviour {

	//public enum EventTypes { vehicle, obstacle, fork };
    //public enum vehicleTypes { light, medium, heavy };


	public int difficultyRating;
	public float postDelay;
    private VehicleFactoryManager.vehicleTypes _type;
    private int numPoints;
    private GameObject e;
    [SerializeField]
    private List<Transform> spawnPoints;

    /*public Event(int dif, VehicleFactoryManager.vehicleTypes type)       //add game object to constructor for spawning
    {
        Debug.Log("Event Created");
        difficultyRating = dif;
        _type = type;
    }*/

    public void initialize(int dif, VehicleFactoryManager.vehicleTypes type, List<Transform> spawns){    //constructor work-around
        difficultyRating = dif;
        _type = type;
        spawnPoints = spawns;
    }

    void Start()
    {
        Debug.Log("Event Created");
        //spawnPoints = new List<Transform>();
        

    }

    public void spawn(VehicleFactoryManager factory)
    {
        numPoints = Random.Range(1, spawnPoints.Count);
        Debug.Log("spawn called");
        //based on type, call proper function - for now just creates light vehicle
        e = factory.newConstructVehicle(_type);
        e.GetComponent<VehicleAI>().setSide(spawnPoints[numPoints].name);
        e.transform.position = spawnPoints[numPoints].transform.position;
		difficultyRating = e.GetComponentInChildren<eventObject>().getDifficulty();
        //GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
}
