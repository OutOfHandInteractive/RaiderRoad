using UnityEngine;
using System.Collections;
using System;

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
    public GameObject Closest(Vector3 myPos, GameObject[] objects) {
        float minDist = 1 / 0f;
        GameObject closest = null;
        foreach (GameObject wall in objects) {
            float dist = Vector3.Distance(wall.transform.position, myPos);
            if(wall.GetComponent<Wall>() != null)
            {
                if ((closest == null || dist < minDist) && wall.GetComponent<Wall>().isOccupied != true)
                {
                    wall.GetComponent<Wall>().isOccupied = true;
                    closest = wall;
                    minDist = dist;
                }
            }
            else if (wall.GetComponent<Engine>() != null)
            {
                if ((closest == null || dist < minDist) && wall.GetComponent<Engine>().isOccupied != true)
                {
                    wall.GetComponent<Engine>().isOccupied = true;
                    closest = wall;
                    minDist = dist;
                }
            }
            else if (wall.GetComponent<ItemDrop>() != null)
            {
                if ((closest == null || dist < minDist) && wall.GetComponent<ItemDrop>().isOccupied != true)
                {
                    wall.GetComponent<ItemDrop>().isOccupied = true;
                    closest = wall;
                    minDist = dist;
                }
            }
            else if (wall.GetComponent<cannon>() != null)
            {
                if ((closest == null || dist < minDist) && wall.GetComponent<cannon>().isOccupied != true)
                {
                    wall.GetComponent<cannon>().isOccupied = true;
                    closest = wall;
                    minDist = dist;
                }
            }
            else if (wall.GetComponent<flamethrower>() != null)
            {
                if ((closest == null || dist < minDist) && wall.GetComponent<flamethrower>().isOccupied != true)
                {
                    wall.GetComponent<flamethrower>().isOccupied = true;
                    closest = wall;
                    minDist = dist;
                }
            }
            else if (wall.GetComponent<PlayerController_Rewired>() != null && wall.GetComponent<PlayerController_Rewired>().state == PlayerController_Rewired.playerStates.up)
            {
                if ((closest == null || dist < minDist))
                {
                    closest = wall;
                    minDist = dist;
                }
            }
            else if (wall.gameObject.tag == "floor")
            {
                if (closest == null || dist < minDist)
                {
                    closest = wall;
                    minDist = dist;
                }
            }
            else if (wall.gameObject.tag == "JumpL" || wall.gameObject.tag == "JumpR")
            {
                if (closest == null || dist < minDist)
                {
                    closest = wall;
                    minDist = dist;
                }
            }
        }
        return closest;
    }

    /// <summary>
    /// Gets the object in the array closest to the enemy's current position
    /// </summary>
    /// <param name="objects">The objects to look through</param>
    /// <returns>The object closest to the enemy's position</returns>
    public GameObject Closest(GameObject[] objects)
    {
        return Closest(gameObject.transform.position, objects);
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

    public bool OnVehicle()
    {
        return Util.IsVehicleRecursive(gameObject);
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
