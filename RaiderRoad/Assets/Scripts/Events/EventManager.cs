using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {


    //public enum EventTypes { vehicle, obstacle, fork };
    //public enum vehicleTypes { light, medium, heavy };

    public float TimeBetweenEvents;
    public float TimeBetweenDifficultyAdjustment;
    private int difficultyRating = 1;       //set to 1 for testing
    public VehicleFactoryManager vFactory; //= gameObject.AddComponent<VehicleFactoryManager>() as VehicleFactoryManager;


    // Equation coefficients
    public float expectedGameLengthModifier;
    public float sinFrequencyModifier;
    public float sinAmplitudeModifier;
    public float difficultySlopeModifier;
    public float baseDifficultyRating;

    // Variation coefficients
    public float randomModifierMin;
    public float randomModifierMax;

    // event generation constants
    public int minEventDifficulty;
    public int maxEventDifficulty;
    [SerializeField]
    private GameObject eCluster;
    [SerializeField]
    private GameObject onDeck;
    [SerializeField]
    private GameObject active;

    void Start(){
        //StartCoroutine(difficultyManager());
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
        VehicleFactoryManager.vehicleTypes type = VehicleFactoryManager.vehicleTypes.light;              //for now using light, but this will be changed based on other factors
        Event _nE;
        List<Event> _new = new List<Event>();
        GameObject newEC = Instantiate(eCluster);
        for (int i = 0; i < difRate; i++)
        {
            Debug.Log("creating event " + i);
            //type becomes something different
            _nE = newEC.AddComponent<Event>() as Event;
            _nE.initialize(difRate, type);
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
