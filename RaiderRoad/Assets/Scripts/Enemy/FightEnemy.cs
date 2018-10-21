using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnemy : MonoBehaviour {

    //Enemy and enemy speed
    private GameObject cObject;
    private float speed = 2f;
    public void StartFight(GameObject enemy)
    {
        //Initialized enemy
        cObject = enemy;
    }

    public void Fight()
    {
        //Get player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //Get enemy speed
        float movement = speed * Time.deltaTime;

        //If player exists
        if (player)
        {
            //Look at player and move towards them
            cObject.transform.LookAt(player.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, player.transform.position, movement);
        }

        //Cancel enterboard invoke and go to enterescape after 5 seconds
        cObject.GetComponent<EnemyAI>().CancelInvoke("EnterBoard");
        cObject.GetComponent<EnemyAI>().Invoke("EnterEscape", 5f);
    }
}
