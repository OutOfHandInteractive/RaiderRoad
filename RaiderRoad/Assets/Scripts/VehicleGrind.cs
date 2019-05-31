using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGrind : MonoBehaviour
{
    public enum Side { left, right };
    public Side side;
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
                    v.takeDamage(0.5f);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("eVehicle"))
        {
			Debug.Log("VEHICLE ON WALL");
            if (side == Side.left)
            {
                Transform sparks = other.transform.Find("SparksL");
                sparks.GetComponent<ParticleSystem>().Play();
                //Instantiate(sparks, place, Quaternion.Euler(new Vector3(-90, 0, 0)), other.gameObject.transform);
            }
            else
            {
                Transform sparks = other.transform.Find("SparksR");
                sparks.GetComponent<ParticleSystem>().Play();
                //Instantiate(sparks, place, Quaternion.Euler(new Vector3(-90,0,0)), other.gameObject.transform);
            }
            vehicles.Add(other.gameObject.GetComponentInParent<VehicleAI>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("eVehicle"))
        {
            if (side == Side.left)
            {
                
                Transform sparks = other.transform.Find("SparksL");
                sparks.GetComponent<ParticleSystem>().Stop();
                //Instantiate(sparks, place, Quaternion.Euler(new Vector3(-90, 0, 0)), other.gameObject.transform);
            }
            else
            {
                Transform sparks = other.transform.Find("SparksR");
                sparks.GetComponent<ParticleSystem>().Stop();
                //Instantiate(sparks, place, Quaternion.Euler(new Vector3(-90,0,0)), other.gameObject.transform);
            }
            Debug.Log("removed v from grind Trigger");
            vehicles.Remove(other.gameObject.GetComponentInParent<VehicleAI>());
        }
    }
}
