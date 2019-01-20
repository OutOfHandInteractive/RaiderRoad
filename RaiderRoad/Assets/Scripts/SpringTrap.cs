using UnityEngine;
using System.Collections;

public class SpringTrap : Trap
{
    public float launchAngle;
    public float launchMag;

    public override void Activate(GameObject victim)
    {
        Debug.Log("Flinging enemy...");
        float angle = Mathf.Deg2Rad * launchAngle;
        float y = Mathf.Sin(angle) * launchMag;
        float z = Mathf.Cos(angle) * launchMag;
        victim.GetComponent<Rigidbody>().AddForce(new Vector3(0, y, -z));
        //victim.GetComponent<Rigidbody>().AddForce(Vector3.forward*1000000000f);
    }
}
