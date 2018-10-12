using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleAI : MonoBehaviour {
    //States
    private enum State { Wander, Chase, Attack, Leave };

    //State Classes
    private WanderVehicle wander;
    private ChaseVehicle chase;
    private AttackVehicle attack;
    private LeaveVehicle leave;

    //Current object and navmesh
    protected NavMeshAgent agent;
    private State currentState;
    private GameObject enemy;

	// Use this for initialization
	void Start () {

        //Initialize all the classes
        enemy = this.gameObject;
        agent = GetComponent<NavMeshAgent>();
        wander = new WanderVehicle();
        chase = new ChaseVehicle();
        attack = new AttackVehicle();
        leave = new LeaveVehicle();
        transform.position = GameObject.Find("Spawn").transform.position;

        //Start wander state
        wander.StartWander(agent, enemy);
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
                chase.Chase();
                break;
            case State.Attack:
                attack.StartAttack(agent, enemy);
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
}
