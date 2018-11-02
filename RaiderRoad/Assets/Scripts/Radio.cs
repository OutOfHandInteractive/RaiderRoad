using UnityEngine;
using System.Collections.Generic;
using System;

public class Radio : MonoBehaviour
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
    private LinkedList<StayVehicle> evacVehicles = new LinkedList<StayVehicle>();
    private Queue<EscapeEnemy> mooksForEvac = new Queue<EscapeEnemy>();
    public void CallForEvac(EscapeEnemy mook)
    {
        mooksForEvac.Enqueue(mook);
        CheckForEvac();
    }

    public void ReadyForEvac(StayVehicle vehicle)
    {
        evacVehicles.AddLast(vehicle);
        CheckForEvac();
    }

    public void EvacLeaving(StayVehicle vehicle)
    {
        evacVehicles.Remove(vehicle);
        // No check
    }

    private void CheckForEvac()
    {
        while(mooksForEvac.Count>0 && evacVehicles.Count > 0)
        {
            //TODO: Make this smarter
            EscapeEnemy mook = mooksForEvac.Dequeue();
            StayVehicle vehicle = evacVehicles.First.Value;
            mook.RadioEvacCallback(vehicle);
        }
    }
}
