using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Constructable<TrapNode>
{
    public float cooldownTime;
    public float launchAngle;
    public float launchMag;

    private List<Collider> colliders = new List<Collider>();
    private float cooldownRemaining = 0;

    public override void OnBreak()
    {
        // Do nothing
    }

    public override void OnStart()
    {
        
    }

    public override void OnUpdate()
    {
        cooldownRemaining = Mathf.Max(0, cooldownRemaining - Time.deltaTime);
    }

    private void CheckTrap()
    {
        if(cooldownRemaining > 0 || isHolo)
        {
            return;
        }
        Debug.Log("Checking " + colliders.Count + " colliders");
        foreach (Collider other in colliders)
        {
            if (other == null)
            {
                colliders.Remove(other);
                continue;
            }
            GameObject target = other.gameObject;
            Debug.Log("Collider object tag: " + target.tag);
            if (target.tag == "Enemy" || target.tag == "Player")
            {
                SpringTrap(target);
                cooldownRemaining = cooldownTime;
            }
            else
            {
                colliders.Remove(other);
            }
        }
    }

    private void SpringTrap(GameObject victim)
    {
        Debug.Log("Flinging enemy...");
        float angle = Mathf.Deg2Rad * launchAngle;
        float y = Mathf.Sin(angle) * launchMag;
        float z = Mathf.Cos(angle) * launchMag;
        victim.GetComponent<Rigidbody>().AddForce(new Vector3(0, y, -z));
        //victim.GetComponent<Rigidbody>().AddForce(Vector3.forward*1000000000f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider Entered");
        if (!isHolo)
        {
            colliders.Add(other);
            CheckTrap();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CheckTrap();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collider Exited");
        colliders.Remove(other);
    }
}
