using System.Collections;
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

    private string side;

	//Statistics
	public float maxHealth;
	public float ramDamage;
	public float speed;

	public float currentHealth;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;

        //Initialize all the classes
        enemy = gameObject;
        agent = GetComponent<NavMeshAgent>();
        wander = new WanderVehicle();
        chase = new ChaseVehicle();
        stay = new StayVehicle();
        attack = new AttackVehicle();
        leave = new LeaveVehicle();
        rb = GetComponent<Rigidbody>();
        /*int action = Random.Range(0, 100);
        if (action < 50)
        {
            side = "left";
        }
        else
        {
            side = "right";
        }*/
        Debug.Log(side);
        //Start wander state
        EnterWander();
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(currentState);
        switch (currentState)
        {
            case State.Wander:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    wander.Wander();
                break;
            case State.Chase:
                chase.Chase(side);
                break;
            case State.Stay:
                stay.Stay(side);
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

		if (currentHealth <= 0) {
			Destroy(gameObject);
		}
	}

    //Used to change state from different classes
    public void EnterWander()
    {
        wander.StartWander(agent, enemy, side);
        currentState = State.Wander;
    }
    public void EnterChase()
    {
        chase.StartChase(agent, enemy);
        currentState = State.Chase;
    }
    public void EnterStay()
    {
        stay.StartStay(agent, enemy);
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

    private void OnTriggerEnter(Collider other)
    {
        //Destroy this when it goes off screen
        if (other.tag == "Exit")
            Destroy(this.gameObject);

    }

	// ---------- Getters and Setters ----------
	public float getMaxHealth() {
		return maxHealth;
	}

	public void setMaxHealth(float _maxHealth) {
		maxHealth = _maxHealth;
		currentHealth = maxHealth;
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
}
