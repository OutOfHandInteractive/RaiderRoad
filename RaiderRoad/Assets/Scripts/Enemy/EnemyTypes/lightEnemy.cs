using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightEnemy : EnemyAI
{
    public float enemySpeed = 3f;
    private int actionChance;
    // Start is called before the first frame update
    void Start()
    {
        actionChance = Random.Range(80, 100);
        GetComponent<StatefulEnemyAI>().stateChance = actionChance;
        speed = enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
