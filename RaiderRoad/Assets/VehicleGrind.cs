using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGrind : MonoBehaviour
{
    private List<VehicleAI> vehicles = new List<VehicleAI>();

    // Update is called once per frame
    void Update()
    {
        if(vehicles.Count > 0)
        {
            foreach(VehicleAI v in vehicles)
            {
                if(v.getHealth() > 0)
                {
                    v.takeDamage(1);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("eVehicle"))
        {
            Debug.Log("added v to grind Trigger");
            vehicles.Add(other.gameObject.GetComponentInParent<VehicleAI>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("eVehicle"))
        {
            Debug.Log("removed v from grind Trigger");
            vehicles.Add(other.gameObject.GetComponentInParent<VehicleAI>());
        }
    }
}
