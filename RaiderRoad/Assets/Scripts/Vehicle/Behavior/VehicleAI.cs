using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleAI : MonoBehaviour {
    //States
    public enum State { Wait, Wander, Chase, Stay, Attack, Leave };
    public enum Side { Left, Right };

    //State Classes
    private WaitVehicle wait;
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

    private Side side;
    private bool hasWeapon;
    public ParticleSystem collision;
    public ParticleSystem deathMiniExplosions;
    public ParticleSystem deathBigExplosion;

    public GameObject explosionSound;

    public float batteryDropChance = 1.0f;
    public GameObject batteryDrop;
    
    public bool testDeath = false;

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

	// Use this for initialization
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
        rb = GetComponent<Rigidbody>();

		front_attachment = GetComponentInChildren<Attachment>();

        hasWeapon = (GetComponentInChildren<HasWeapon>() != null);
        Debug.Log(side);
        //Start wander state
        EnterWait();

        vCamShake = GameObject.FindGameObjectWithTag("MainVCam").GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(transform.position.x, .7f, transform.position.z);
        if(transform.position.z >16f)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
        //Debug.Log(currentState);
        if (currentState == State.Attack)
        {
            //agent.speed = 30;
        }
        else
        {
            //agent.speed = 15;
        }
        if(transform.GetComponentInChildren<PlayerController_Rewired>())
        {
            EnterWander();
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
        }

        // Test Death
        if (testDeath)
        {
            //DelayedDeath();
            Die();
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

	public void DestroyPart() {
		takeDamage(maxHealth * 0.4f);
	}

	#region state change functions
	//Used to change state from different classes
    public void EnterWait()
    {
        wait.StartWait(enemy);
        currentState = State.Wait;
    }
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
        leave.StartLeave(agent, enemy);
        currentState = State.Leave;
    }
	#endregion

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
            transform.parent = other.transform;
            DelayedDeath();
            StartCoroutine(WaitToDie());
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
        }
        
        Instantiate(explosionSound, transform.position, Quaternion.identity);
        Instantiate(deathBigExplosion, transform.position, Quaternion.identity);
        //Radio.GetRadio().RemoveVehicle(gameObject);
        Destroy(gameObject);
    }

    private void DelayedDeath()
    {
        ParticleSystem myMiniXplos = Instantiate(deathMiniExplosions, transform.position, Quaternion.identity);
        myMiniXplos.transform.parent = transform;
        float time = Mathf.SmoothStep(0, 1, 20 * Time.deltaTime);
        //agent.isStopped = true;
        transform.Translate(Vector3.forward * time * -1);
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie()
    {
        StartCoroutine(DeathMovement());
        yield return new WaitForSeconds(5);
        Die();
    }

    IEnumerator DeathMovement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * -1);
        yield return null;
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
