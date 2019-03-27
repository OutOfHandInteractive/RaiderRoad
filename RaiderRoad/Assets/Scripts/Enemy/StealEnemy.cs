using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StealEnemy : EnemyAIState {


    //enemy, speed
    public bool hasStolen = false;
    private GameObject[] drops;
    private GameObject drop;

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        drops = GameObject.FindGameObjectsWithTag("Drops");
        drop = Closest(gameObject.transform.position, drops);
    }

    public override void UpdateState()
    {
        if (master.getDamaged())
        {
            master.EnterFight();
        }

        //If there are no more drops, go to Escape state, else keep going for drops
        if (hasStolen && gameObject.transform.GetComponentInChildren<ItemDrop>())
        {
            master.EnterEscape();
        }
        else
        {
            //Find wall and go to it
            if(drop != null)
            {
                MoveToward(drop);
            }
            else
            {
                master.EnterDestroy();
            }

        }
    }

    public override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if (other.gameObject.tag == "Drops")
        {
            Debug.Log("HIT" + other.gameObject.name);
            GameObject drop = other.gameObject;
            Destroy(other.gameObject);
            Instantiate(drop, transform);
            //other.transform.position = new Vector3(0, 2, 0);
            hasStolen = true;
        }
    }

    public override Color StateColor()
    {
        return Color.magenta;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Steal;
    }
}
