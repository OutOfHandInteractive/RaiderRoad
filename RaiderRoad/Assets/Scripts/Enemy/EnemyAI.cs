﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
    //States
    private enum State { Wait, Board, Weapon, Steal, Destroy, Fight, Escape, Death };

    //State Classes
    private WaitEnemy wait;
    private BoardEnemy board;
    private DestroyEnemy destroy;
    private FightEnemy fight;
    private EscapeEnemy escape;

    //Enemy variables
    private GameObject enemy;
    private State currentState;
    private Rigidbody rb;

    //Vehicle variables
    private VehicleAI vehicle;
    private string side;

    // Use this for initialization
    void Start () {
        enemy = gameObject;
        rb = GetComponent<Rigidbody>();
        wait = new WaitEnemy();
        board = new BoardEnemy();
        destroy = new DestroyEnemy();
        fight = new FightEnemy();
        escape = new EscapeEnemy();

        //Get vehicle information, side
        vehicle = gameObject.GetComponentInParent<VehicleAI>();
        side = vehicle.getSide();

        EnterWait();

    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(currentState);
        //Go to weapon state when vehicle is ramming
        if (vehicle.getState() == VehicleAI.State.Attack)
        {
            EnterWeapon();
        }
        else
        {
            switch (currentState)
            {
                case State.Wait:
                    wait.StartWait(enemy);
                    wait.Wait();
                    break;
                case State.Board:
                    board.StartBoard(enemy, rb, side);
                    board.Board();
                    break;
                case State.Destroy:
                    destroy.StartDestroy(enemy);
                    destroy.Destroy();
                    break;
                case State.Fight:
                    fight.StartFight(enemy);
                    fight.Fight();
                    break;
                case State.Escape:
                    escape.StartEscape(enemy, rb, side);
                    escape.Escape();
                    break;
            }
        }


    }

    //Methods to enter states
    public void EnterWait()
    {
        currentState = State.Wait;
    }
    public void EnterBoard()
    {
        currentState = State.Board;
    }

    public void EnterWeapon()
    {
        currentState = State.Weapon;
    }

    public void EnterSteal()
    {
        currentState = State.Steal;
    }

    public void EnterDestroy()
    {
        currentState = State.Destroy;
    }

    public void EnterFight()
    {
        currentState = State.Fight;
    }

    public void EnterEscape()
    {
        currentState = State.Escape;
    }

    public void EnterDeath()
    {
        currentState = State.Death;
    }

    //Collison handling
    private void OnCollisionEnter(Collision collision)
    {
        //Destroy wall if enemy touches it
        if (collision.gameObject.tag == "wall")
        {
            Destroy(collision.gameObject);
        }
        //Die if enemy touches road
        if (collision.gameObject.tag == "road" && currentState != State.Wait)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Change transform to stay on vehicles
        if (collision.gameObject.tag == "eVehicle")
        {
            transform.parent = collision.transform;
        }
        if (collision.gameObject.tag == "RV")
        {
            transform.parent = collision.transform;
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
}