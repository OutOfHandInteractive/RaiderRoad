using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Util 
{
    public static bool isEnemy(GameObject gameObject)
    {
        return gameObject.tag == "Enemy";
    }

    public static bool isPlayer(GameObject gameObject)
    {
        return gameObject.tag == "Player";
    }

    public static Vector3 Copy(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y);
    }

    public static void RemoveNulls<T>(IList<T> list)
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
}
