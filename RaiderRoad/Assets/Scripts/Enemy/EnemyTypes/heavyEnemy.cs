using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heavyEnemy : EnemyAI
{
    public float enemySpeed = 1f;
    private int actionChance;
    // Start is called before the first frame update
    void Start()
    {
        actionChance = Random.Range(30, 80);
        GetComponent<StatefulEnemyAI>().stateChance = actionChance;
        speed = enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
