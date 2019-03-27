﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is for enemies that want to seek out and beat up players.
/// </summary>
public class FightEnemy : TargetedEnemy {

    public struct FightContext : StateContext
    {
        public GameObject target; 
    }

    //Enemy and enemy speed
    private GameObject fightRange;
    private float playerDamage = 0;
    private bool chasing = true;
    private float knockback_force = 2000f;
    private GameObject _target;
    private int playerHit = 0;
    private GameObject eVehicle;
    private NavMeshAgent agent;
    private HashSet<GameObject> inRange = new HashSet<GameObject>();

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        //Initialized enemy
        agent = master.Agent;
        if(context != null && context is FightContext)
        {
            _target = ((FightContext)context).target;
        }
        else
        {
            _target = null;
        }
        fightRange = gameObject.transform.Find("EnemyAttack").gameObject;
        //eVehicle = stateMachine.Vehicle.gameObject;
    }

    protected override string[] TargetedTags()
    {
        return new string[] { "Player" };
    }

    protected override bool IsValidTarget(GameObject obj)
    {
        return obj != null && Util.IsAlive(obj);
    }

    public override void UpdateState()
    {
        //Get player object
        GameObject player = GetTarget();
        //Get enemy speed

        //If doesnt exist or if player has been hit go into escape state
        if (player == null || playerDamage >= 4f || master.currentHealth <= 25f)
        {
            Debug.Log("reached");
            master.EnterEscape();
        }
        else if (OnVehicle())
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
            MoveToward(targetPosition);
        }
        else if (chasing && OnRV())
        {
            //Look at player and move towards them
            Vector3 targetPosition = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
            gameObject.transform.LookAt(targetPosition);
            agent.SetDestination(targetPosition);
            master.getAnimator().Running = true;
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, movement);

        }
    }

    public override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if (Util.IsPlayer(other.gameObject))
        {
            inRange.Add(other.gameObject);
            if (!isWindingUp)
            {
                StartCoroutine(WindUp());
            }
        }
    }

    public override void TriggerExit(Collider other)
    {
        base.TriggerExit(other);
        if (Util.IsPlayer(other.gameObject))
        {
            inRange.Remove(other.gameObject);
        }
    }

    private bool isWindingUp = false;
    IEnumerator WindUp()
    {
        Debug.Log(Time.time);
        isWindingUp = true;
        try
        {
            WindupAttack();
            //myAni.SetTrigger("WindUp");
            yield return new WaitForSeconds(.5f);
            if (master.IsCurrent(this))
            {
                bool hit = false;
                master.getAnimator().Attack();
                foreach (GameObject other in inRange)
                {
                    HitPlayer(other, master.damagePower);
                    hit = true;
                }
                if (!hit)
                {
                    Missed();
                }
            }
            Debug.Log(Time.time);
        }
        finally
        {
            isWindingUp = false;
        }
    }

    /// <summary>
    /// Show the wind-up
    /// </summary>
    public void WindupAttack()
    {
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, .5f);
        chasing = false;
        //gameObject.transform.position = Vector3.zero;
    }

    /// <summary>
    /// Punch the given player
    /// </summary>
    /// <param name="other">The player to hit</param>
    public void HitPlayer(GameObject other, float damage)
    {
        playerDamage += damage;
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 0f, 0f, .5f);
        other.gameObject.GetComponent<PlayerController_Rewired>().takeDamage(damage);
        Vector3 dir = other.transform.position - gameObject.transform.position;
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

    public override Color StateColor()
    {
        return Color.red;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Fight;
    }
}