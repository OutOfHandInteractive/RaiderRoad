using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StealEnemy : EnemyAI {


    //enemy, speed
    private GameObject cObject;
    public bool hasStolen = false;
    private GameObject[] drops;
    private GameObject drop;
    public void StartSteal(GameObject enemy)
    {
        cObject = enemy;
        drops = GameObject.FindGameObjectsWithTag("Drops");
        drop = Closest(cObject.transform.position, drops);
    }

    public void Steal()
    {
        //Set wall gameobject
        //Set movement speed of enemy
        float movement = speed * Time.deltaTime;

        if (cObject.GetComponent<StatefulEnemyAI>().getDamaged())
        {
            cObject.GetComponent<StatefulEnemyAI>().EnterFight();
        }

        //If there are no more drops, go to Escape state, else keep going for drops
        if (hasStolen && cObject.transform.GetComponentInChildren<ItemDrop>())
        {
            movement /= 2;
            cObject.GetComponent<StatefulEnemyAI>().EnterEscape();
        }
        else
        {
            //Find wall and go to it
            if(drop != null)
            {
                cObject.transform.LookAt(drop.transform);
                cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, drop.transform.position, movement);
            }
            else
            {
                movement /= 2;
                cObject.GetComponent<StatefulEnemyAI>().EnterEscape();
            }

        }
    }
}
