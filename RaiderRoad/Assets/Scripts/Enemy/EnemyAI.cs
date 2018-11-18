using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
    //States
    public enum State { Wait, Board, Weapon, Steal, Destroy, Fight, Escape, Death };

    //State Classes
    private WaitEnemy wait;
    private BoardEnemy board;
    private WeaponAttackEnemy weapon;
    private DestroyEnemy destroy;
    private FightEnemy fight;
    private EscapeEnemy escape;
    private DeathEnemy death;

    //Enemy variables
    private GameObject enemy;
    private State currentState;
    private Rigidbody rb;
    private GameObject parent;

    //Vehicle variables
    private VehicleAI vehicle;
    private string side;
    public GameObject munnitions;
    public GameObject fire;
    private GameObject interactable;

	// statistics
	public float maxHealth;
	private float currentHealth;

    // Use this for initialization
    void Start () {
		currentHealth = maxHealth;

        enemy = gameObject;
        rb = GetComponent<Rigidbody>();
        wait = new WaitEnemy();
        board = new BoardEnemy();
        weapon = new WeaponAttackEnemy();
        destroy = new DestroyEnemy();
        fight = new FightEnemy();
        escape = new EscapeEnemy();
        death = new DeathEnemy();

        //Get vehicle information, side
        vehicle = gameObject.GetComponentInParent<VehicleAI>();
        Debug.Log(vehicle.getSide());
        side = vehicle.getSide();
        parent = transform.parent.gameObject;
        interactable = vehicle.GetComponentInChildren<HasWeapon>().gameObject;
        Debug.Log(interactable);


        EnterWait();

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(currentState);
        //Go to weapon state when vehicle is ramming

        Debug.Log(interactable);
        Debug.Log(transform.parent);
        if (!interactable.transform.GetComponentInChildren<EnemyAI>())
        {
            transform.parent = interactable.transform;
            transform.position = interactable.transform.position;
            transform.rotation = Quaternion.identity;
        }
        if (transform.parent.name == "EnemyInt" && vehicle.getState() == VehicleAI.State.Chase)
        {
            EnterWeapon();

        }
        else
        {
            switch (currentState)
            {
                case State.Wait:
                    wait.StartWait(enemy,vehicle);
                    wait.Wait();
                    break;
                case State.Weapon:
                    weapon.StartWeapon(enemy, vehicle, munnitions, fire);
                    weapon.Weapon();
                    break;
                case State.Board:
                    board.StartJump(enemy, rb, side);
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
                    escape.StartJump(enemy, rb, side);
                    escape.Escape();
                    break;
                case State.Death:
                    death.Death(enemy);
                    break;
            }
        }


    }

	public void takeDamage(float damage) {
        Debug.Log(currentHealth);
		currentHealth -= damage;

		if (currentHealth <= 0) {
			EnterDeath();
		}
	}

    //Methods to enter states, change color based on states
    public void EnterWait()
    {
        currentState = State.Wait;
        enemy.GetComponent<Renderer>().material.color = Color.white;
        
    }
    public void EnterBoard()
    {
        currentState = State.Board;
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
    }

    public void EnterDestroy()
    {
        currentState = State.Destroy;
        enemy.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void EnterFight()
    {
        currentState = State.Fight;
        enemy.GetComponent<Renderer>().material.color = Color.red;
    }

    public void EnterEscape()
    {
        currentState = State.Escape;
        enemy.GetComponent<Renderer>().material.color = Color.blue;
    }

    public void EnterDeath()
    {
        currentState = State.Death;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
    }

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
        if (collision.gameObject.tag == "floor")
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
        if (collision.gameObject.tag == "floor")
        {
            transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if you hit the player and do action
        if(other.gameObject.tag == "Player" && currentState == State.Fight)
        {
            Debug.Log("Hit");
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up*100, ForceMode.Impulse);
        }
        if(other.gameObject.tag == "EnemyInteract" && currentState == State.Wait)
        {
            transform.parent = other.transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Check if you hit a wall and destroy it
        if (other.gameObject.tag == "Wall" && currentState == State.Destroy)
        {
            //Debug.Log("HIT");
            other.gameObject.GetComponent<Wall>().Damage(25f);
        }
    }

    public State GetState()
    {
        return currentState;
    }

    public void SetState(State _currentState)
    {
        currentState = _currentState;
    }
 
}
