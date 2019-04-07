using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heavyEnemy : EnemyType
{
    public float enemySpeed = 1f;

    public override RandomChoice<StatefulEnemyAI.State> BoardingChooser()
    {
        RandomChoice<StatefulEnemyAI.State> res = new RandomChoice<StatefulEnemyAI.State>(StatefulEnemyAI.State.Fight);
        res.SetChance(StatefulEnemyAI.State.Destroy, 1f / 5f);
        return res;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<StatefulEnemyAI>().speed = enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
