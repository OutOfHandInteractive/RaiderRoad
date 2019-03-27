using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Util
{
    public static bool IsWall(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == Constants.WALL_TAG;
    }

    public static bool IsEngine(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == Constants.ENGINE_TAG;
    }

    public static bool IsRoad(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == Constants.ROAD_TAG;
    }

    public static bool IsEnemy(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == Constants.ENEMY_TAG;
    }

    public static bool IsPlayer(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == Constants.PLAYER_TAG;
    }

    public static bool IsAlive(GameObject player)
    {
        PlayerController_Rewired pc = player.GetComponent<PlayerController_Rewired>();
        return pc != null && pc.state == PlayerController_Rewired.playerStates.up;
    }

    public static bool IsRV(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == Constants.RV_TAG;
    }

    public static bool IsVehicle(GameObject gameObject)
    {
        return gameObject != null && gameObject.tag == Constants.VEHICLE_TAG;
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

    public static void AssertNotNull(string message, Object obj)
    {
        if(obj == null)
        {
            Debug.LogAssertion(message);
        }
    }
}
