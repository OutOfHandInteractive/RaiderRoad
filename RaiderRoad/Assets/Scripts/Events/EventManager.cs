using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Control spawning of events / enemies in-game
/// </summary>
public class EventManager : MonoBehaviour {
	// --------------------- public variables -------------------------
	// enumerations
	public enum eventTypes { vehicle, obstacle };

	// references
	public VehicleFactoryManager vFactory;

	// gameplay values
	public float TimeBetweenDifficultyAdjustment;

	// -------------------- nonpublic variables ------------------------
	// Static references
	GameManager g;

	// Difficulty
	[Header("Basic Difficulty Information")]
	[SerializeField] private int difficultyRating;
	[SerializeField] private List<float> difficultyMultiplier;

	// Equation coefficients                    -------all set to 1 for now
	[Header("Difficulty Equation Coefficients")]
	[SerializeField] private float expectedGameLengthModifier;
	[SerializeField] private float sinFrequencyModifier;
	[SerializeField] private float sinAmplitudeModifier;
	[SerializeField] private float difficultySlopeModifier;
	[SerializeField] private float baseDifficultyRating;

	// Variation coefficients
	[Header("Other Coefficients")]
	[SerializeField] private float randomModifierMin;
	[SerializeField] private float randomModifierMax;
	[SerializeField] private int minEventDifficulty;
	[SerializeField] private int maxEventDifficulty;
    
    //cluster objects - prefab, currently active, and next ready
	[Header("Cluster Information")]
    [SerializeField] private GameObject eCluster;
    [SerializeField] private GameObject onDeck;
    [SerializeField] private GameObject active;
    
    //spawn points for events
	[Header("Spawn Points")]
    [SerializeField]
    private List<Transform> vspawnPoints;

    //threat ranges
    private int lThreatMin = 1;
    private int lThreatMax = 4;
    private int mThreatMin = 4;
    private int mThreatMax = 8;
    private int hThreatMin = 8;
    private int hThreatMax = 12;
    private int oThreat = 1;

    //coroutine wait time
    private float waitToStart = 20f;        //for the below coroutines, this has it so calculations don't being until events start spawning
    //spawn delay bounds
    private float delayLower = 2f;  //will not be lower
    [SerializeField] private float curDelay = 12f;     //upperBound of 15 - will not exceed this
    private float betweenDelayAdjust = 18f;  //delay will be adjusted every 20 seconds
    private float sFactor = 0.85f;        //to start, decrementing to 85% every 20 seconds will hit the lower bound at approximately 4 minutes in
    //weapon frequency variables
    private float wDelay = 15f; //time between weapon frequency adjustments
    private float weaponRate = 0.1f; //minimum chance to get a weapon on a vehicle - updates over time from event manager [increased from 0]
    private float weaponRateUpper = 0.5f;   //max weapon chance - 50/50
    private float wFactor = 0.05f;         //rate of increase in weapon frequency every update

	#region System Functions
    /// <summary>
    /// Starts all the coroutines, finds all the spawn points for vehicles, and gets a reference to the game manager
    /// </summary>
    void Start(){
		g = GameManager.GameManagerInstance;

        //StartCoroutine(difficultyManager());
        vspawnPoints = new List<Transform>();
        foreach (Transform child in transform)      //get vehicle spawn points
        {
            //Debug.Log(child);
            vspawnPoints.Add(child);
        }
        
		StartCoroutine(difficultyManager());
		StartCoroutine(initialize());                   //initializes first cluster
        StartCoroutine(reduceSpawnTime());            //start cycle of spawn delay reduction
        StartCoroutine(weaponFrequency());             //start cycle of weapon frequency increase
    }
	#endregion

    /// <summary>
    /// Decreases the time between events being spawned in the clusters at a certain interval
    /// </summary>
	IEnumerator reduceSpawnTime()       //over time, reduce interval between spawns (passed to event clusters when created)
    {
        yield return new WaitForSeconds(waitToStart);        //wait allocated time
        while(true){
            if (curDelay*sFactor > delayLower){      //if next iteration is greater than lower bound...
                curDelay = curDelay * sFactor;       //...decrement
            }
            else{
                curDelay = delayLower;              //else, reduce to lower bound
            }
            yield return new WaitForSeconds(betweenDelayAdjust);        //wait allocated time
        }
    }

    /// <summary>
    /// Increases the chance of a vehicle having a weapon at a certain interval
    /// </summary>
    //weapon frequency coroutine
    IEnumerator weaponFrequency()
    {
        yield return new WaitForSeconds(waitToStart);        //wait allocated time
        while (true)
        {
            if (weaponRate + wFactor > weaponRateUpper)
            {      //if next iteration is less than upper bound...
                weaponRate = weaponRate + wFactor;       //...increment
            }
            else
            {
                weaponRate = weaponRateUpper;              //else, keep at upper bound
            }
            yield return new WaitForSeconds(wDelay);        //wait allocated time
        }
    }

    /// <summary>
    /// Generates an initial cluster, then waits to activate it
    /// </summary>
    IEnumerator initialize()
    {
        onDeck = generate(difficultyRating);                //create event cluster at starting difficulty and set as on-deck
        yield return new WaitForSecondsRealtime(10);       //delay for some short time - let's say 10 seconds
        lastDone();                                     //switches on-deck to active, deploys it, and creates new on-deck cluster
    }

<<<<<<< HEAD
    /// <summary>
    /// Sets the on-deck cluster to active, activates it, and generates a new on-deck cluster
    /// </summary>
    //called from last cluster generated once it reaches certain threshold - deploys next cluster and generates a new one on deck
    public void lastDone()
    {
=======
	/// <summary>
	/// Called from last cluster generated after threshold reached
	/// Deploys next event cluster and creates a new one to hold onto
	/// </summary>
    public void lastDone() {
>>>>>>> 896b375549ceadb54bfc45e1b09a380abbaf5558
        active = onDeck;
        deployActive();               //deploys 'active' cluster
        onDeck = generate(difficultyRating);
    }

    /// <summary>
    /// "Turns on" the active cluster so it starts spawning its events
    /// </summary>
    void deployActive()
    {
        active.GetComponent<EventCluster>().startDispense();
    }

    /// <summary>
    /// Creates a new event cluster and all the events within it
    /// </summary>
    /// /// /// <param name="difRate">The difficulty rating for the new cluster</param>
    GameObject generate(int difRate)      //I don't think we need a coroutine for thise - at least not yet
    {
        VehicleFactoryManager.vehicleTypes vtype = VehicleFactoryManager.vehicleTypes._null; //need to assign for debugging
        eventTypes etype;
        Event _nE;
        List<Event> _new = new List<Event>();
        GameObject newEC = Instantiate(eCluster);

		int difficultySpace = difRate;
        int vThreat = 0;  //this is used down below when subtracting the constants for each vtype from the difficultySpace
        int vMod = 0;       //this is used down below when generating a number in the threat ranges above based on type
        List<Transform> sPoints = new List<Transform>();
        int randNum;////////////
        while (difficultySpace > 0)
        {
            //Debug.Log("creating event ");
            //determine etype - temporary
            //randNum = UnityEngine.Random.Range(1,10);
            //if(randNum % 7 == 0){	// Im assuming this is a percentage - can we get it put in constants to avoid magic numbers?
            //    etype = EventManager.eventTypes.obstacle;
            //}else{
				etype = EventManager.eventTypes.vehicle;
            //}
            //s.Log(etype);
            //------------------end temp
            
            if (etype == EventManager.eventTypes.vehicle)
            {
				//determine vehicle type
                if(difficultyRating >= Constants.HEAVY_VEHICLE_BASE_THREAT){
                    randNum = UnityEngine.Random.Range((int)VehicleFactoryManager.vehicleTypes.medium, (int)VehicleFactoryManager.vehicleTypes.heavy + 1);
                }
                else if (difficultyRating >= Constants.MEDIUM_VEHICLE_BASE_THREAT) {
					randNum = UnityEngine.Random.Range((int)VehicleFactoryManager.vehicleTypes.light, (int)VehicleFactoryManager.vehicleTypes.medium + 1);
				}
				else {
					randNum = UnityEngine.Random.Range((int)VehicleFactoryManager.vehicleTypes.light, (int)VehicleFactoryManager.vehicleTypes.light + 1);
				}
				vtype = (VehicleFactoryManager.vehicleTypes)randNum;
                //--------------------------------------------------------------------------------------------------------
                if(vtype == VehicleFactoryManager.vehicleTypes.light){
                    vThreat = Constants.LIGHT_VEHICLE_BASE_THREAT;
                    vMod = UnityEngine.Random.Range(lThreatMin, lThreatMax);
                }
                else if(vtype == VehicleFactoryManager.vehicleTypes.medium){
                    vThreat = Constants.MEDIUM_VEHICLE_BASE_THREAT;
                    vMod = UnityEngine.Random.Range(mThreatMin, mThreatMax);
                }
                else if(vtype == VehicleFactoryManager.vehicleTypes.heavy){   //this is for heavies when we get those in
                    vThreat = Constants.HEAVY_VEHICLE_BASE_THREAT;
                    vMod = UnityEngine.Random.Range(hThreatMin, hThreatMax);
                }
                //---------------------------------------------------------------------------------------------------------
                difficultySpace = difficultySpace - vThreat;  //subtract threat from difSpace
                sPoints = vspawnPoints;
            }
            /*else if (etype == EventManager.eventTypes.obstacle)
            {
                vtype = VehicleFactoryManager.vehicleTypes._null;
                sPoints = ospawnPoints;

				difficultySpace -= Constants.SMALL_OBSTACLE_BASE_THREAT;
            }*/
            //Debug.Log(vtype);
            
            _nE = newEC.AddComponent<Event>() as Event;
            _nE.initialize(difRate, vtype, etype, sPoints);
            if (etype == EventManager.eventTypes.vehicle){      //pass the vMod value to the event only if event is a vehicle
                _nE.setMod(vMod);
            }
            _new.Add(_nE);          //uses current dif rate, [for now] default spawn position, [for now] default enemy to create an event
        }
        newEC.GetComponent<EventCluster>().startUp(_new, vFactory, curDelay, weaponRate);
        return newEC;
    }

    /// <summary>
<<<<<<< HEAD
    /// Recalculates difficulty at given interval
    /// </summary>
    //saved from previous manager
    //------------------------------------------------------------------
    // control game difficulty rating
    IEnumerator difficultyManager()
    {
        while (true)
        {
=======
	/// Continuously update difficulty rating over time
	/// </summary>
	/// <returns>Current difficulty rating</returns>
    IEnumerator difficultyManager() {
        while (true) {
>>>>>>> 896b375549ceadb54bfc45e1b09a380abbaf5558
            difficultyRating = (int)Mathf.Ceil(calculateDifficultyRating() * difficultyMultiplier[g.GetPlayerCount()-1]);

            yield return new WaitForSecondsRealtime(TimeBetweenDifficultyAdjustment);
        }
    }
<<<<<<< HEAD
    /// <summary>
    /// Calculates the difficulty
    /// </summary>
    // Use difficulty equation to calculate event difficulty rating based on current time
    private int calculateDifficultyRating()
    {
=======

	/// <summary>
	/// Use difficulty equation to calculate event difficulty rating based on current time
	/// </summary>
	/// <returns></returns>
	private int calculateDifficultyRating() {
>>>>>>> 896b375549ceadb54bfc45e1b09a380abbaf5558
        float timeMinutes = GameManager.GameManagerInstance.GetGameTime() / 60;
        double calculatedDifficulty;

        System.Random rand = new System.Random();
        double randomModifier = (rand.NextDouble() * (randomModifierMax - randomModifierMin)) + randomModifierMin;	// this is kinda broke
        
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
