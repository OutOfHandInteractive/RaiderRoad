using UnityEngine;
using System.Collections;

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
}
