using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Main state machine class for the vehicle AI
/// </summary>
public class VehicleAI : MonoBehaviour {
    /// <summary>
    /// The vehicle states
    /// </summary>
    public enum State { Wait, Wander, Chase, Stay, Attack, Leave, Rammed };

    /// <summary>
    /// The sides of the RV
    /// </summary>
    public enum Side { Left, Right };

    //State Classes
    private WaitVehicle wait;
    private WanderVehicle wander;
    private ChaseVehicle chase;
    private StayVehicle stay;
    private AttackVehicle attack;
    private LeaveVehicle leave;
    private RammedVehicle rammed;

    //Current object and navmesh
    protected NavMeshAgent agent;
    public State currentState;
    private GameObject enemy;
    private Rigidbody rb;
    private int attackPoint;

    private Side side;
    private bool hasWeapon;
    public ParticleSystem collision;
    public ParticleSystem deathMiniExplosions;
    public ParticleSystem deathBigExplosion;

    public GameObject explosionSound;

    public float batteryDropChance = 1.0f;
    public GameObject batteryDrop;

    public bool testDeath = false;
    public bool isRammed = false;
    public bool isGrinding = false;
    private float stayTime = 0;
    private float delayTime = 0;

    //Statistics
    public float maxHealth;
	[SerializeField] private float ramDamage;
	[SerializeField] private float speed;
	[SerializeField] private float movementChance;
	[SerializeField] private int threat;

	// --------------- private variables --------------------
	// references
	private Attachment front_attachment;

    // Camera Shake
    private CameraShake vCamShake;

    // gameplay values
    private float currentHealth;
	[SerializeField] private float highDamageThreshold;
    private bool dying = false;

    /// <summary>
    /// Initialiaze the state machine
    /// </summary>
	void Start () {
		currentHealth = maxHealth;
        hasWeapon = false;
        //Initialize all the classes
        enemy = gameObject;
        agent = GetComponent<NavMeshAgent>();
        wait = enemy.AddComponent<WaitVehicle>();
        wander = enemy.AddComponent<WanderVehicle>();
        chase = enemy.AddComponent<ChaseVehicle>();
        stay = enemy.AddComponent<StayVehicle>();
        attack = enemy.AddComponent<AttackVehicle>();
        leave = enemy.AddComponent<LeaveVehicle>();
        rammed = enemy.AddComponent<RammedVehicle>();
        rb = GetComponent<Rigidbody>();

		front_attachment = GetComponentInChildren<Attachment>();

        hasWeapon = (GetComponentInChildren<HasWeapon>() != null);
        Debug.Log(side);
        //Start wander state
        transform.position = new Vector3(transform.position.x, .7f, transform.position.z);
        Debug.Log("starting pos: " + transform.position);
        EnterWait();

        //find MainVCam
        GameManager g = GameManager.GameManagerInstance;
        vCamShake = g.MainVCamShake;
    }

    /// <summary>
    /// Update and call down to the current state
    /// </summary>
    void Update () {
        //transform.position = new Vector3(transform.position.x, .7f, transform.position.z);
        if(isRammed)
        {
            EnterRammed();
        }
        /*if(transform.position.z >16f)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }*/
        //Debug.Log(currentState);
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
            stayTime += Time.deltaTime;
            delayTime += Time.deltaTime;
            if(stayTime < 15)
            {
                if(delayTime > 2)
                {
                    EnterWander();
                }
            }
            else
            {
                EnterLeave();
            }
        }
        if (dying)
        {
            return;
        }
        switch (currentState)
        {
            case State.Wait:
                wait.Wait();
                break;
            case State.Wander:
                wander.Wander();
                break;
            case State.Chase:
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
            case State.Rammed:
                rammed.StartRammed();
                break;
        }

        // Test Death
        if (testDeath)
        {
            //DelayedDeath();
            Die();
        }
    }

	// ---------------- Combat Functions ------------------
    /// <summary>
    /// Suffer the given amount of damage
    /// </summary>
    /// <param name="damage">The damage to take</param>
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

    /// <summary>
    /// Take proportionate damage to the destruction of a part
    /// </summary>
	public void DestroyPart() {
		takeDamage(maxHealth * 0.4f);
	}

	#region state change functions
	//Used to change state from different classes

    /// <summary>
    /// Enter the Wait state
    /// </summary>
    public void EnterWait()
    {
        wait.StartWait(enemy);
        currentState = State.Wait;
    }

    /// <summary>
    /// Enter the Wander state
    /// </summary>
	public void EnterWander()
    {
        wander.StartWander(agent, enemy, side, hasWeapon);
        currentState = State.Wander;
    }

    /// <summary>
    /// Enter the Chase state
    /// </summary>
    public void EnterChase()
    {
        chase.StartChase(agent, enemy, side);
        currentState = State.Chase;
    }

    /// <summary>
    /// Enter the Stay state
    /// </summary>
    public void EnterStay(int stickPoint)
    {
        stay.StartStay(agent, enemy, side, stickPoint);
        currentState = State.Stay;
    }

    /// <summary>
    /// Enter the Attack state
    /// </summary>
    public void EnterAttack()
    {
        attack.StartAttack(agent, enemy, rb, side);
        currentState = State.Attack;
    }

    /// <summary>
    /// Enter the Leave state
    /// </summary>
    public void EnterLeave()
    {
        leave.StartLeave(agent, enemy);
        currentState = State.Leave;
    }

    /// <summary>
    /// Enter the Rammed state
    /// </summary>
    public void EnterRammed()
    {
        rammed.StartRammed();
        currentState = State.Rammed;
    }
    #endregion
    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("COLLIDE " + collision.gameObject.tag);
        if (collision.gameObject.tag == "RV")
        {
            isRammed = true;
        }
        if (collision.gameObject.tag == "eVehicle" && isGrinding)
        {
            Die();
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "RV" && isRammed)
        {
            float time = Mathf.SmoothStep(0, 1, 5 * Time.deltaTime);
            transform.Translate(Vector3.forward * time);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RV")
        {
            isRammed = false;
            EnterWander();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Destroy this when it goes off screen
        if (other.tag == "Exit")
            Die();
        if (other.gameObject.tag.Equals("Obstacle"))
        {
            Debug.Log("You Hit an Obstacle");
            ParticleSystem explosion = Instantiate(collision, other.gameObject.transform.position, Quaternion.identity, gameObject.transform);
            explosion.gameObject.transform.localScale *= 10;
            //Destroy(other.gameObject);
            GetComponent<Rigidbody>().isKinematic = false;
            transform.parent = other.gameObject.transform;
            DelayedDeath(false);
        }

    }

	#region Effect Functions
	private void startHighDamageSmokeEffects() {
		front_attachment.StartHighDamageSmokeEffects();
	}
	#endregion

	#region death functions
	private void Die()
    {
        
        if (currentState != State.Leave)
        {
            // Camera Shake
            vCamShake.Shake(.5f, 10f, .5f);

            // Battery Drop
            float rand = Random.value; // Battery Drop Chance
            if (side == Side.Left)
            {
                if (rand <= batteryDropChance)
                {
                    GameObject battery = Instantiate(batteryDrop, transform.position, Quaternion.identity);
                    battery.GetComponent<DropExplosion>().Eject(1f);
                }
            }
            else
            {
                if (rand <= batteryDropChance)
                {
                    GameObject battery = Instantiate(batteryDrop, transform.position, Quaternion.identity);
                    battery.GetComponent<DropExplosion>().Eject(-1f);
                }
            }
        }

        // Player Eject
        foreach (PlayerController_Rewired pc in gameObject.GetComponentsInChildren<PlayerController_Rewired>())
        {
            pc.transform.parent = null;
            if (side == Side.Left)
            {
                pc.Eject(1f);
            }
            else
            {
                pc.Eject(-1f);
            }
        }

        Instantiate(explosionSound, transform.position, Quaternion.identity);
        Instantiate(deathBigExplosion, transform.position, Quaternion.identity);
        //Radio.GetRadio().RemoveVehicle(gameObject);
        Destroy(gameObject);
    }

    private void DelayedDeath(bool deathMovement = true)
    {
        dying = true;
        ParticleSystem myMiniXplos = Instantiate(deathMiniExplosions, transform.position, Quaternion.identity);
        myMiniXplos.transform.parent = transform;
        agent.isStopped = true;
        agent.enabled = false;
        //float time = Mathf.SmoothStep(0, 1, 20 * Time.deltaTime);
        //transform.Translate(Vector3.forward * time * -1);
        StartCoroutine(WaitToDie(deathMovement));
    }

    IEnumerator WaitToDie(bool deathMovement)
    {
        if (deathMovement)
        {
            StartCoroutine(DeathMovement());
        }
        yield return new WaitForSeconds(5);
        Die();
    }

    IEnumerator DeathMovement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * -1);
        yield return null;
    }
    IEnumerator WaitToLeaveWithPlayer()
    {
        EnterWander();
        yield return new WaitForSeconds(3);
        EnterLeave();
    }
    #endregion

    #region Getters and Setters
    public float getMaxHealth() {
		return maxHealth;
	}

	public void SetMaxHealth(float _maxHealth) {
		maxHealth = _maxHealth;
		currentHealth = maxHealth;
	}

	public float getHealth() {
		return currentHealth;
	}

	public float getRamDamage() {
		return ramDamage;
	}

	public void SetRamDamage(float _ramDamage) {
		ramDamage = _ramDamage;
	}

	public float getSpeed() {
		return speed;
	}

	public void SetSpeed(float _speed) {
		speed = _speed;
	}

	public float getMovementChance() {
		return movementChance;
	}

	public void SetMovementChance(float _chance) {
		movementChance = _chance;
	}

    /// <summary>
    /// Convert the string to a side and assign the vehicle to that side
    /// </summary>
    /// <param name="_side">The string to interpret</param>
    public void setSide(string _side)
    {
        if(_side == "left")
        {
            side = Side.Left;
        }
        else
        {
            side = Side.Right;
        }
    }

    public Side getSide()
    {
        return side;
    }
    public State getState()
    {
        return currentState;
    }
	#endregion
}
