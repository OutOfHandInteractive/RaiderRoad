using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnemy : EnemyAI {

    //Enemy and enemy speed
    private GameObject cObject;
    private GameObject _target;
    private float speed = 2f;

    private int playerHit = 0;
    public void StartFight(GameObject enemy, GameObject target = null)
    {
        //Initialized enemy
        cObject = enemy;
        _target = target;
    }

    private GameObject GetTarget()
    {
        if(_target == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            _target = Closest(cObject.transform.position, players);
        }
        return _target;
    }

    public void Fight()
    {
        //Get player object
        GameObject player = GetTarget();
        //Get enemy speed
        float movement = speed * Time.deltaTime;

        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("eVehicle");

        //If doesnt exist or if player has been hit go into escape state
        if (player != null || playerHit > 1)
        {

            //Find vehicle to escape to
            GameObject vehicle = Closest(cObject.transform.position, vehicles);
            if (vehicle == null)
                cObject.GetComponent<StatefulEnemyAI>().EnterFight();
            cObject.transform.LookAt(vehicle.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, vehicle.transform.position, movement);
            //If a reasonable jumping distance to vehicle, escape
            if (Vector3.Distance(cObject.transform.position, vehicle.transform.position) < 5f)
                cObject.GetComponent<StatefulEnemyAI>().EnterEscape();

            //Check if player is hit
            if (Vector3.Distance(cObject.transform.position, player.transform.position) < 1f)
            {
                //Debug.Log("PlayerHit");
                playerHit++;
            }
        }
        else
        {
            //Look at player and move towards them
            cObject.transform.LookAt(player.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, player.transform.position, movement);

        }
    }
}
