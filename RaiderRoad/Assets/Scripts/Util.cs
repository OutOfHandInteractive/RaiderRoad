using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    /// Tests if an object is a vehicle or is parented to one
    /// </summary>
    /// <param name="gameObject">The object to test</param>
    /// <returns>True IFF the object is a vehicle or is parented to one</returns>
    public static bool IsVehicleRecursive(GameObject gameObject)
    {
        return GetVehicleRecursive(gameObject) != null;
    }

    public static GameObject GetVehicleRecursive(GameObject gameObject)
    {
        if(gameObject == null || IsVehicle(gameObject))
        {
            return gameObject;
        }
        return GetVehicleRecursive(Parent(gameObject));
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
    /// Builds a string representation of the full path leading to the given object
    /// </summary>
    /// <param name="obj">An object</param>
    /// <returns>A string representation of the full path leading to the given object</returns>
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
    /// <param name="gameObject">The child</param>
    /// <returns>The parent of that child or null if it has none or if the object is null itself</returns>
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
    /// Copies the given vector
    /// </summary>
    /// <param name="vector">The vector to copy</param>
    /// <returns>A copy of the given vector</returns>
    public static Vector3 Copy(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    /// <summary>
    /// Remove nulls from the given list. Plays around with unity's "fake null"
    /// </summary>
    /// <typeparam name="T">The type of the objects</typeparam>
    /// <param name="list">The list to filter down</param>
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
    /// <param name="list">The list</param>
    /// <param name="remove">The object to remove</param>
    public static void RemoveAll<T>(ICollection<T> list, T remove)
    {
        while (list.Remove(remove)) { }
    }
}
