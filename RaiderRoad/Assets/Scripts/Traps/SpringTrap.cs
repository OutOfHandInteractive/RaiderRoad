using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for spring traps. These traps lie on the ground and catapult any enemies off the back of the RV.
/// </summary>
public class SpringTrap : Trap
{
    private static Vector3 NORTH = new Vector3(0, 0, 1);
    private static Vector3 SOUTH = new Vector3(0, 0, -1);
    private static Vector3 EAST = new Vector3(1, 0, 0);
    private static Vector3 WEST = new Vector3(-1, 0, 0);

    /// <summary>
    /// The launch angle in degrees. Set in the editor
    /// </summary>
    public float launchAngle;

    /// <summary>
    /// The launch magnitude in force units (Newtons?). Set in the editor
    /// NOTE: testing shows this has to be huge
    /// </summary>
    public float launchMag;

    [SerializeField] private Vector3 direction = SOUTH;

    public override void OnStart()
    {
        base.OnStart();
        UpdateLine();
    }

    private void UpdateLine()
    {
        LineRenderer line = gameObject.GetComponentInChildren<LineRenderer>();
        line.enabled = isHolo;
        if (isHolo)
        {
            Vector3 myPos = gameObject.transform.localPosition;
            Vector3[] positions = { myPos, myPos + LaunchVector(launchAngle, 5) };
            line.SetPositions(positions);
        }
    }

    public override void OnBreak()
    {
        // Do nothing
    }

    public override bool CanTarget(GameObject target)
    {
        return Util.isEnemy(target) || Util.isPlayer(target);
    }

    public void SetDirection(Vector3 roughDir)
    {
        Vector3 unit = roughDir.normalized;
        Vector3 leader = NORTH;
        float minDist = Vector3.Distance(unit, leader);
        foreach(Vector3 vec3 in new Vector3[] { SOUTH, EAST, WEST })
        {
            float dist = Vector3.Distance(unit, vec3);
            if(dist < minDist)
            {
                leader = vec3;
                minDist = dist;
            }
        }
        direction = leader;
        UpdateLine();
    }

    /// <summary>
    /// On activation, the victim gets a large force applied to their rigidbody
    /// </summary>
    /// <param name="victim">The target to fling</param>
    public override void Activate(GameObject victim)
    {
        if (Util.isPlayer(victim))
        {
            Debug.Log("Flinging player...");
        }
        else
        {
            Debug.Log("Flinging enemy...");
        }
        Fling(victim, launchAngle, launchMag);
    }

    private void Fling(GameObject victim, float angle, float mag)
    {
        victim.GetComponent<Rigidbody>().AddForce(LaunchVector(angle, mag));
        //victim.GetComponent<Rigidbody>().AddForce(Vector3.forward*1000000000f);
    }

    private Vector3 LaunchVector(float angle, float mag)
    {
        angle *= Mathf.Deg2Rad;
        Vector3 res = Mathf.Cos(angle) * direction;
        res.y = Mathf.Sin(angle);
        return res * mag;
    }
}
