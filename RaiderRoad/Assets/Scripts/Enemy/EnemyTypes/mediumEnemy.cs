using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mediumEnemy : EnemyType
{
    public float enemySpeed = 2f;
    private int actionChance = 0;

    public override RandomChoice<StatefulEnemyAI.State> BoardingChooser()
    {
        RandomChoice<StatefulEnemyAI.State> res = new RandomChoice<StatefulEnemyAI.State>(StatefulEnemyAI.State.Fight);
        res.SetChance(StatefulEnemyAI.State.Destroy, 4f / 5f);
        return res;
    }

    // Start is called before the first frame update
    void Start()
    {
        actionChance = Random.Range(0, 50);
        StatefulEnemyAI master = GetComponent<StatefulEnemyAI>();
        master.speed = enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
