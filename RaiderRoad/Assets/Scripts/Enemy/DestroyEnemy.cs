using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : AbstractEnemyAI {
    //enemy, speed
    private GameObject cObject;
    private float speed = 2f;

    public void StartDestroy(GameObject enemy)
    {
        cObject = enemy;
    }

    public void Destroy()
    {
        //Set wall gameobject
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        //Set movement speed of enemy
        float movement = speed * Time.deltaTime;

        //If there are no more walls, go to Escape state, else keep going for walls
        if (walls.Length <= 0)
        {
            cObject.GetComponent<EnemyAI>().Invoke("EnterEscape", 5f);
            //cObject.GetComponent<EnemyAI>().EnterEscape();
        }else
        {
            GameObject wall = Closest(cObject.transform.position, walls);
            cObject.transform.LookAt(wall.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, wall.transform.position, movement);
        }

        //cObject.GetComponent<EnemyAI>().Invoke("EnterEscape", 10f);
    }
}
