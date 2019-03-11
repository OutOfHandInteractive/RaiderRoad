using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class JumpEnemy : EnemyAI
{
    protected GameObject cObject;
    protected Rigidbody cRb;
    private float initialAngle;
    private bool hasJumped = false;
    protected VehicleAI.Side cSide;
    protected int action;
    protected NavMeshAgent agent;
    protected VehicleAI vehicle;

    public virtual void StartJump(GameObject enemy, Rigidbody rb, VehicleAI.Side side, NavMeshAgent _agent, int stateChance, VehicleAI _vehicle)
    {
        cObject = enemy;
        agent = _agent;
        cRb = rb;
        cSide = side;
        action = stateChance;
        initialAngle = 75f;
        vehicle = _vehicle;
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
        Vector3 planePos = new Vector3(cObject.transform.position.x, 0, cObject.transform.position.z);
        Vector3 planeTar = new Vector3(pos.x, 0, pos.z);

        //Planar distance between objects
        float distance = Vector3.Distance(planeTar, planePos);
        //Distance along the y axis between objects
        float yOffset = cObject.transform.position.y - pos.y;

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
        cObject.GetComponent<StatefulEnemyAI>().getAnimator().SetTrigger("Jump");
        cObject.GetComponent<StatefulEnemyAI>().getAnimator().SetBool("Grounded", false);
        hasJumped = true;
    }
}
