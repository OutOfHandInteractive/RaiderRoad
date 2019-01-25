﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatefulEnemyAI : EnemyAI {
    //States
    public enum State { Wait, Board, Weapon, Steal, Destroy, Fight, Escape, Death, Lure };
    //State Classes
    private WaitEnemy wait;
    private BoardEnemy board;
    private WeaponAttackEnemy weapon;
    private DestroyEnemy destroy;
    private StealEnemy steal;
    private FightEnemy fight;
    private EscapeEnemy escape;
    private DeathEnemy death;
    private LureEnemy lure;

    //Enemy variables
    private GameObject enemy;
    private State currentState;
    private Rigidbody rb;
    private GameObject parent;
    private Vector3 scale;
    private bool damaged;

    //Vehicle variables
    private VehicleAI vehicle;
    private string side;
    public GameObject munnitions;
    public GameObject fire;
    private GameObject interactable;
    public GameObject dropOnDeath;

    // statistics
    public float maxHealth;
    public float damagePower;
    public float currentHealth;
    private bool inRange;

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
        inRange = false;
        scale = transform.localScale;
        damaged = false;

        enemy = gameObject;
        rb = GetComponent<Rigidbody>();
        wait = enemy.AddComponent<WaitEnemy>();
        board = enemy.AddComponent<BoardEnemy>();
        weapon = enemy.AddComponent<WeaponAttackEnemy>();
        destroy = enemy.AddComponent<DestroyEnemy>();
        steal = enemy.AddComponent<StealEnemy>();
        fight = enemy.AddComponent<FightEnemy>();
        escape = enemy.AddComponent<EscapeEnemy>();
        death = enemy.AddComponent<DeathEnemy>();
        lure = enemy.AddComponent<LureEnemy>();

        //Get vehicle information, side
        vehicle = gameObject.GetComponentInParent<VehicleAI>();
        Debug.Log(vehicle.getSide());
        side = vehicle.getSide();
        parent = transform.parent.gameObject;
        if(vehicle.GetComponentInChildren<HasWeapon>() != null)
        {
            interactable = vehicle.GetComponentInChildren<HasWeapon>().gameObject;
        }
        else
        {
            interactable = null;
        }

        Debug.Log(interactable);


        EnterWait();

    }

    public GameObject GetEnemyObject()
    {
        return enemy;
    }
    
    // Update is called once per frame
    void Update () {
        //Debug.Log(currentState);
        //Go to weapon state when vehicle is ramming

        Debug.Log(interactable);
        Debug.Log(transform.parent);
        if (interactable && !interactable.transform.GetComponentInChildren<EnemyAI>())
        {
            gameObject.tag = "usingWeapon";
            transform.SetParent(interactable.transform, true);
            transform.position = interactable.transform.position;
            //transform.rotation = Quaternion.identity;
            transform.localScale = scale;
            weapon.StartWeapon(enemy, vehicle, munnitions, fire, side);
            GameObject weapons = weapon.getWeapon();
            weapon.LookAtPlayer(weapons);
        }
        if (transform.parent != null && transform.parent.name == "EnemyInt" && vehicle.getState() == VehicleAI.State.Chase)
        {
            EnterWeapon();

        }
        else
        {
            switch (currentState)
            {
                case State.Wait:
                    wait.Wait();
                    break;
                case State.Weapon:
                    weapon.Weapon();
                    break;
                case State.Board:
                    board.Board();
                    break;
                case State.Destroy:
                    destroy.Destroy();
                    break;
                case State.Steal:
                    steal.Steal();
                    break;
                case State.Fight:
                    fight.Fight();
                    break;
                case State.Escape:
                    escape.Escape();
                    break;
                case State.Death:
                    death.Death(enemy, dropOnDeath);
                    break;
                case State.Lure:
                    lure.Lure();
                    break;
            }
        }

    }

    public void takeDamage(float damage) {
        Debug.Log(currentHealth);
        currentHealth -= damage;
        damaged = true;

        if (currentHealth <= 0) {
            EnterDeath();
        }
    }

    //Methods to enter states, change color based on states
    public void EnterStateIfNotAlready(State state)
    {
        if(currentState != state)
        {
            EnterState(state);
        }
    }
    public void EnterState(State state)
    {
        switch (state)
        {
            case State.Wait:
                EnterWait();
                break;
            case State.Board:
                EnterBoard();
                break;
            case State.Weapon:
                EnterWeapon();
                break;
            case State.Steal:
                EnterSteal();
                break;
            case State.Destroy:
                EnterDestroy();
                break;
            case State.Fight:
                EnterFight();
                break;
            case State.Escape:
                EnterEscape();
                break;
            case State.Death:
                EnterDeath();
                break;
            case State.Lure:
                EnterLure();
                break;
        }
    }
    public void EnterWait()
    {
        currentState = State.Wait;
        wait.StartWait(enemy, vehicle);
        enemy.GetComponent<Renderer>().material.color = Color.white;
        
    }
    public void EnterBoard()
    {
        currentState = State.Board;
        board.StartJump(enemy, rb, side);
        enemy.GetComponent<Renderer>().material.color = Color.green;
    }

    public void EnterWeapon()
    {
        currentState = State.Weapon;
        enemy.GetComponent<Renderer>().material.color = Color.gray;
    }

    public void EnterSteal()
    {
        currentState = State.Steal;
        steal.StartSteal(enemy);
        enemy.GetComponent<Renderer>().material.color = Color.magenta;
    }

    public void EnterDestroy()
    {
        currentState = State.Destroy;
        destroy.StartDestroy(enemy);
        enemy.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void EnterFight()
    {
        currentState = State.Fight;
        fight.StartFight(enemy);
        enemy.GetComponent<Renderer>().material.color = Color.red;
    }

    public void EnterEscape()
    {
        currentState = State.Escape;
        escape.StartJump(enemy, rb, side);
        enemy.GetComponent<Renderer>().material.color = Color.blue;
    }

    public void EnterDeath()
    {
        currentState = State.Death;
    }

    public void EnterLure()
    {
        State prev = currentState;
        currentState = State.Lure;
        lure.StartLure(prev);
        enemy.GetComponent<Renderer>().material.color = Color.cyan;
    }

    public void Stunned()
    {
        
    }

    /*public void Damage(float damage)
    {
        currentHealth -= damage;
    }*/

    //Collison handling
    private void OnCollisionEnter(Collision collision)
    {
        //Die if enemy touches road
        if (collision.gameObject.tag == "road" /*currentState != State.Wait*/)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Change transform to stay on vehicles
        if (collision.gameObject.tag == "eVehicle")
        {
            transform.parent = parent.transform;
        }
        if (collision.gameObject.tag == "RV")
        {
            transform.parent = collision.transform.root;
            //currentState = State.Destroy;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Change parent when not on vehicles
        if (collision.gameObject.tag == "eVehicle")
        {
            transform.parent = null;
        }
        if (collision.gameObject.tag == "RV")
        {
            transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if you hit the player and do action
        if (other.gameObject.tag == "Player" && currentState == State.Fight)
        {   
            vehicle.StartCoroutine(WindUp(other));
        }
        if (other.gameObject.tag == "EnemyInteract" && currentState == State.Wait)
        {
            transform.parent = other.transform;
        }
    }
    IEnumerator WindUp(Collider other)
    {
        Debug.Log(Time.time);
        fight.WindupAttack();
        yield return new WaitForSeconds(.5f);
        if(inRange)
        {
            fight.HitPlayer(other);
        }
        else
        {
            fight.Missed();
        }
        Debug.Log(Time.time);
    }
    private void OnTriggerStay(Collider other)
    {
        //Check if you hit a wall and destroy it
        if (other.gameObject.tag == "Player" && currentState == State.Fight)
        {
            inRange = true;
        }
        if (other.gameObject.tag == "Wall" && currentState == State.Destroy)
        {
            //Debug.Log("HIT");
            other.gameObject.GetComponent<Wall>().Damage(25f);
        }
        if (other.gameObject.tag == "Engine" && currentState == State.Destroy)
        {
            other.gameObject.GetComponent<Engine>().Damage(25f);
            if(other.gameObject.GetComponent<Engine>().health <= 0)
            {
                destroy.engineKill = true;
            }
        }
        if (other.gameObject.tag == "Drops" && currentState == State.Steal)
        {
            //Debug.Log("HIT");
            Destroy(other.gameObject);
            steal.hasStolen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && currentState == State.Fight)
        {
            inRange = false;
        }
    }
    // -------------------- Getters and Setters ----------------------
    public State GetState()
    {
        return currentState;
    }

    public void SetState(State _currentState)
    {
        currentState = _currentState;
    }
 
    public float getMaxHealth() {
        return maxHealth;
    }

    public float getHealth() {
        return currentHealth;
    }

    public bool getDamaged()
    {
        return damaged;
    }
}
