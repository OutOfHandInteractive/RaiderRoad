using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Super class for all Raider AI classes. Mostly just contains useful utility methods for basic things like movement or choosing nearby objects
/// </summary>
public abstract class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// Enemy movement speed
    /// </summary>
    protected float speed = 2f;

    /// <summary>
    /// Gets the object from the list that is closest to the given position
    /// </summary>
    /// <param name="myPos">The postition to search around</param>
    /// <param name="objects">THe list of objects to look through</param>
    /// <returns>The object closest to the given point</returns>
    public GameObject Closest(Vector3 myPos, IEnumerable<GameObject> objects) {
        float minDist = 1 / 0f;
        GameObject closest = null;
        foreach (GameObject obj in objects) {
            float dist = Vector3.Distance(obj.transform.position, myPos);
            if (closest == null || dist < minDist)
            {
                closest = obj;
                minDist = dist;
            }
        }
        return closest;
    }

    /// <summary>
    /// Gets the object in the array closest to the enemy's current position
    /// </summary>
    /// <param name="objects">The objects to look through</param>
    /// <returns>The object closest to the enemy's position</returns>
    public GameObject Closest(IEnumerable<GameObject> objects)
    {
        return Closest(gameObject.transform.position, objects);
    }

    /// <summary>
    /// Gets the closest living player
    /// </summary>
    /// <returns>The closest living player</returns>
    public GameObject ClosestLivingPlayer()
    {
        return Closest(from obj in GameObject.FindGameObjectsWithTag(Constants.PLAYER_TAG) where Util.IsAlive(obj) select obj);
    }

    /// <summary>
    /// Checks if this radier is on an enemy vehicle
    /// </summary>
    /// <returns>True IFF this raider is on a vehicle</returns>
    public bool OnVehicle()
    {
        return Util.IsVehicleRecursive(gameObject);
    }

    /// <summary>
    /// Point and move the enemy towards the given object
    /// </summary>
    /// <param name="target">The object to move towards</param>
    public void MoveToward(GameObject target)
    {
        MoveToward(target.transform);
        //Mark - Y? Y U DEW DIS? (why have a function that just calls another function)
    }

    /// <summary>
    /// Point and move the enemy towrds the given transform
    /// </summary>
    /// <param name="target">THe transform to move towards</param>
    public void MoveToward(Transform target)
    {
        MoveToward(target.position);
    }

    public void MoveToward(Vector3 target)
    {
        float movement = speed * Time.deltaTime;
        gameObject.transform.LookAt(target);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, movement);
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
