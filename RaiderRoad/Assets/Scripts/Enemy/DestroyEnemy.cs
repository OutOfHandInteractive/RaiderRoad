using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This enemy just wants to destroy things and pick fights
/// </summary>
public class DestroyEnemy : EnemyAI {
    //enemy, speed
    private GameObject cObject;
    private int action;

    /// <summary>
    /// TODO: Ernest please explain this
    /// </summary>
    public bool engineKill = false;

    /// <summary>
    /// Initializes this state
    /// </summary>
    /// <param name="enemy">This enemy (Deprecated)</param>
    public void StartDestroy(GameObject enemy)
    {
        cObject = enemy;
        action = Random.Range(0, 100);
    }

    /// <summary>
    /// Performs the destroy actions
    /// </summary>
    public void Destroy()
    {
        //Set wall gameobject
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] engines = GameObject.FindGameObjectsWithTag("Engine");
        //Set movement speed of enemy
        float movement = speed * Time.deltaTime;

        if(cObject.GetComponent<StatefulEnemyAI>().getDamaged())
        {
            cObject.GetComponent<StatefulEnemyAI>().EnterFight();
        }

        //If there are no more walls, go to Fight state, else keep going for walls
        if (engineKill && cObject.transform.parent != null)
        {
            cObject.GetComponent<StatefulEnemyAI>().EnterFight();
        }else
        {
            //Find destroyable and go to it
            ChanceDestroy(walls, engines, movement);
        }
    }

    /// <summary>
    /// TODO Explain this too, please, Erndog
    /// </summary>
    /// <param name="walls"></param>
    /// <param name="engines"></param>
    /// <param name="movement"></param>
    public void ChanceDestroy(GameObject[] walls, GameObject[] engines, float movement)
    {
        if(action < 90)
        {
            if(walls.Length <= 0)
            {
                cObject.GetComponent<StatefulEnemyAI>().EnterFight();
            }
            else
            {
                GameObject wall = Closest(cObject.transform.position, walls);
                MoveToward(wall);
            }
        }
        else
        {
            GameObject engine = Closest(cObject.transform.position, engines);
            MoveToward(engine);
        }
    }
}
