using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is for enemies that want to seek out and beat up players.
/// </summary>
public class FightEnemy : TargetedEnemy {

    public struct FightContext : StateContext
    {
        public GameObject target; 
    }

    //Enemy and enemy speed
    private GameObject fightRange;
    private float playerDamage = 0;
    private float knockback_force = 2000f;
    private GameObject eVehicle;
    private NavMeshAgent agent;
    private HashSet<GameObject> inRange;
    [SerializeField] private bool isWindingUp = false;

    [SerializeField] Vector3 targetPosition;

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        //Initialized enemy
        inRange = new HashSet<GameObject>();
        agent = master.Agent;
        if(context != null && context is FightContext)
        {
            SetTarget(((FightContext)context).target);
        }
        else
        {
            // Reset targeter
            SetTarget(null);
        }
        fightRange = gameObject.transform.Find("EnemyAttack").gameObject;
        //eVehicle = stateMachine.Vehicle.gameObject;
    }

    protected override string[] TargetedTags()
    {
        return new string[] { Constants.PLAYER_TAG };
    }

    protected override bool IsValidTarget(GameObject obj)
    {
        return obj != null && Util.IsAlive(obj);
    }

    public override void UpdateState()
    {
        if (isWindingUp)
        {
            return;
        }
        //Get player object
        GameObject player = GetTarget();
        //Get enemy speed

        //If doesnt exist or if player has been hit go into escape state
        if(player == null)
        {
            return;
        }
        else if (playerDamage >= 4f || master.currentHealth <= 25f)
        {
            Debug.Log("reached");
            master.CallEvac();
        }
        if(inRange.Count > 0)
        {
            StartCoroutine(WindUp());
        }
        else
        {
            targetPosition = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
            if (OnVehicle())
            {
                MoveToward(targetPosition);
            }
            else if (OnRV())
            {
                //Look at player and move towards them
                gameObject.transform.LookAt(targetPosition);
                agent.SetDestination(targetPosition);
                master.getAnimator().Running = true;
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, movement);

            }
            else
            {
                Debug.LogError("WTF? Not on vehicle or RV?");
            }
        }
    }

    public override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if (Util.IsPlayer(other.gameObject))
        {
            inRange.Add(other.gameObject);
        }
    }

    public override void TriggerExit(Collider other)
    {
        base.TriggerExit(other);
        if (Util.IsPlayer(other.gameObject))
        {
            inRange.Remove(other.gameObject);
        }
    }

    IEnumerator WindUp()
    {
        //Debug.Log(Time.time);
        isWindingUp = true;
        WindupAttack();
        //myAni.SetTrigger("WindUp");
        yield return new WaitForSeconds(.5f);
        if (master.IsCurrent(this))
        {
            bool hit = false;
            master.getAnimator().Attack();
            foreach (GameObject other in inRange)
            {
                HitPlayer(other, master.damagePower);
                hit = true;
            }
            if (!hit)
            {
                Missed();
            }
        }
        else
        {
            Missed();
        }
        //Debug.Log(Time.time);
        agent.isStopped = false;
        isWindingUp = false;
    }

    /// <summary>
    /// Show the wind-up
    /// </summary>
    public void WindupAttack()
    {
        agent.isStopped = true;
        master.getAnimator().Running = false;
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, .5f);
    }

    /// <summary>
    /// Punch the given player
    /// </summary>
    /// <param name="other">The player to hit</param>
    public void HitPlayer(GameObject other, float damage)
    {
        playerDamage += damage;
        //fightRange.GetComponent<Renderer>().material.color = new Color(255f, 0f, 0f, .5f);
        other.GetComponent<PlayerController_Rewired>().takeDamage(damage);
        Vector3 dir = other.transform.position - gameObject.transform.position;
        dir = Vector3.Normalize(new Vector3(dir.x, 0.0f, dir.z));
        other.GetComponent<Rigidbody>().AddForce(dir * knockback_force);
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, 0f);
    }

    /// <summary>
    /// Show the miss
    /// </summary>
    public void Missed()
    {
        fightRange.GetComponent<Renderer>().material.color = new Color(255f, 150f, 0f, 0f);
    }

    public override Color StateColor()
    {
        return Color.red;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Fight;
    }
}