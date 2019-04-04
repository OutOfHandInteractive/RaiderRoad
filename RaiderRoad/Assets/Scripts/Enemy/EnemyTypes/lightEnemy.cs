using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightEnemy : EnemyType
{
    public float enemySpeed = 3f;
    private int actionChance;

    public override RandomChoice<StatefulEnemyAI.State> BoardingChooser()
    {
        return new RandomChoice<StatefulEnemyAI.State>(StatefulEnemyAI.State.Steal);
    }
    // Start is called before the first frame update
    void Start()
    {
        actionChance = Random.Range(80, 100);
        StatefulEnemyAI master = GetComponent<StatefulEnemyAI>();
        master.speed = enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
