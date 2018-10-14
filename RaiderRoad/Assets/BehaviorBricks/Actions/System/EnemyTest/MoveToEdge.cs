using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("EnemyTest/MoveToEdge")]
public class MoveToEdge : BasePrimitiveAction
{
    [InParam("enemy")]
    [Help("Enemy")]
    public GameObject enemy;

    [InParam("vehicle")]
    [Help("Vehicle to jump to")]
    public GameObject vehicle;

    [InParam("rigidEnemy")]
    [Help("Rigidbody enemy")]
    public Rigidbody rbody;

    private Transform enemyTransform;
    public override void OnStart()
    {
        enemyTransform = enemy.transform;
    }

    public override TaskStatus OnUpdate()
    {
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, vehicle.transform.position, .1f);
        return TaskStatus.COMPLETED;
    }
}

