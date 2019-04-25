using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropExplosion : MonoBehaviour
{
    private Rigidbody rb;

    public void Eject(float zSign)
    {
        rb = GetComponent<Rigidbody>();
        transform.parent = null;
        float gravity = Physics.gravity.magnitude;

        //Positions of this object and the target on the same plane
        int rand = Random.Range(1,5);
        Vector3 pos;
        switch (rand)
        {
            case 1:
                pos = GameObject.Find("player1Spawn").transform.position;
                break;

            case 2:
                pos = GameObject.Find("player2Spawn").transform.position;
                break;

            case 3:
                pos = GameObject.Find("player3Spawn").transform.position;
                break;

            default:
                pos = GameObject.Find("player4Spawn").transform.position;
                break;
        }
        Vector3 planePos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 planeTar = new Vector3(pos.x, 0, pos.z);

        //Selected angle in radians
        float angle = 60f * Mathf.Deg2Rad;

        //Planar distance between objects
        float distance = Vector3.Distance(planeTar, planePos);
        //Distance along the y axis between objects
        float yOffset = transform.position.y - pos.y;

        //Equation to get initial velocity
        // vi = (1/cos(theta)) * sqrt((g * d^2 /2)/(d*tan(theta)+y))
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));


        //Use positive velocity if vehicle is on left side, negative otherwise
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), zSign * initialVelocity * Mathf.Cos(angle));

        //Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, (planeTar - planePos) * zSign);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        rb.AddForce(finalVelocity * rb.mass, ForceMode.Impulse);
    }
}
