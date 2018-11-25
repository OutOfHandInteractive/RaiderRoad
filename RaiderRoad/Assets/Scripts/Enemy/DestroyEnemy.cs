﻿using System.Collections;
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
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("eVehicle");
        //Set movement speed of enemy
        float movement = speed * Time.deltaTime;

        if(cObject.GetComponent<EnemyAI>().getDamaged())
        {
            cObject.GetComponent<EnemyAI>().EnterFight();
        }

        //If there are no more walls, go to Fight state, else keep going for walls
        if (walls.Length <= 0 && cObject.transform.parent != null)
        {
            /*GameObject vehicle = Closest(cObject.transform.position, vehicles);
            cObject.transform.LookAt(vehicle.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, vehicle.transform.position, movement);
            if (Vector3.Distance(cObject.transform.position, vehicle.transform.position) < 5f || vehicle == null)
                cObject.GetComponent<EnemyAI>().EnterEscape();*/
            cObject.GetComponent<EnemyAI>().EnterFight();
            //cObject.GetComponent<EnemyAI>().EnterEscape();
        }else
        {
            //Find wall and go to it
            GameObject wall = Closest(cObject.transform.position, walls);
            cObject.transform.LookAt(wall.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, wall.transform.position, movement);
        }
    }
}
