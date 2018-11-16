using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnemy : EnemyAI {

    //Enemy and enemy speed
    private GameObject cObject;
    private GameObject _target;
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
        

        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("eVehicle");
        float movement = speed * Time.deltaTime;
        //If doesnt exist or if player has been hit go into escape state
        if (player != null && playerHit > 1)
        {
            cObject.GetComponent<StatefulEnemyAI>().EnterEscape();
        }
        else
        {
            //Look at player and move towards them
            cObject.transform.LookAt(player.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, player.transform.position, movement);

        }
        //Check if player is hit
        if (player != null && Vector3.Distance(cObject.transform.position, player.transform.position) < 1f)
        {
            //Debug.Log("PlayerHit");
            playerHit++;
        }
    }
}
