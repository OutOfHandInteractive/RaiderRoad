using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAITest : MonoBehaviour {

    private bool hit;
    public Rigidbody enemy;
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "eVehicle")
        {
            transform.parent = other.transform;
        }

        if (other.gameObject.tag == "RV")
        {
            transform.parent = other.transform;
        }
        if (other.gameObject.tag == "Edge")
        {
            Debug.Log("Hit");
            hit = true;
            enemy.velocity = Vector3.zero;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "eVehicle")
        { transform.parent = null; }

        if (other.gameObject.tag == "RV")
        {
            transform.parent = null;
        }
    }
}
