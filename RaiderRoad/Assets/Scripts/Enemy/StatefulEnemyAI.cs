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

    private EnemyAIState currState;

    //Enemy variables
    protected NavMeshAgent agent;
    public NavMeshAgent Agent {
        get {
            return agent;
        }
    }
    private GameObject enemy;
    [SerializeField]
    private State currentState;
    private Rigidbody rb;
    public Rigidbody Rb
    {
        get
        {
            return rb;
        }
    }
    private GameObject parent;
    private Vector3 scale;
    private bool damaged;

    //Vehicle variables
    private VehicleAI vehicle;
    public VehicleAI Vehicle
    {
        get
        {
            return vehicle;
        }
    }
    private VehicleAI.Side side;
    public VehicleAI.Side Side
    {
        get
        {
            return side;
        }
    }
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

    //Animation
    public Animator myAni;
    private EnemyAnimator enemyAnimator = new EnemyAnimator();

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
        inRange = false;
        scale = transform.localScale;
        damaged = false;

        enemy = gameObject;
        agent = GetComponent<NavMeshAgent>();
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

        enemyAnimator.animator = myAni;

        Debug.Log(interactable);
        Debug.Log(transform.parent);
        if (interactable && !interactable.GetComponent<HasWeapon>().enemyUsing)
        {
            gameObject.tag = "usingWeapon";
            interactable.GetComponent<HasWeapon>().enemyUsing = true;
        }
        if (gameObject.tag == "usingWeapon")
        {
            //transform.SetParent(interactable.transform, true);
            //transform.rotation = Quaternion.identity;
            //transform.localScale = scale;
            //transform.localScale = transform.localScale;
            EnterWeapon();
        }
        else
        {
            EnterWait();
        }
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
        else if (GetState() != State.Weapon && Util.IsVehicle(transform.root.gameObject) && transform.root.GetComponentInChildren<PlayerController_Rewired>())
        {
            EnterFight();
        }
        currState.UpdateState();

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

    private EnemyAIState GetByState(State state)
    {
        switch (state)
        {
            case State.Wait: return wait;
            case State.Board: return board;
            case State.Weapon: return weapon;
            case State.Steal: return steal;
            case State.Destroy: return destroy;
            case State.Fight: return fight;
            case State.Escape: return escape;
            case State.Death: return death;
            case State.Lure: return lure;
            case State.Stunned: return stun;
        }
        return null;
    }

    public bool IsCurrent(EnemyAIState state)
    {
        return currState == state;
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
            case State.Wait: EnterWait(); break;
            case State.Board: EnterBoard(); break;
            case State.Weapon: EnterWeapon(); break;
            case State.Steal: EnterSteal(); break;
            case State.Destroy: EnterDestroy(); break;
            case State.Fight: EnterFight(); break;
            case State.Escape: EnterEscape(); break;
            case State.Death: EnterDeath(); break;
            case State.Lure: EnterLure(); break;
            case State.Stunned: EnterStun(); break;
        }
    }

    private StateContext LeaveContext(State state)
    {
        switch (state)
        {
            //TODO: Add cases as necessary for different states
            default: return null;
        }
    }

    private void Enter(EnemyAIState state, StateContext enterContext=null) 
    {
        if(currState != null)
        {
            currState.LeaveState(LeaveContext(currentState));
        }
        currState = state;
        currentState = state.State();
        state.EnterState(this, enterContext);
        enemy.GetComponent<Renderer>().material.color = state.StateColor();
    }

    /// <summary>
    /// Enter the wait state
    /// </summary>
    public void EnterWait()
    {
        Enter(wait);
    }

    /// <summary>
    /// Enter the board state
    /// </summary>
    public void EnterBoard()
    {
        Enter(board);
    }

    /// <summary>
    /// Enter the wepaon state
    /// </summary>
    public void EnterWeapon()
    {
        Enter(weapon);
    }

    /// <summary>
    /// Enter the steal state
    /// </summary>
    public void EnterSteal()
    {
        Enter(steal);
    }

    /// <summary>
    /// Enter the destroy state
    /// </summary>
    public void EnterDestroy()
    {
        Enter(destroy);
    }

    /// <summary>
    /// Enter the fight state
    /// </summary>
    public void EnterFight()
    {
        Enter(fight);
    }

    /// <summary>
    /// Enter the escape state
    /// </summary>
    public void EnterEscape()
    {
        Enter(escape);
    }

    /// <summary>
    /// Enter the death state
    /// </summary>
    public void EnterDeath()
    {
        Enter(death);
    }

    /// <summary>
    /// Enter the lure state
    /// </summary>
    public void EnterLure()
    {
        Enter(lure, new LureEnemy.LureContext(currentState));
    }

    /// <summary>
    /// Enter the stun state
    /// </summary>
    public void EnterStun()
    {
        Enter(stun);
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
        if (Util.IsRoad(collision.gameObject) /*currentState != State.Wait*/)
        {
            EnterDeath();
        }
        //Change transform to stay on vehicles
        if (currentState != State.Weapon && Util.IsVehicleRecursive(collision.gameObject))
        {
            transform.parent = collision.gameObject.transform;
            getAnimator().Grounded = true;
        }
        /*if (collision.gameObject.tag == "RV")
        {
            transform.parent = collision.transform.root;
            agent.enabled = true;
            //currentState = State.Destroy;
        }*/
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {
        //Change parent when not on vehicles
        if (Util.IsVehicle(collision.gameObject))
        {
            transform.parent = null;
        }
        /*if (collision.gameObject.tag == "RV")
        {
            //agent.enabled = false;
            transform.parent = null;
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Util.IsRV(other.gameObject))
        {
            transform.parent = other.gameObject.transform;
            agent.enabled = true;
            getAnimator().Grounded = true;
        }
        currState.TriggerEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        currState.TriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currState.TriggerExit(other);
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

    public EnemyAnimator getAnimator()
    {
        return enemyAnimator;
    }
}
