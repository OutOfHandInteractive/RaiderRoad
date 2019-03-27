using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public abstract class JumpEnemy : EnemyAIState
{
    protected Rigidbody cRb;
    private float initialAngle;
    private bool hasJumped = false;
    protected VehicleAI.Side cSide;
    protected int action;
    protected NavMeshAgent agent;
    protected VehicleAI vehicle;

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        agent = master.Agent;
        cRb = master.Rb;
        cSide = master.Side;
        action = master.stateChance;
        initialAngle = 75f;
        vehicle = master.Vehicle;
    }

    protected void Jump(Vector3 pos, float zSign)
    {
        //Execute jump only once
        if (hasJumped)
        {
            return;
        }
        //Get gravity
        float gravity = Physics.gravity.magnitude;
        //Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        //Positions of this object and the target on the same plane
        Vector3 planePos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        Vector3 planeTar = new Vector3(pos.x, 0, pos.z);

        //Planar distance between objects
        float distance = Vector3.Distance(planeTar, planePos);
        //Distance along the y axis between objects
        float yOffset = gameObject.transform.position.y - pos.y;

        //Equation to get initial velocity
        // vi = (1/cos(theta)) * sqrt((g * d^2 /2)/(d*tan(theta)+y))
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        
        //Use positive velocity if vehicle is on left side, negative otherwise
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), zSign * initialVelocity * Mathf.Cos(angle));

        //Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planeTar - planePos);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        cRb.AddForce(finalVelocity * cRb.mass, ForceMode.Impulse);
        //agent.SetDestination(pos);
        //animation
        master.getAnimator().Jump();
        hasJumped = true;
    }
}
