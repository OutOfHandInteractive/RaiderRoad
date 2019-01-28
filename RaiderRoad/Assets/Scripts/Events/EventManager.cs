using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EventManager : MonoBehaviour {

    public enum eventTypes { vehicle, obstacle };
    public float TimeBetweenDifficultyAdjustment = 60;     //for now, difficulty updated every minute
    [SerializeField]
    private int difficultyRating = 1;       //set to 1 for testing
    public VehicleFactoryManager vFactory;


    // Equation coefficients                    -------all set to 1 for now
    public float expectedGameLengthModifier = 1;
    public float sinFrequencyModifier = 1;
    public float sinAmplitudeModifier = 1;
    public float difficultySlopeModifier = 1;
    public float baseDifficultyRating = 1;

    // Variation coefficients
    public float randomModifierMin;
    public float randomModifierMax;

    // event generation constants
    public int minEventDifficulty;
    public int maxEventDifficulty;
    
    //cluster objects - prefab, currently active, and next ready
    [SerializeField]
    private GameObject eCluster;
    [SerializeField]
    private GameObject onDeck;
    [SerializeField]
    private GameObject active;
    
    //spawn points for events
    [SerializeField]
    private List<Transform> vspawnPoints;
    public GameObject oSpawnsParent;
    [SerializeField]
    private List<Transform> ospawnPoints;

    void Start(){
        //StartCoroutine(difficultyManager());
        vspawnPoints = new List<Transform>();
        foreach (Transform child in transform)      //get vehicle spawn points
        {
            Debug.Log(child);
            vspawnPoints.Add(child);
        }
        ospawnPoints = new List<Transform>();
        foreach (Transform child in oSpawnsParent.transform)      //get obstacle spawn points
        {
            Debug.Log(child);
            ospawnPoints.Add(child);
        }
        StartCoroutine(initialize());                   //initializes first cluster
    }

    IEnumerator initialize()
    {
        onDeck = generate(difficultyRating);                //create event cluster at starting difficulty and set as on-deck
        yield return new WaitForSecondsRealtime(10);       //delay for some short time - let's say 30 seconds for now/10 for testing
        lastDone();                                     //switches on-deck to active, deploys it, and creates new on-deck cluster
        StartCoroutine(difficultyManager());
    }

    //called from last cluster generated once it reaches certain threshold - deploys next cluster and generates a new one on deck
    public void lastDone()
    {
        active = onDeck;
        deployActive();               //deploys 'active' cluster
        onDeck = generate(difficultyRating);
    }

    void deployActive()
    {
        active.GetComponent<EventCluster>().startDispense();
    }

    GameObject generate(int difRate)      //I don't think we need a coroutine for thise - at least not yet
    {
        VehicleFactoryManager.vehicleTypes vtype = VehicleFactoryManager.vehicleTypes._null; //need to assign for debugging
        eventTypes etype;
        Event _nE;
        List<Event> _new = new List<Event>();
        GameObject newEC = Instantiate(eCluster);
        int clusterSize = 3 + difRate;
        List<Transform> sPoints = new List<Transform>();
        int randNum;////////////
        for (int i = 0; i < clusterSize; i++)
        {
            Debug.Log("creating event " + i);
            //determine etype - temporary
            randNum = UnityEngine.Random.Range(1,6);
            if(randNum % 5 == 0){
                etype = EventManager.eventTypes.obstacle;
            }else{
                etype = EventManager.eventTypes.vehicle;
            }
            Debug.Log(etype);
            //------------------end temp
            if (etype == EventManager.eventTypes.vehicle)
            {
                //determine vehicle type --- need to implement, for now just does medium and light randomly
                randNum = UnityEngine.Random.Range(1,3);
                if(randNum == 3){
                    vtype = VehicleFactoryManager.vehicleTypes.medium;
                }else{
                    vtype = VehicleFactoryManager.vehicleTypes.light;
                }
                sPoints = vspawnPoints;
            }
            else if (etype == EventManager.eventTypes.obstacle)
            {
                vtype = VehicleFactoryManager.vehicleTypes._null;
                sPoints = ospawnPoints;
            }
            Debug.Log(vtype);
            _nE = newEC.AddComponent<Event>() as Event;
            _nE.initialize(difRate, vtype, etype, sPoints);
            _new.Add(_nE);          //uses current dif rate, [for now] default spawn position, [for now] default enemy to create an event
        }
        newEC.GetComponent<EventCluster>().startUp(_new, vFactory);
        return newEC;
    }

    //saved from previous manager
    //------------------------------------------------------------------
    // control game difficulty rating
    IEnumerator difficultyManager()
    {
        while (true)
        {
            difficultyRating = calculateDifficultyRating();
            Debug.Log(difficultyRating);

            yield return new WaitForSecondsRealtime(TimeBetweenDifficultyAdjustment);
        }
    }

    // Use difficulty equation to calculate event difficulty rating based on current time
    private int calculateDifficultyRating()
    {
        float timeMinutes = GameManager.GameManagerInstance.getGameTime() / 60;
        double calculatedDifficulty;

        System.Random rand = new System.Random();
        double randomModifier = (rand.NextDouble() * (randomModifierMax - randomModifierMin)) + randomModifierMin;

        // Equation to calculate difficulty rating. Has base linear slope modified by a sin function to give peaks and valleys
        // to difficulty
        // diff = diffSlope*x + sin(frequencyModifier*x) + baseDifficulty
        calculatedDifficulty = ((difficultySlopeModifier * timeMinutes) + (sinAmplitudeModifier * Math.Sin(sinFrequencyModifier * timeMinutes)) + baseDifficultyRating);

        // add modifier to calculated difficulty +/- some percent
        calculatedDifficulty += calculatedDifficulty * randomModifier;

        // return rating rounded to nearest whole number
        return Convert.ToInt32(calculatedDifficulty);
    }
}
