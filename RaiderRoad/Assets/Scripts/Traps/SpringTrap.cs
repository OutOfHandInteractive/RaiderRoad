using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for spring traps. These traps lie on the ground and catapult any enemies off the back of the RV.
/// </summary>
public class SpringTrap : Trap
{
    /// <summary>
    /// The launch angle in degrees. Set in the editor
    /// </summary>
    public float launchAngle;

    /// <summary>
    /// The launch magnitude in force units (Newtons?). Set in the editor
    /// NOTE: testing shows this has to be huge
    /// </summary>
    public float launchMag;

    public override void OnBreak()
    {
        // Do nothing
    }

    /// <summary>
    /// On activation, the victim gets a large force applied to their rigidbody
    /// </summary>
    /// <param name="victim">The target to fling</param>
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
