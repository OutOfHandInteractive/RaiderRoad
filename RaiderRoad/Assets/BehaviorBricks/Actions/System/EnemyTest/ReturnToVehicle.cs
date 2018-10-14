using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("EnemyTest/ReturnToVehicle")]
public class ReturnToVehicle : BasePrimitiveAction
{
    [InParam("rigidEnemy")]
    [Help("Rigidbody enemy")]
    public Rigidbody rbody;

    private Vector3 jump;
    private bool grounded;
    public override void OnStart()
    {
        jump = new Vector3(0f, 2f, 2f);
        if (grounded)
        {
            rbody.AddForce(jump * 2f, ForceMode.Impulse);
            grounded = false;
        }

    }

    public override TaskStatus OnUpdate()
    {
        //enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, vehicle.transform.position, .03f);
        return TaskStatus.COMPLETED;
    }

    private void OnCollisionStay()
    {
        grounded = true;
    }
}