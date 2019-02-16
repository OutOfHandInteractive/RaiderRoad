using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mediumEnemy : EnemyAI
{
    public float enemySpeed = 2f;
    private int actionChance;
    // Start is called before the first frame update
    void Start()
    {
        actionChance = Random.Range(0, 50);
        GetComponent<StatefulEnemyAI>().stateChance = actionChance;
        speed = enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
