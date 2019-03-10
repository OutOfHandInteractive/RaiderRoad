using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGrind : MonoBehaviour
{
    public enum Side { left, right };
    public Side side;
    public ParticleSystem sparks;
    private List<VehicleAI> vehicles = new List<VehicleAI>();

    // Update is called once per frame
    void Update()
    {
        if(vehicles.Count > 0)
        {
            foreach(VehicleAI v in vehicles)
            {
                if (v.getHealth() > 0)
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
            if (side == Side.left)
            {
                float xVal = other.transform.position.x;
                xVal -= 1.7f;
                Vector3 place = new Vector3(xVal, other.transform.position.y, other.transform.position.z);
                Instantiate(sparks, place, Quaternion.Euler(new Vector3(-90, 0, 0)), other.gameObject.transform);
            }
            else
            {
                float xVal = other.transform.position.x;
                xVal += 1.7f;
                Vector3 place = new Vector3(xVal, other.transform.position.y, other.transform.position.z);
                Instantiate(sparks, place, Quaternion.Euler(new Vector3(-90,0,0)), other.gameObject.transform);
            }
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
