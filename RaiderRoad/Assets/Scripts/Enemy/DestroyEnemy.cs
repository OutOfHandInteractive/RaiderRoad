using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour {
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
        GameObject wall = GameObject.FindGameObjectWithTag("wall");
        //Set movement speed of enemy
        float movement = speed * Time.deltaTime;

        //If there are no more walls, go to Escape state, else keep going for walls
        if(!wall)
        {
            cObject.GetComponent<EnemyAI>().EnterEscape();
        }
        else if (wall)
        {
            cObject.transform.LookAt(wall.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, wall.transform.position, movement);
        }

        //cObject.GetComponent<EnemyAI>().Invoke("EnterEscape", 10f);
    }
}
