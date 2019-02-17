﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleAI : MonoBehaviour {
    //States
    public enum State { Wander, Chase, Stay, Attack, Leave };

    //State Classes
    private WanderVehicle wander;
    private ChaseVehicle chase;
    private StayVehicle stay;
    private AttackVehicle attack;
    private LeaveVehicle leave;

    //Current object and navmesh
    protected NavMeshAgent agent;
    public State currentState;
    private GameObject enemy;
    private Rigidbody rb;
    private int attackPoint;

    private string side;
    private bool hasWeapon;
    public ParticleSystem collision;

    //Statistics
    public float maxHealth;
	[SerializeField] private float ramDamage;
	[SerializeField] private float speed;
	[SerializeField] private float movementChance;
	[SerializeField] private int threat;

	// --------------- private variables --------------------
	// references
	private Attachment front_attachment;

	// gameplay values
	private float currentHealth;
	[SerializeField] private float highDamageThreshold;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
        hasWeapon = false;
        //Initialize all the classes
        enemy = gameObject;
        agent = GetComponent<NavMeshAgent>();
        wander = enemy.AddComponent<WanderVehicle>();
        chase = enemy.AddComponent<ChaseVehicle>();
        stay = enemy.AddComponent<StayVehicle>();
        attack = enemy.AddComponent<AttackVehicle>();
        leave = enemy.AddComponent<LeaveVehicle>();
        rb = GetComponent<Rigidbody>();

		front_attachment = GetComponentInChildren<Attachment>();

        if(GetComponentInChildren<HasWeapon>() != null)
        {
            hasWeapon = true;
        }
        Debug.Log(side);
        //Start wander state
        EnterWander();
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(currentState);
        if (!agent.enabled)
        {
            //Early exit
            return;
        }
        if (currentState == State.Attack)
        {
            agent.speed = 30;
        }
        else
        {
            agent.speed = 15;
        }
        if(transform.GetComponentInChildren<PlayerController_Rewired>())
        {
            EnterWander();
        }
        switch (currentState)
        {
            case State.Wander:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    wander.Wander();
                break;
            case State.Chase:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    chase.Chase(side);
                break;
            case State.Stay:
                stay.Stay();
                break;
            case State.Attack:
                attack.Attack();
                break;
            case State.Leave:
                leave.Leave();
                break;
        }
    }

	// ---------------- Combat Functions ------------------
	public void takeDamage(float damage) {
		currentHealth -= damage;

		Debug.Log("took " + damage + " damage");

		if (currentHealth <= (maxHealth * highDamageThreshold)) {
			startHighDamageSmokeEffects();
		}

		if (currentHealth <= 0) {
            DelayedDeath();
        }
	}

	public void destroyPart() {
		takeDamage(maxHealth * 0.4f);
	}

	#region state change functions
	//Used to change state from different classes
	public void EnterWander()
    {
        wander.StartWander(agent, enemy, side, hasWeapon);
        currentState = State.Wander;
    }
    public void EnterChase()
    {
        chase.StartChase(agent, enemy, side);
        currentState = State.Chase;
    }
    public void EnterStay(int stickPoint)
    {
        stay.StartStay(agent, enemy, side, stickPoint);
        currentState = State.Stay;
    }
    public void EnterAttack()
    {
        attack.StartAttack(agent, enemy, rb, side);
        currentState = State.Attack;
    }
    public void EnterLeave()
    {
        leave.StartLeave(agent);
        currentState = State.Leave;
    }
	#endregion

	private void OnTriggerEnter(Collider other)
    {
        //Destroy this when it goes off screen
        if (other.tag == "Exit")
            Destroy(this.gameObject);
        if (other.gameObject.tag.Equals("Obstacle"))
        {
            Debug.Log("You Hit an Obstacle");
            ParticleSystem explosion = Instantiate(collision, other.gameObject.transform.position, Quaternion.identity, gameObject.transform);
            explosion.gameObject.transform.localScale *= 10;
            //Destroy(other.gameObject);
            transform.parent = other.transform;
            DelayedDeath();
            StartCoroutine(WaitToDie());
        }

    }

	#region Effect Functions
	private void startHighDamageSmokeEffects() {
		front_attachment.startHighDamageSmokeEffects();
	}
	#endregion

	#region death functions
	private void Die()
    {
        foreach (PlayerController_Rewired pc in gameObject.GetComponentsInChildren<PlayerController_Rewired>())
        {
            pc.RoadRash();
            pc.transform.parent = null;
        }
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
    }

    private void DelayedDeath()
    {
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(5);
        Die();
    }
	#endregion

	#region Getters and Setters
	public float getMaxHealth() {
		return maxHealth;
	}

	public void setMaxHealth(float _maxHealth) {
		maxHealth = _maxHealth;
		currentHealth = maxHealth;
	}

	public float getHealth() {
		return currentHealth;
	}

	public float getRamDamage() {
		return ramDamage;
	}

	public void setRamDamage(float _ramDamage) {
		ramDamage = _ramDamage;
	}

	public float getSpeed() {
		return speed;
	}

	public void setSpeed(float _speed) {
		speed = _speed;
	}

	public float getMovementChance() {
		return movementChance;
	}

	public void setMovementChance(float _chance) {
		movementChance = _chance;
	}

    public void setSide(string _side)
    {
        side = _side;
    }

    public string getSide()
    {
        return side;
    }
    public State getState()
    {
        return currentState;
    }
	#endregion
}
