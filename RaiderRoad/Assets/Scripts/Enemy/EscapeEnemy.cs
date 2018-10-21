using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeEnemy : MonoBehaviour {

    //Gameobject, rigidbody, vehicle, initialangle for jump, if enemy jumped, current side 
    private GameObject cObject;
    private Rigidbody cRb;
    private Transform eVehicle;
    private float initialAngle;
    private bool hasJumped = false;
    private string cSide;

    public void StartEscape(GameObject enemy, Rigidbody rb, string side)
    {
        //Initialize vehicle, enemy, rigidbody, side and angle for jumping
        eVehicle = GameObject.FindGameObjectWithTag("eVehicle").transform;
        cObject = enemy;
        cRb = rb;
        cSide = side;
        initialAngle = 75f;
    }

    public void Escape()
    {

        //Enemy vehicle destination position
        Vector3 pos = eVehicle.position;

        //Get gravity
        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        //Positions of this object and the target on the same plane
        Vector3 planeTar = new Vector3(pos.x, 0, pos.z);
        Vector3 planePos = new Vector3(cObject.transform.position.x, 0, cObject.transform.position.z);

        //Planar distance between objects
        float distance = Vector3.Distance(planeTar, planePos);
        //Distance along the y axis between objects
        float yOffset = cObject.transform.position.y - pos.y;
        
        //Equation to get initial velocity
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3();
        if (cSide.Equals("left"))
        {
            //Use negative velocity if vehicle is on left side
            velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), -initialVelocity * Mathf.Cos(angle));
        }
        else
        {
            //Use positive velocity if vehicle is on right side
            velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
        }


        //Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planeTar - planePos);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        //Execute jump only once
        if (!hasJumped)
        {
            cRb.AddForce(finalVelocity * cRb.mass, ForceMode.Impulse);
            hasJumped = true;
        }
    }
}
