using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnemy : EnemyAI {

    //Enemy and enemy speed
    private GameObject cObject;
    private GameObject _target;
    private float speed = 2f;

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
        //If player exists
        if (player != null)
        {
            //Look at player and move towards them
            cObject.transform.LookAt(player.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, player.transform.position, movement);
        }

        //Cancel enterboard invoke and go to enterescape after 5 seconds
        cObject.GetComponent<StatefulEnemyAI>().Invoke("EnterEscape", 5f);
    }
}
