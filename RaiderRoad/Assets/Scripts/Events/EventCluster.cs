using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCluster : MonoBehaviour {

    public  List<Event> events = new List<Event>();
    [SerializeField]
    private GameObject manager;
    [SerializeField]
    private VehicleFactoryManager vFactory;
    private int difficulty;
    public int initSize;
    public float complete;
    [SerializeField]
    private float weight;
    [SerializeField]
    private float threshold;
    private bool spawnFlag = true;
    [SerializeField]
    private float delay;
    private int i = 0;          //needed to have this outside a function so that the coroutine doesn't mess up its value

    //constructor - need list of events
    /*public EventCluster(List<Event> sequence, VehicleFactoryManager factory){
        events = sequence;
        vFactory = factory;
    }

    //on start
    void Start()
    {
        manager = GameObject.Find("EventManager");
        initSize = events.Count;        //get number of events in cluster
        weight = 1 / initSize;             //determine weight of a single event for completeness
        foreach (Event element in events){           //sum of event difficulties
            difficulty += element.difficultyRating;
        }
        //start spawning
        StartCoroutine(dispense());
    }*/

    public void startUp(List<Event> sequence, VehicleFactoryManager factory)
    {
        manager = GameObject.Find("EventManager");
		vFactory = factory;
        events = sequence;
        initSize = events.Count;        //get number of events in cluster
        weight = 1 / initSize;             //determine weight of a single event for completeness
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
        while (true){
            if (i < initSize)
            {
                callEvent();
            }
            yield return new WaitForSeconds(delay);     //call next event after delay
        }
    }

    //increase completeness of cluster - called from vehicle on destroy
    public void updatePercent(){
        complete += weight;
        if(weight >= threshold && spawnFlag){   //if cluster completion at certain level & no new cluster has been called
            spawnFlag = false;                  //disable so only one new cluster gets generated
            manager.GetComponent<EventManager>().lastDone();        //call the generate function in manager
        }
    }
        
    //calls next event in cluster
    void callEvent(){
        events[i].spawn(vFactory);
        i++;
    }
}
