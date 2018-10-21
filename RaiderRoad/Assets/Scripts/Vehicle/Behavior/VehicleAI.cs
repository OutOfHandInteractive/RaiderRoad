using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleAI : MonoBehaviour {
    //States
    public enum State { Wander, Chase, Attack, Leave };

    //State Classes
    private WanderVehicle wander;
    private ChaseVehicle chase;
    private AttackVehicle attack;
    private LeaveVehicle leave;

    //Current object and navmesh
    protected NavMeshAgent agent;
    public State currentState;
    private GameObject enemy;

    private string side;

	//Statistics
	private float maxHealth;
	private float ramDamage;
	private float speed;

	// Use this for initialization
	void Start () {

        //Initialize all the classes
        enemy = gameObject;
        agent = GetComponent<NavMeshAgent>();
        wander = new WanderVehicle();
        chase = new ChaseVehicle();
        attack = new AttackVehicle();
        leave = new LeaveVehicle();
        transform.position = GameObject.Find("Spawn").transform.position;

        int action = Random.Range(0, 100);
        if (action < 50)
        {
            side = "left";
        }
        else
        {
            side = "right";
        }

        //Start wander state
        wander.StartWander(agent, enemy, side);
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(currentState);
        switch (currentState)
        {
            case State.Wander:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    wander.Wander();
                break;
            case State.Chase:
                chase.StartChase(agent, enemy);
                chase.Chase(side);
                break;
            case State.Attack:
                attack.StartAttack(agent, enemy, side);
                attack.Attack();
                break;
            case State.Leave:
                leave.StartLeave(agent);
                leave.Leave();
                break;
        }
    }

    //Used to change state from different classes
    public void EnterChase()
    {
        currentState = State.Chase;
    }
    public void EnterAttack()
    {
        currentState = State.Attack;
    }
    public void EnterLeave()
    {
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

    public string getSide()
    {
        return side;
    }
    public State getState()
    {
        return currentState;
    }
}
