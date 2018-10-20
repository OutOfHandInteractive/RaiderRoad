using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoardEnemy : MonoBehaviour {

    //enemy, rigidbody,rv, angle to jump, if enemy jumped, chance to take action, current side 
    private GameObject cObject;
    private Rigidbody cRb;
    private Transform RV;
    private float initialAngle;
    private bool hasJumped = false;
    private int action = Random.Range(0, 100);
    private string cSide;


    public void StartBoard(GameObject enemy, Rigidbody rb, string side)
    {
        //Set rv, enemy, rigidbody, current side, and angle to jump
        RV = GameObject.FindGameObjectWithTag("RV").transform;
        cObject = enemy;
        cRb = rb;
        cSide = side;
        initialAngle = 75f;


    }

    public void Board()
    {
        //RV destination position
        Vector3 pos = RV.position;

        //Get gravity
        float gravity = Physics.gravity.magnitude;
        //Selected angle in radians
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
            //Use positive velocity if vehicle is on left side
            velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
        }
        else
        {
            //Use negative velocity if vehicle is on left side
            velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), -initialVelocity * Mathf.Cos(angle));
        }

        //Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planeTar - planePos);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        //Execute jump only once
        if(!hasJumped)
        {
            cRb.AddForce(finalVelocity * cRb.mass, ForceMode.Impulse);
            hasJumped = true;
        }

        //50% chance to go into Destroy State or Fight State
        if (action < 50)
        {
            cObject.GetComponent<EnemyAI>().Invoke("EnterDestroy", 5f);
        }
        else
        {
            cObject.GetComponent<EnemyAI>().Invoke("EnterFight", 5f);
        }
        
    }
}
