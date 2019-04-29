using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitVehicle : MonoBehaviour
{
    private GameObject cObject;
    private bool done = false;
    public void StartWait(GameObject enemy)
    {
        cObject = enemy;
    }

    public void Wait()
    {
        if(!done)
        {
            StartCoroutine(loadEnemies(2f));
        }
    }
    IEnumerator loadEnemies(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cObject.GetComponent<VehicleAI>().EnterWander();
        done = true;
    }
}
