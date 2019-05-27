using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for vehicles in the wait state
/// </summary>
public class WaitVehicle : MonoBehaviour
{
    private GameObject cObject;
    private bool done = false;

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="enemy">The enemy</param>
    public void StartWait(GameObject enemy)
    {
        cObject = enemy;
    }

    /// <summary>
    /// Do the wait action
    /// </summary>
    public void Wait()
    {
        if(!done)
        {
            StartCoroutine(loadEnemies(2f));
        }
    }
    private IEnumerator loadEnemies(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cObject.GetComponent<VehicleAI>().EnterWander();
        done = true;
    }
}
