using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State machine for governing Raider AI. Passes updates down to the current state.
/// </summary>
public class StatefulEnemyAI : EnemyAI {
    //States
    /// <summary>
    /// This enum represents the different AI states. Tightly coupled with the state classes.
    /// </summary>
    public enum State { Wait, Board, Weapon, Steal, Destroy, Fight, Escape, Death, Lure, Stunned };
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
    private StunnedEnemy stun;

    //Enemy variables
    private GameObject enemy;
    [SerializeField]
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
    public float damageMeter;
    public int stateChance;
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
        stun = enemy.AddComponent<StunnedEnemy>();

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

    /// <summary>
    /// Gets the enemy object (Deprecated; use .gameObject)
    /// </summary>
    /// <returns>The enemy object</returns>
    public GameObject GetEnemyObject()
    {
        return enemy;
    }
    
    // Update is called once per frame
    void Update () {
        if (currentHealth <= 0)
        {
            EnterDeath();
        }

        if (GetState() != State.Weapon && transform.root.GetComponentInChildren<PlayerController_Rewired>() && transform.root.tag == "eVehicle")
        {
            EnterFight();
        }
        //Debug.Log(currentState);
        //Go to weapon state when vehicle is ramming

        Debug.Log(interactable);
        Debug.Log(transform.parent);
        if (interactable && !interactable.GetComponent<HasWeapon>().enemyUsing)
        {
            gameObject.tag = "usingWeapon";
            interactable.GetComponent<HasWeapon>().enemyUsing = true;
        }
        if(gameObject.tag == "usingWeapon")
        {
            //transform.SetParent(interactable.transform, true);
            //transform.rotation = Quaternion.identity;
            //transform.localScale = scale;
            //transform.localScale = transform.localScale;
            weapon.StartWeapon(enemy, vehicle, munnitions, fire, side);
            GameObject weapons = weapon.getWeapon();
            weapon.LookAtPlayer(weapons);
			transform.position = interactable.transform.position;
        }
        if (gameObject.tag == "usingWeapon" && vehicle.getState() == VehicleAI.State.Chase)
        {
            EnterWeapon();
            weapon.Weapon();

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
                case State.Stunned:
                    stun.StartStun();
                    break;
            }
        }

    }

    /// <summary>
    /// Makes the enemy suffer damage. Damage is subtracted from health. If health drops to 0 or below the enemy will immediately enter death state.
    /// </summary>
    /// <param name="damage">The damage to take</param>
    public void takeDamage(float damage) {
        Debug.Log(currentHealth);
        currentHealth -= damage;
        damaged = true;

        if (currentHealth <= 0) {
            EnterDeath();
        }
    }

    //Methods to enter states, change color based on states
    /// <summary>
    /// Enter the given state if not already in it
    /// </summary>
    /// <param name="state">The state to move to</param>
    public void EnterStateIfNotAlready(State state)
    {
        if(currentState != state)
        {
            EnterState(state);
        }
    }

    /// <summary>
    /// Enter the given state
    /// </summary>
    /// <param name="state">The state to enter</param>
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
            case State.Stunned:
                EnterStun();
                break;
        }
    }

    /// <summary>
    /// Enter the wait state
    /// </summary>
    public void EnterWait()
    {
        currentState = State.Wait;
        wait.StartWait(enemy, vehicle);
        enemy.GetComponent<Renderer>().material.color = Color.white;
        
    }

    /// <summary>
    /// Enter the board state
    /// </summary>
    public void EnterBoard()
    {
        currentState = State.Board;
        board.StartJump(enemy, rb, side, stateChance);
        enemy.GetComponent<Renderer>().material.color = Color.green;
    }

    /// <summary>
    /// Enter the wepaon state
    /// </summary>
    public void EnterWeapon()
    {
        currentState = State.Weapon;
        enemy.GetComponent<Renderer>().material.color = Color.gray;
    }

    /// <summary>
    /// Enter the steal state
    /// </summary>
    public void EnterSteal()
    {
        currentState = State.Steal;
        steal.StartSteal(enemy);
        enemy.GetComponent<Renderer>().material.color = Color.magenta;
    }

    /// <summary>
    /// Enter the destroy state
    /// </summary>
    public void EnterDestroy()
    {
        currentState = State.Destroy;
        destroy.StartDestroy(enemy);
        enemy.GetComponent<Renderer>().material.color = Color.yellow;
    }

    /// <summary>
    /// Enter the fight state
    /// </summary>
    public void EnterFight()
    {
        currentState = State.Fight;
        fight.StartFight(enemy, vehicle);
        enemy.GetComponent<Renderer>().material.color = Color.red;
    }

    /// <summary>
    /// Enter the escape state
    /// </summary>
    public void EnterEscape()
    {
        currentState = State.Escape;
        escape.StartJump(enemy, rb, side, stateChance);
        enemy.GetComponent<Renderer>().material.color = Color.blue;
    }

    /// <summary>
    /// Enter the death state
    /// </summary>
    public void EnterDeath()
    {
        currentState = State.Death;
    }

    /// <summary>
    /// Enter the lure state
    /// </summary>
    public void EnterLure()
    {
        State prev = currentState;
        currentState = State.Lure;
        lure.StartLure(prev);
        enemy.GetComponent<Renderer>().material.color = Color.cyan;
    }

    /// <summary>
    /// Enter the stun state
    /// </summary>
    public void EnterStun()
    {
        currentState = State.Stunned;
        stun.StartStun();
        enemy.GetComponent<Renderer>().material.color = Color.black;
    }

    /// <summary>
    /// Starts a coroutine to stun the enemy for the given amount of time
    /// </summary>
    /// <param name="secs">The stun duration in seconds</param>
    public void Stunned(float secs = 1)
    {
        StartCoroutine(waitStun(secs));
    }

    IEnumerator waitStun(float secs)
    {
        EnterStun();
        yield return new WaitForSeconds(secs);
        EnterFight();
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
        //Change transform to stay on vehicles
        if (collision.gameObject.tag == "eVehicle" && gameObject.tag != "usingWeapon")
        {
            transform.parent = parent.transform;
        }
        if (collision.gameObject.tag == "RV")
        {
            transform.parent = collision.transform.root;
            //currentState = State.Destroy;
        }
    }

    private void OnCollisionStay(Collision collision)
    {

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
            StartCoroutine(WindUp(other));
        }
        if (other.gameObject.tag == "EnemyInteract" && currentState == State.Wait)
        {
            transform.parent = other.transform;
        }
        if (other.gameObject.tag == "Drops" && currentState == State.Steal)
        {
            Debug.Log("HIT" + other.gameObject.name);
            GameObject drop = other.gameObject;
            Destroy(other.gameObject);
            Instantiate(drop, transform);
            //other.transform.position = new Vector3(0, 2, 0);
            steal.hasStolen = true;
        }
    }
    IEnumerator WindUp(Collider other)
    {
        Debug.Log(Time.time);
        fight.WindupAttack();
        yield return new WaitForSeconds(.5f);
        if(inRange)
        {
            fight.HitPlayer(other, damagePower);
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
            damageMeter = damageMeter + (100 * Time.deltaTime);
            if (damageMeter >= 100)
            {
                other.gameObject.GetComponent<Wall>().Damage(100f);
                damageMeter = 0;
            }
        }
        if (other.gameObject.tag == "Engine" && currentState == State.Destroy)
        {
            damageMeter = damageMeter + (100 * Time.deltaTime);
            if (damageMeter >= 100)
            {
                other.gameObject.GetComponent<Engine>().Damage(100f);
                damageMeter = 0;
            }
            if (other.gameObject.GetComponent<Engine>().health <= 0)
            {
                destroy.engineKill = true;
            }
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
