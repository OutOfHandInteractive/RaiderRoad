using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnemy : AbstractEnemyAI {

    //Enemy and enemy speed
    private GameObject cObject;
    private GameObject fightRange;
    private float speed = 2f;
    private float playerDamage = 0;
    private bool chasing = true;
    private float damagePower = 2f;
    private float knockback_force = 2000f;
    public void StartFight(GameObject enemy)
    {
        //Initialized enemy
        cObject = enemy;
        fightRange = cObject.transform.Find("EnemyAttack").gameObject;
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
        if (!player || playerDamage >= 4f || cObject.GetComponent<EnemyAI>().currentHealth <= 25f)
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
        else if(chasing)
        {
            //Look at player and move towards them
            cObject.transform.LookAt(player.transform);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, player.transform.position, movement);

        }


    }

    public void WindupAttack()
    {
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, .5f);
        chasing = false;
        //cObject.transform.position = Vector3.zero;
    }
    public void HitPlayer(Collider other)
    {
        playerDamage += damagePower;
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 0f, 0f, .5f);
        other.gameObject.GetComponent<PlayerController_Rewired>().takeDamage(damagePower);
        Vector3 dir = other.transform.position - cObject.transform.position;
        dir = Vector3.Normalize(new Vector3(dir.x, 0.0f, dir.z));
        other.GetComponent<Rigidbody>().AddForce(dir * knockback_force);
        chasing = true;
    }
    public void Missed()
    {
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, 0f);
        chasing = true;
    }
}