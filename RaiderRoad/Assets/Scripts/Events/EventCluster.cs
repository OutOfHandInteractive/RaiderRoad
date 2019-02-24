using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCluster : MonoBehaviour {

	// ----------------------- public variables ----------------------
	// references
    public List<Event> events = new List<Event>();

	// gameplay values
	public int initSize;
	public float complete;

	// ---------------------- nonpublic variables ----------------------
	// references
	[SerializeField] private GameObject manager;
    [SerializeField] private VehicleFactoryManager vFactory;
	[SerializeField] private GameObject _obstacle;

	// gameplay values
	private int difficulty;
    [SerializeField] private float weight;
    [SerializeField] private float threshold;
    private bool spawnFlag = true;
    [SerializeField] private float delay = 15;   //for testing 15, seconds
    //private int i = 0;          //needed to have this outside a function so that the coroutine doesn't mess up its value

    public void startUp(List<Event> sequence, VehicleFactoryManager factory)
    {
        manager = GameObject.Find("EventManager");
		vFactory = factory;
        events = sequence;
        initSize = events.Count;        //get number of events in cluster
        weight = 1.0f / initSize;             //determine weight of a single event for completeness
        threshold = weight * (initSize-1);      //this should probably be replaced with something better but for now it works
        foreach (Event element in events)
        {           //sum of event difficulties
            difficulty += element.difficultyRating;
        }
    }


    public void startDispense()
    {
        //start spawning
        StartCoroutine(dispense());
    }

    //spawning coroutine
    IEnumerator dispense(){
        foreach (Event e in events){
            callEvent(e);
            yield return new WaitForSeconds(delay);     //call next event after delay
        }
    }

    //increase completeness of cluster - called from vehicle on destroy
    public void updatePercent(){
        complete += weight;
        if(complete >= threshold && spawnFlag){   //if cluster completion at certain level & no new cluster has been called
            spawnFlag = false;                  //disable so only one new cluster gets generated
            manager.GetComponent<EventManager>().lastDone();        //call the generate function in manager
        }else if (complete >= 1){
            Debug.Log("cluster complete");
            Destroy(this.gameObject);
        }
    }
        
    //calls next event in cluster
    void callEvent(Event eve){
        if (eve._etype == EventManager.eventTypes.obstacle)
        {
            eve.oSpawn(_obstacle);
        }
        else if (eve._etype == EventManager.eventTypes.vehicle)
        {
            eve.spawn(vFactory);
        }
    }
}
