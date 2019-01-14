using UnityEngine;
using System.Collections;
using System;

public abstract class EnemyAI : MonoBehaviour
{
    protected float speed = 2f;
    public GameObject Closest(Vector3 myPos, GameObject[] objects)
    {
        float minDist = 1 / 0f;
        GameObject closest = null;
        foreach (GameObject wall in objects)
        {
            float dist = Vector3.Distance(wall.transform.position, myPos);
            if (closest == null || dist < minDist /*|| deadPlayer != PlayerController_Rewired.playerStates.down*/)
            {
                closest = wall;
                minDist = dist;
            }
        }
        return closest;
    }

    /// <summary>
    /// This Method is for checking if an object TRULY IS null. Not Unity's stupid "fake null" that it looks for with it's overloaded == operator.
    /// </summary>
    /// <param name="obj">Object to test</param>
    /// <returns>true iff the object is really null</returns>
    public static bool IsNull(object obj)
    {
        try
        {
            obj.ToString();
            return false;
        }catch(NullReferenceException e)
        {
            return true;
        }
    }
}
