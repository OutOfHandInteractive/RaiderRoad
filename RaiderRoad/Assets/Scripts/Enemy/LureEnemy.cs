using UnityEngine;
using System.Collections.Generic;
using System;

public class LureEnemy : EnemyAI
{
    private HashSet<GameObject> lures = new HashSet<GameObject>();
    private StatefulEnemyAI.State prev;

    public void AddLure(LureTrap trap)
    {
        lures.Add(trap.gameObject);
    }

    internal void StartLure(StatefulEnemyAI.State prevState)
    {
        prev = prevState;
    }

    internal void Lure()
    {
        foreach(GameObject lure in lures)
        {
            if(lure == null)
            {
                lures.Remove(lure);
            }
        }
        if(lures.Count <= 0)
        {
            gameObject.GetComponent<StatefulEnemyAI>().EnterState(prev);
        }
        else
        {
            GameObject[] gameObjects = new GameObject[lures.Count];
            lures.CopyTo(gameObjects);
            MoveToward(Closest(gameObjects));
        }

    }
}
