using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Util 
{
    public static bool isEnemy(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "Enemy";
    }

    public static bool isPlayer(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "Player";
    }

    public static bool IsRV(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "RV";
    }

    public static bool IsVehicle(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == "eVehicle";
    }

    public static bool IsVehicleRecursive(GameObject gameObject)
    {
        return gameObject != null && (IsVehicle(gameObject) || IsVehicleRecursive(Parent(gameObject)));
    }

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

    public static Vector3 Copy(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y);
    }

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

    public static void RemoveAll<T>(ICollection<T> list, T remove)
    {
        while (list.Remove(remove)) { }
    }
}
