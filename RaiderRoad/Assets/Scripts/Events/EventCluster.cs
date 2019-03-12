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

    //spawn reduction variables
    [SerializeField] private float sDelay = 15;   //to start 15, seconds
    private float sDelayLower = 1f;  //delay will not be less than this
    private float sFactor = 0.9f;    //less than overall so the spawns don't increase as fast (since the starting point will keep dropping from manager)

    //weapon frequency
    [SerializeField]
    private float wChance;   //gets set by event manager


    public void startUp(List<Event> sequence, VehicleFactoryManager factory, float _sDelay, float _wChance)
    {
        manager = GameObject.Find("EventManager");
		vFactory = factory;
        events = sequence;
        sDelay = _sDelay;                //update delay to proper val from manager
        wChance = _wChance;               //set weapon chance from manager
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
        StartCoroutine(reduceDelay());      //start reducing time between spawns
    }

    //spawning coroutine
    IEnumerator dispense(){
        foreach (Event e in events){
            callEvent(e);
            yield return new WaitForSeconds(sDelay);     //call next event after delay
        }
    }
    
    //delay reduction coroutine
    IEnumerator reduceDelay()
    {
        yield return new WaitForSeconds(sDelay);        //wait allocated time
        while(true){
            if (sDelay*sFactor > sDelayLower){      //if next iteration is greater than lower bound...
                sDelay = sDelay * sFactor;       //...decrement
            }
            else{
                sDelay = sDelayLower;              //else, reduce to lower bound
            }
            yield return new WaitForSeconds(sDelay);        //wait allocated time
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
            eve.spawn(vFactory, wChance);
        }
    }
}
