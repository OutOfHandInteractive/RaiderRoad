using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    private Collider player;
    protected NavMeshAgent agent;
    private GameObject enemy;
    [SerializeField] private State currentState;
    private Rigidbody rb;
    private GameObject parent;
    private Vector3 scale;
    private bool damaged;
	private bool isDestroying = false;
    private bool isFighting = false;

    //Vehicle variables
	[Header("Vehicle Variables")]
    private VehicleAI vehicle;
    private VehicleAI.Side side;
    public GameObject munnitions;
    public GameObject fire;
    private GameObject interactable;
    public GameObject dropOnDeath;

	// statistics
	[Header("Statistics")]
	public float maxHealth;
    public float damagePower;
    public float currentHealth;
    public float damageMeter;
    public int stateChance;
    private bool inRange;
	[SerializeField] private float wallDestroyTime;
	[SerializeField] private float batteryDestroyTime;

	//UI Elements
	[Header("UI")]
	public GameObject fightIcon;
    public GameObject destroyIcon;
    private bool isBigger = false;
    private bool isSmaller = false;

	//Animation
	[Header("Animation and FX")]
	public Animator myAni;

	// FX
	[SerializeField] private ParticleSystem deathFx;
	[SerializeField] private Transform deathFXSpawnLocation;

	#region System Functions
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

        if(vehicle.GetComponentInChildren<HasWeapon>() != null) {
            interactable = vehicle.GetComponentInChildren<HasWeapon>().gameObject;
        }
        else {
            interactable = null;
        }

        Debug.Log(interactable);

        EnterWait();
    }

	// Update is called once per frame
	void Update() {
        if (currentHealth <= 0) {
			EnterDeath();
		}

		if (GetState() != State.Weapon && transform.root.GetComponentInChildren<PlayerController_Rewired>()
			&& transform.root.tag == "eVehicle") {
			EnterFight();
		}
		//Go to weapon state when vehicle is ramming

		if (interactable && !interactable.GetComponent<HasWeapon>().enemyUsing) {
			gameObject.tag = "usingWeapon";
			interactable.GetComponent<HasWeapon>().enemyUsing = true;
		}

		if (gameObject.tag == "usingWeapon") {
			weapon.StartWeapon(enemy, vehicle, munnitions, fire, side);
			GameObject weapons = weapon.getWeapon();
			weapon.LookAtPlayer(weapons);
			transform.position = interactable.transform.position;
		}

        if(isFighting)
        {
            isFighting = false;
            StartCoroutine(WindUp(player));
        }

		if (gameObject.tag == "usingWeapon" && vehicle.getState() == VehicleAI.State.Chase) {
			EnterWeapon();
			weapon.Weapon();

		}
		else {
			switch (currentState) {
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
				//case State.Death:
					//death.Death(enemy, dropOnDeath, deathFx);
					//break;
				case State.Lure:
					lure.Lure();
					break;
				case State.Stunned:
					stun.StartStun();
					break;
			}
		}
	}
	#endregion

    /// <summary>
    /// Makes the enemy suffer damage. Damage is subtracted from health. If health drops to 0 or below the enemy will immediately enter death state.
    /// </summary>
    /// <param name="damage">The damage to take</param>
    public void takeDamage(float damage) {
        currentHealth -= damage;
        damaged = true;

        if (currentHealth <= 0) {
            EnterDeath();
        }
    }

	#region State Entry Functions
	//Methods to enter states
    /// <summary>
    /// Enter the given state
    /// </summary>
    /// <param name="state">The state to enter</param>
    public void EnterState(State state) {
        switch (state) {
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
    public void EnterWait() {
        currentState = State.Wait;
        wait.StartWait(enemy, vehicle);
        enemy.GetComponent<Renderer>().material.color = Color.white;
        
    }

    /// <summary>
    /// Enter the board state
    /// </summary>
    public void EnterBoard() {
        currentState = State.Board;
        board.StartJump(enemy, rb, side, agent, stateChance, vehicle);
        enemy.GetComponent<Renderer>().material.color = Color.green;
    }

    /// <summary>
    /// Enter the wepaon state
    /// </summary>
    public void EnterWeapon() {
        currentState = State.Weapon;
        enemy.GetComponent<Renderer>().material.color = Color.gray;
    }

    /// <summary>
    /// Enter the steal state
    /// </summary>
    public void EnterSteal() {
        currentState = State.Steal;
        if (GetComponent<lightEnemy>())
        {
            myAni.SetBool("Sneaking", true);
        }
        steal.StartSteal(enemy, destroyIcon);
        enemy.GetComponent<Renderer>().material.color = Color.magenta;
    }

    /// <summary>
    /// Enter the destroy state
    /// </summary>
    public void EnterDestroy() {
        currentState = State.Destroy;
        if(GetComponent<lightEnemy>())
        {
            myAni.SetBool("Sneaking", true);
        }

		destroy.StartDestroy(enemy, agent, destroyIcon);
        enemy.GetComponent<Renderer>().material.color = Color.yellow;
    }

    /// <summary>
    /// Enter the fight state
    /// </summary>
    public void EnterFight() {
        currentState = State.Fight;
        fight.StartFight(enemy, vehicle, agent, fightIcon);
        enemy.GetComponent<Renderer>().material.color = Color.red;
    }

    /// <summary>
    /// Enter the escape state
    /// </summary>
    public void EnterEscape() {
        currentState = State.Escape;
		escape.StartJump(enemy, rb, side, agent, stateChance, vehicle);
        enemy.GetComponent<Renderer>().material.color = Color.blue;
    }

    /// <summary>
    /// Enter the death state
    /// </summary>
    public void EnterDeath() {	// being called every frame, shouldn't be
        currentState = State.Death;
		if (GetComponentInChildren<Canvas>()) {
			GetComponentInChildren<Canvas>().gameObject.SetActive(false);
		}
		GetComponent<NavMeshAgent>().enabled = false;
		myAni.SetBool("Death", true);
    }

	/// <summary>
	/// Call to destroy character and play out death functionality
	/// </summary>
	public void PlayDeath() {
		death.Death(enemy, dropOnDeath, deathFx, deathFXSpawnLocation);
	}

    /// <summary>
    /// Enter the lure state
    /// </summary>
    public void EnterLure() {
        State prev = currentState;
        currentState = State.Lure;
        lure.StartLure(prev);
        enemy.GetComponent<Renderer>().material.color = Color.cyan;
    }

    /// <summary>
    /// Enter the stun state
    /// </summary>
    public void EnterStun() {
        currentState = State.Stunned;
        stun.StartStun();
        enemy.GetComponent<Renderer>().material.color = Color.black;
    }
	#endregion

	#region State Helper Functions
	// ------------------------------ Steal -----------------------------------
	public void ExitStealState(State nextState) {
		if (nextState != State.Destroy) {
			myAni.SetBool("Sneaking", false);
		}
	}

    

	// ----------------------------- Destroy ----------------------------------
	IEnumerator DestroyWall(Collider other) {
		isDestroying = true;
		agent.speed = 0;
        myAni.SetTrigger("StartBreak"); //visual of enemy breaking object

        yield return new WaitForSeconds(myAni.GetCurrentAnimatorStateInfo(0).length);
        increaseIconSize();
        yield return new WaitForSeconds(wallDestroyTime);
		agent.speed = speed;

		if (other) {	// make sure target is still there
			other.gameObject.GetComponent<Wall>().Damage(100f);
		}
		myAni.SetTrigger("InterruptAction");	// this needs to change. need a better way to stop the destroying anim

		if (gameObject.GetComponent<lightEnemy>()) {
			EnterSteal();
		}
        decreaseIconSize();

        isDestroying = false;

		yield return null;
	}

	IEnumerator DestroyBattery(Collider other) {
		isDestroying = true;
        myAni.SetTrigger("StartBreak"); //visual of enemy breaking object
        agent.speed = 0;
        yield return new WaitForSeconds(myAni.GetCurrentAnimatorStateInfo(0).length);
        increaseIconSize();
        yield return new WaitForSeconds(batteryDestroyTime);
        agent.speed = speed;
		if (other) {
			other.gameObject.GetComponent<Engine>().Damage(100f);

			if (other.gameObject.GetComponent<Engine>().health <= 0) {
				destroy.engineKill = true;
			}
		}
        decreaseIconSize();

        isDestroying = false;
		myAni.SetTrigger("InterruptAction");    // this needs to change. need a better way to stop the destroying anim
	}

    private void increaseIconSize()
    {
        RectTransform icon = destroyIcon.GetComponent<RectTransform>();
        float maxHeight = 20;
        float maxWidth = 15;
        float increaseValue = 1f;
        if (icon.rect.height < maxHeight && icon.rect.width < maxWidth && !isBigger)
        {
            destroyIcon.GetComponent<Image>().color = Color.red;
            icon.sizeDelta = new Vector2(icon.rect.height + increaseValue, icon.rect.width + increaseValue);
            destroyIcon.GetComponent<RectTransform>().sizeDelta = icon.sizeDelta;
        }
        else
        {
            isBigger = true;
        }
    }

    private void decreaseIconSize()
    {
        RectTransform icon = destroyIcon.GetComponent<RectTransform>();
        if (icon.rect.height > 0 && icon.rect.width > 0 && !isSmaller)
        {
            destroyIcon.SetActive(false);
        }
        else
        {
            isSmaller = true;
        }
    }

    public void ExitDestroyState() {
		if (isDestroying) {
            agent.speed = speed;
            StopCoroutine("DestroyWall");
			StopCoroutine("DestroyBattery");
			myAni.SetTrigger("InterruptAction");
		}

		myAni.SetBool("Sneaking", false);
	}

	// ------------------------------ Fight -----------------------------------
	/// <summary>
	/// Starts raider wind-up to attack sequence before damage is dealt
	/// </summary>
	/// <param name="other">Collider assigned to the target of the raider's attack</param>
	/// <returns></returns>
	IEnumerator WindUp(Collider other) {
		fight.WindupAttack(other);
		yield return new WaitForSeconds(.5f);
		myAni.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        if(inRange)
        { 
            isFighting = true;
        }
	}

	/// <summary>
	/// Call to deal damage to a player. Requires prior call of IEnumerator Windup() and subsequent attack animation with event trigger.
	/// </summary>
	public void attack() {
		if (inRange) {
			fight.HitPlayer(damagePower);
		}
		else {
			fight.Missed();
		}
	}

	// ------------------------------ Stunned ---------------------------------
	/// <summary>
	/// Starts a coroutine to stun the enemy for the given amount of time
	/// </summary>
	/// <param name="secs">The stun duration in seconds</param>
	public void Stunned(float secs = 1) {
        StartCoroutine(waitStun(secs));
    }

    IEnumerator waitStun(float secs) {
        EnterStun();
        yield return new WaitForSeconds(secs);
        EnterFight();
    }
	#endregion

	#region Collision and Trigger Functions
	//Collison handling
	private void OnCollisionEnter(Collision collision) {
        //Die if enemy touches road
        if (collision.gameObject.tag == "eVehicle" /*currentState != State.Wait*/)
        {
            transform.parent = collision.gameObject.transform.root;
        }
        if (collision.gameObject.tag == "road" /*currentState != State.Wait*/) {
            Destroy(gameObject);
        }
        //Change transform to stay on vehicles
        GameObject vehicle = Util.GetVehicleRecursive(collision.gameObject);
        if (vehicle != null && gameObject.tag != "usingWeapon") {
            transform.parent = vehicle.transform;
        }
    }

    private void OnCollisionExit(Collision collision) {
        //Change parent when not on vehicles
        /*if (collision.gameObject.tag == "eVehicle") {
            transform.parent = null;
        }*/
    }

    private void OnTriggerEnter(Collider other) {
        //Check if you hit the player and do action
        if(other.gameObject.tag == "RV") {
            transform.parent = other.gameObject.transform;
            agent.enabled = true;
        }

        if (other.gameObject.tag == "Player" && currentState == State.Fight) {
            player = other;
            isFighting = true;
        }

        if (other.gameObject.tag == "EnemyInteract" && currentState == State.Wait) {
            transform.parent = other.transform;
        }

        if (other.gameObject.tag == "Drops" && currentState == State.Steal) {
            Debug.Log("HIT" + other.gameObject.name);
            GameObject drop = other.gameObject;
            Destroy(other.gameObject);
            GameObject d = Instantiate(drop, transform);
            d.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            steal.hasStolen = true;

			myAni.SetTrigger("PickUpObject");
        }
    }

    private void OnTriggerStay(Collider other) {
        //Check if you hit a wall and destroy it
        if (other.gameObject.tag == "Player" && currentState == State.Fight) {
            inRange = true;
        }

        if (other.gameObject.tag == "Wall" && currentState == State.Destroy) {
            agent.speed = 0;
            StartCoroutine("DestroyWall", other);
        }

        if (other.gameObject.tag == "Engine" && currentState == State.Destroy) {
            agent.speed = 0;
            StartCoroutine("DestroyBattery", other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && currentState == State.Fight)
        {
            inRange = false;
        }
        if (other.gameObject.tag == "RV")
        {
            //transform.parent = null;
            //agent.enabled = true;
        }
    }
	#endregion

	#region Getters and Setters
	public State GetState()
    {
        return currentState;
    }

	/// <summary>
	/// Gets the enemy object (Deprecated; use .gameObject)
	/// </summary>
	/// <returns>The enemy object</returns>
	public GameObject GetEnemyObject() {
		return enemy;
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

    public Animator getAnimator()
    {
        return myAni;
    }
	#endregion
}
