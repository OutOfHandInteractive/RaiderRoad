using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "eVehicle" || other.gameObject.tag == "Destructable")
        {
            Debug.LogWarning("HIT");
            other.transform.root.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            other.transform.root.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "eVehicle" || other.gameObject.tag == "Destructable")
        {
            Debug.LogWarning("HIT");
            other.transform.root.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            other.transform.root.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
