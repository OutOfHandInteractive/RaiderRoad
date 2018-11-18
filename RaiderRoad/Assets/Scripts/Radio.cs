using UnityEngine;
using System.Collections.Generic;
using System;

public class Radio
{
    private static Radio instance = new Radio();

    public static Radio GetRadio()
    {
        return instance;
    }

    private Radio() { }

    /*
     * Evacuation of Mooks onto escape vehicles
     */
    private static List<StayVehicle> evacVehicles = new List<StayVehicle>();
    private static Queue<EscapeEnemy> mooksForEvac = new Queue<EscapeEnemy>();
    public void CallForEvac(EscapeEnemy mook)
    {
        mooksForEvac.Enqueue(mook);
        CheckForEvac();
    }

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

    public void EvacLeaving(StayVehicle vehicle)
    {
        Debug.Log("Leaving");
        evacVehicles.Remove(vehicle);
        // No check
    }

    private void CheckForEvac()
    {
        //evacVehicles.RemoveAll(delegate (StayVehicle v) { return v == null; });
        while(mooksForEvac.Count>0 && evacVehicles.Count > 0)
        {
            //TODO: Make this smarter
            EscapeEnemy mook = mooksForEvac.Dequeue();
            StayVehicle vehicle = evacVehicles[0];
            //Debug.Assert(vehicle != null);
            //Debug.Log("Found vehicle: " +vehicle);
            mook.RadioEvacCallback(vehicle);
        }
    }
}
