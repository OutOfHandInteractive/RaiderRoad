﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is for enemies that want to seek out and beat up players.
/// </summary>
public class FightEnemy : EnemyAI {

    //Enemy and enemy speed
    private GameObject cObject;
    private GameObject fightRange;
    private float playerDamage = 0;
    private bool chasing = true;
    private float damagePower = 2f;
    private float knockback_force = 2000f;
    private GameObject _target;
    private int playerHit = 0;

    /// <summary>
    /// Initialize this state
    /// </summary>
    /// <param name="enemy">This enemy</param>
    /// <param name="target">The target to attack, if any</param>
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
            //PlayerController_Rewired.playerStates deadPlayer = _target.GetComponent<PlayerController_Rewired>().state;
            //if(deadPlayer != PlayerController_Rewired.playerStates.down)
        }
        return _target;
    }

    /// <summary>
    /// Perform the fight actions
    /// </summary>
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
            Vector3 targetPosition = new Vector3(player.transform.position.x, cObject.transform.position.y, player.transform.position.z);
            cObject.transform.LookAt(targetPosition);
            cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, player.transform.position, movement);

        }
    }

    /// <summary>
    /// Show the wind-up
    /// </summary>
    public void WindupAttack()
    {
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, .5f);
        chasing = false;
        //cObject.transform.position = Vector3.zero;
    }

    /// <summary>
    /// Punch the given player collider
    /// </summary>
    /// <param name="other">The player to hit</param>
    public void HitPlayer(Collider other)
    {
        playerDamage += damagePower;
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 0f, 0f, .5f);
        other.gameObject.GetComponent<PlayerController_Rewired>().takeDamage(damagePower);
        Vector3 dir = other.transform.position - cObject.transform.position;
        dir = Vector3.Normalize(new Vector3(dir.x, 0.0f, dir.z));
        other.GetComponent<Rigidbody>().AddForce(dir * knockback_force);
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, 0f);
        chasing = true;
    }

    /// <summary>
    /// Show the miss
    /// </summary>
    public void Missed()
    {
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, 0f);
        chasing = true;
    }
}