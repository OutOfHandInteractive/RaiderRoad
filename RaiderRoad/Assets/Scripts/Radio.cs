using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton class for communication between Raiders and Vehicles.
/// </summary>
public class Radio
{
    private static Radio instance = new Radio();

    private static void Reset()
    {
        instance = new Radio();
    }

    /// <summary>
    /// Gets the static Radio instance
    /// </summary>
    /// <returns>The Radio instance</returns>
    public static Radio GetRadio()
    {
        return instance;
    }

    private Radio() {
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private void SceneManager_sceneUnloaded(Scene scene)
    {
        if(scene.name == sceneManagerScript.GAME_SCENE)
        {
            Reset();
        }
    }

    /*
     * Evacuation of Mooks onto escape vehicles
     */
    private List<StayVehicle> evacVehicles = new List<StayVehicle>();
    private Queue<EscapeEnemy> mooksForEvac = new Queue<EscapeEnemy>();

    private List<PlayerController_Rewired> targets = new List<PlayerController_Rewired>();

    private List<GameObject> vehicles = new List<GameObject>();

    /// <summary>
    /// This method is for escaping enemies to call for a vehicle to evacuate him
    /// </summary>
    /// <param name="mook">The enemy looking for evac</param>
    public void CallForEvac(EscapeEnemy mook)
    {
        mooksForEvac.Enqueue(mook);
        CheckForEvac();
    }

    /// <summary>
    /// This method is for vehicles to indicate that they are ready to receive raiders
    /// </summary>
    /// <param name="vehicle">The vehicle making the call</param>
    public void ReadyForEvac(StayVehicle vehicle)
    {
        if(vehicle == null)
        {
            Debug.Log("WTF");
        }
        Debug.Log("Ready for evac: "+vehicle.ToString());
        evacVehicles.Add(vehicle);
        CheckForEvac();
    }

    /// <summary>
    /// This method is for evac vehicles leaving the scene (or dying).
    /// </summary>
    /// <param name="vehicle">The vehicle that is leaving</param>
    public void EvacLeaving(StayVehicle vehicle)
    {
        Debug.Log("Leaving");
        evacVehicles.Remove(vehicle);
        Debug.Log("evacVehicles length: " + evacVehicles.Count);
        // No check
    }

    /// <summary>
    /// Add the player to the list of targets (Possibly Deprecated)
    /// </summary>
    /// <param name="target">The player</param>
    public void CurrentTargets(PlayerController_Rewired target)
    {
        targets.Add(target);
    }

    private void CheckForEvac()
    {
        //evacVehicles.RemoveAll(delegate (StayVehicle v) { return v == null; });
        while(mooksForEvac.Count>0 && evacVehicles.Count > 0)
        {
            StayVehicle vehicle = evacVehicles[0];
            if(vehicle == null)
            {
                Debug.Log("Dropping \"null\" vehicle");
                evacVehicles.RemoveAt(0);
                continue;
            }
            //TODO: Make this smarter
            EscapeEnemy mook = mooksForEvac.Dequeue();
            //Debug.Assert(vehicle != null);
            Debug.Log("Found vehicle: " +vehicle.ToString());
            mook.RadioEvacCallback(vehicle);
        }
    }

    private void CheckPlayers()
    {
        while (targets.Count > 0)
        {
            PlayerController_Rewired players = targets[0];
            if (players == null)
            {
                targets.RemoveAt(0);
            }
        }
    }

    public void AddVehicle(GameObject vehicle)
    {
        vehicles.Add(vehicle);
    }

    public void RemoveVehicle(GameObject vehicle)
    {
        vehicles.Remove(vehicle);
    }

    public bool checkState() { 
		vehicles.RemoveAll(item => item == null);

        foreach (GameObject vehicle in vehicles) {
            if(vehicle.GetComponent<VehicleAI>().getState() == VehicleAI.State.Attack 
				|| vehicle.GetComponent<VehicleAI>().getState() == VehicleAI.State.Stay) {
                return true;
            }
        }
        return false;
    }
}
