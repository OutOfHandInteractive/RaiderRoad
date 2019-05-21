using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

/// <summary>
/// Static utility class
/// </summary>
public class Util 
{
    /// <summary>
    /// Tests if an object is an enemy
    /// </summary>
    /// <param name="gameObject">The object to test</param>
    /// <returns>True IFF the object is an enemy</returns>
    public static bool isEnemy(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "Enemy";
    }

    /// <summary>
    /// Tests if an object is a player
    /// </summary>
    /// <param name="gameObject">The object to test</param>
    /// <returns>True IFF the object is a player</returns>
    public static bool isPlayer(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == Constants.PLAYER_TAG;
    }

    /// <summary>
    /// Tests if an object is the RV
    /// </summary>
    /// <param name="gameObject">The object to test</param>
    /// <returns>True IFF the object is the RV</returns>
    public static bool IsRV(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "RV";
    }

    /// <summary>
    /// Tests if an object is a vehicle
    /// </summary>
    /// <param name="gameObject">The object to test</param>
    /// <returns>True IFF the object is a vehicle</returns>
    public static bool IsVehicle(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "eVehicle";
    }

    /// <summary>
    /// Tests if an object is a vehicle or is parented to a vehicle
    /// </summary>
    /// <param name="gameObject">The object to test</param>
    /// <returns>True IFF the the object is a vehicle or is parented to one</returns>
    public static bool IsVehicleRecursive(GameObject gameObject)
    {
        return gameObject != null && (IsVehicle(gameObject) || IsVehicleRecursive(Parent(gameObject)));
    }

    /// <summary>
    /// Tests if an object is a wall
    /// </summary>
    /// <param name="gameObject">The object to test</param>
    /// <returns>True IFF the object is a wall</returns>
    public static bool IsWall(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "Wall";
    }

    /// <summary>
    /// Tests if an object is a weapon
    /// </summary>
    /// <param name="gameObject">The object to test</param>
    /// <returns>True IFF the object is a weapon</returns>
    public static bool IsWeapon(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "weapon";
    }

    /// <summary>
    /// Builds a string that represents the full path name of an object, from the root object
    /// </summary>
    /// <param name="obj">The object to build the path to</param>
    /// <returns>A string representing the full path of the object from root</returns>
    public static string FullObjectPath(GameObject obj)
    {
        GameObject parent = Parent(obj);
        if(parent == null)
        {
            return obj.name;
        }
        else
        {
            return FullObjectPath(parent) + '/' + obj.name;
        }
    }

    /// <summary>
    /// Gets the parent of an object in a null-safe way
    /// </summary>
    /// <param name="gameObject">The child object</param>
    /// <returns>The parent of that object or null if it has no parent or if the object is null itself</returns>
    public static GameObject Parent(GameObject gameObject)
    {
        if(gameObject == null)
        {
            return null;
        }
        Transform parent = gameObject.transform.parent;
        if(parent == null)
        {
            return null;
        }
        return parent.gameObject;
    }

    /// <summary>
    /// Creates a copy of the given vector
    /// </summary>
    /// <param name="vector">The vector to copy</param>
    /// <returns>A copy of that vector</returns>
    public static Vector3 Copy(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    public static bool IsAlive(GameObject obj)
    {
        return isPlayer(obj) && obj.GetComponent<PlayerController_Rewired>().getState() == PlayerController_Rewired.playerStates.up;
    }

    /// <summary>
    /// Removes all nulls from the given list. Plays some stupid games to check Unity's "fake null"
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list</typeparam>
    /// <param name="list">The list to remove nulls from</param>
    public static void RemoveNulls<T>(ICollection<T> list) where T : Object
    {
        List<T> drop = new List<T>();
        foreach(T obj in list){
            if(obj == null)
            {
                drop.Add(obj);
            }
        }
        foreach(T obj in drop)
        {
            list.Remove(obj);
        }
    }

    /// <summary>
    /// Removes all copies of the given object from the given list
    /// </summary>
    /// <typeparam name="T">The type of objects in the list</typeparam>
    /// <param name="list">The list to remove from</param>
    /// <param name="remove">The object to remove</param>
    public static void RemoveAll<T>(ICollection<T> list, T remove)
    {
        while (list.Remove(remove)) { }
    }
}
