﻿using UnityEngine;
using System.Collections;

public abstract class AbstractEnemyAI : MonoBehaviour
{

    public GameObject Closest(Vector3 myPos, GameObject[] objects)
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
}