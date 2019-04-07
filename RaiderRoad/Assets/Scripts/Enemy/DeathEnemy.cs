using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This enemy just wants to die
/// </summary>
public class DeathEnemy : EnemyAIState {

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
    }

    /// <summary>
    /// Performs the raider death actions (ritual?)
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="drop"></param>
    private void Death(GameObject enemy, GameObject drop)
    {
        spawnDrop(drop, enemy);
        stealDrop(enemy);
        Destroy(enemy);
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Death;
    }

    public override Color StateColor()
    {
        return Color.gray;
    }

    public override void UpdateState()
    {
        Death(gameObject, master.dropOnDeath);
    }

    void spawnDrop(GameObject drop, GameObject enemy)
    {
        Debug.Log("Wall dropped!");
        if(transform.parent != null) //REALLY SHOULDN'T STAY THIS WAY IMO
        {
            GameObject item = Instantiate(drop, new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z), 
                Quaternion.identity, transform.parent.transform);
            item.name = "Wall Drop";
        }
    }
    void stealDrop(GameObject enemy)
    {
        foreach (Transform child in enemy.transform)
        {
            if (child.tag == "Drops")
            {
                child.parent = transform.parent;
            }
        }
    }
}
