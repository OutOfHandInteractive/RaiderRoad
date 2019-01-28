using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : EnemyAI {
    //enemy, speed
    private GameObject cObject;
    private int action;
    public bool engineKill = false;
    public void StartDestroy(GameObject enemy)
    {
        cObject = enemy;
        action = Random.Range(0, 100);
    }

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

    public void ChanceDestroy(GameObject[] walls, GameObject[] engines, float movement)
    {
        if(action < 90)
        {
            GameObject wall = Closest(cObject.transform.position, walls);
            if(walls.Length <= 0)
            {
                cObject.GetComponent<StatefulEnemyAI>().EnterFight();
            }
            MoveToward(wall);
        }
        else
        {
            GameObject engine = Closest(cObject.transform.position, engines);
            MoveToward(engine);
        }
    }
}
