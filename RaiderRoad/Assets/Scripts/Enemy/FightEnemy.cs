using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnemy : EnemyAI {

    //Enemy and enemy speed
    private GameObject cObject;
    private GameObject fightRange;
    private float speed = 2f;
    private float playerDamage = 0;
    private bool chasing = true;
    private float damagePower = 2f;
    private float knockback_force = 2000f;
    private GameObject _target;
    private int playerHit = 0;
    public void StartFight(GameObject enemy, GameObject target = null)
    {
        //Initialized enemy
        cObject = enemy;
        _target = target;
        fightRange = cObject.transform.Find("EnemyAttack").gameObject;
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
        if (!player || playerDamage >= 4f || cObject.GetComponent<StatefulEnemyAI>().currentHealth <= 25f)
        {
            cObject.GetComponent<StatefulEnemyAI>().EnterEscape();
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