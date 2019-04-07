using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This enemy just wants to destroy things and pick fights
/// </summary>
public class DestroyEnemy : TargetedEnemy {
    //enemy, speed
    private int action;

    /// <summary>
    /// TODO: Ernest please explain this
    /// </summary>
    public bool engineKill = false;
    
    private NavMeshAgent agent;

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        agent = master.Agent;
        action = Random.Range(0, 100);
    }

    /// <summary>
    /// Performs the destroy actions
    /// </summary>
    public override void UpdateState()
    {

        if (master.getDamaged())
        {
            master.EnterFight();
        }

        //If there are no more walls, go to Fight state, else keep going for walls
        if (engineKill && gameObject.GetComponent<lightEnemy>())
        {
            master.EnterSteal();
        }
        else if (engineKill && gameObject.transform.parent != null)
        {
            master.EnterFight();
        }
        else
        {
            //Find destroyable and go to it
            ChanceDestroy();
        }
    }

    protected override string[] TargetedTags()
    {
        if(action < 90)
        {
            return new string[] { Constants.WALL_TAG, Constants.ENGINE_TAG };
        }
        return new string[] { Constants.ENGINE_TAG };
    }

    protected override bool IsValidTarget(GameObject obj)
    {
        return obj != null;
    }

    protected override bool SearchFilter(GameObject obj)
    {
        return base.SearchFilter(obj) && Unoccupied(obj);
    }

    private void ChanceDestroy()
    {
        GameObject _target = GetTarget();
        if(_target != null)
        {
            Occupy(_target);
            Util.AssertNotNull("Agent should not be null", agent);
            agent.SetDestination(_target.transform.position);
            master.getAnimator().Running = true;
        }
        else
        {
            master.EnterFight();
        }
    }

    public override void TriggerStay(Collider other)
    {
        base.TriggerStay(other);
        bool isWall = Util.IsWall(other.gameObject);
        bool isEngine = Util.IsEngine(other.gameObject);
        if (other.gameObject == GetTarget())
        {
            agent.isStopped = true;
            master.getAnimator().Running = false;
            //Debug.Log("HIT");
            master.getAnimator().Attack(); //visual of enemy breaking object
            master.damageMeter += 100f/3f * Time.deltaTime;
            if (master.damageMeter >= 100)
            {
                other.gameObject.GetComponent<Constructable>().Damage(100f);
                if (isWall && gameObject.GetComponent<lightEnemy>())
                {
                    Debug.Log("STEAL THE WALL DUDE");
                    master.EnterSteal();
                }
                engineKill = engineKill || isEngine;
                master.damageMeter = 0;
                agent.isStopped = false;
            }
        }
    }

    public override Color StateColor()
    {
        return Color.yellow;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Destroy;
    }
}
