using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Super class for all Raider AI classes. Mostly just contains useful utility methods for basic things like movement or choosing nearby objects
/// </summary>
public abstract class EnemyAI : MonoBehaviour
{

    public abstract float Speed();

    /// <summary>
    /// Gets the object from the list that is closest to the given position
    /// </summary>
    /// <param name="myPos">The postition to search around</param>
    /// <param name="objects">THe list of objects to look through</param>
    /// <returns>The object closest to the given point</returns>
    public GameObject Closest(Vector3 myPos, IEnumerable<GameObject> objects)
    {
        float minDist = 1 / 0f;
        GameObject closest = null;
        foreach (GameObject wall in objects)
        {
            float dist = Vector3.Distance(wall.transform.position, myPos);
            if (closest == null || dist < minDist)
            {
                closest = wall;
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

    public GameObject ClosestLivingPlayer()
    {
        return Closest(from player in GameObject.FindGameObjectsWithTag(Constants.PLAYER_TAG) where Util.IsAlive(player) select player);
    }

    /// <summary>
    /// Point and move the enemy towards the given object
    /// </summary>
    /// <param name="target">The object to move towards</param>
    public void MoveToward(GameObject target)
    {
        MoveToward(target.transform);
        //Mark - Y? Y U DEW DIS? (why have a function that just calls another function)
        // It's a convenience function, Mark. It looks cleaner to say MoveToward(someObjective) than MoveToward(someObjective.transform.position) all the time
    }

    /// <summary>
    /// Point and move the enemy towrds the given transform
    /// </summary>
    /// <param name="target">The transform to move towards</param>
    public void MoveToward(Transform target)
    {
        MoveToward(target.position);
    }

    /// <summary>
    ///  Point and move the enemy towards the given transfrom
    /// </summary>
    /// <param name="target">The position to move towards</param>
    public virtual void MoveToward(Vector3 target)
    {
        float movement = Speed() * Time.deltaTime;
        gameObject.transform.LookAt(target);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, movement);
    }
    
    /// <summary>
    /// Checks if the raider is on a vehicle
    /// </summary>
    /// <returns>true iff the raider is currently on a vehicle</returns>
    public bool OnVehicle()
    {
        Transform parent = gameObject.transform.parent;
        return parent != null && Util.IsVehicle(parent.gameObject);
    }

    /// <summary>
    /// Checks if the raider is on the RV
    /// </summary>
    /// <returns>true iff the raider is currently on the RV</returns>
    public bool OnRV()
    {
        Transform parent = gameObject.transform.parent;
        return parent != null && Util.IsRV(parent.gameObject);
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
        }catch(NullReferenceException)
        {
            return true;
        }
    }

    public static bool Unoccupied(GameObject obj)
    {
        EnemyTarget target = obj.GetComponent<EnemyTarget>();
        return target == null || !target.isOccupied;
    }

    public static void Occupy(GameObject obj)
    {
        if(obj != null)
        {
            EnemyTarget target = obj.GetComponent<EnemyTarget>();
            if(target == null)
            {
                target = obj.AddComponent<EnemyTarget>();
            }
            target.isOccupied = true;
        }
    }
}
