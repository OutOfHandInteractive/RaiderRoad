using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnemy : AbstractEnemyAI {

    //Enemy and enemy speed
    private GameObject cObject;
    private float speed = 2f;
    private int playerHit = 0;
    public void StartFight(GameObject enemy)
    {
        //Initialized enemy
        cObject = enemy;
    }

    public void Fight()
    {
        //Get player object
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);

        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("eVehicle");
        //Get enemy speed
        float movement = speed * Time.deltaTime;

        //If doesnt exist or if player has been hit go into escape state
        if (!player || playerHit > 1)
        {

            //Find vehicle to escape to
            GameObject vehicle = Closest(cObject.transform.position, vehicles);
            if (vehicle == null)
                cObject.GetComponent<EnemyAI>().EnterFight();
            cObject.transform.LookAt(vehicle.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, vehicle.transform.position, movement);
            //If a reasonable jumping distance to vehicle, escape
            if (Vector3.Distance(cObject.transform.position, vehicle.transform.position) < 5f)
                cObject.GetComponent<EnemyAI>().EnterEscape();
        }
        else
        {
            //Look at player and move towards them
            cObject.transform.LookAt(player.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, player.transform.position, movement);

        }

        //Check if player is hit
        if (Vector3.Distance(cObject.transform.position, player.transform.position) < 1f)
        {
            //Debug.Log("PlayerHit");
            playerHit++;
        }

    }
}
