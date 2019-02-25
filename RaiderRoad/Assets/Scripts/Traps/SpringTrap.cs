using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for spring traps. These traps lie on the ground and catapult any enemies off the back of the RV.
/// </summary>
public class SpringTrap : Trap
{
    private static Quaternion NORTHQ = Quaternion.Euler(0, 0, 0);
    private static Quaternion EASTQ = Quaternion.Euler(0, 90, 0);
    private static Quaternion SOUTHQ = Quaternion.Euler(0, 180, 0);
    private static Quaternion WESTQ = Quaternion.Euler(0, 270, 0);

    private static Vector3 NORTH_V = new Vector3(0, 0, 1);
    private static Vector3 SOUTH_V = new Vector3(0, 0, -1);
    private static Vector3 EAST_V = new Vector3(1, 0, 0);
    private static Vector3 WEST_V = new Vector3(-1, 0, 0);

    /// <summary>
    /// The launch angle in degrees. Set in the editor
    /// </summary>
    public float launchAngle;

    /// <summary>
    /// The launch magnitude in force units (Newtons?). Set in the editor
    /// NOTE: testing shows this has to be huge
    /// </summary>
    public float launchMag;
    
    private GameObject holoPlayer = null;
    private LineRenderer line;
    private Vector3 direction = NORTH_V;
    [SerializeField] private float lineLength;
    [SerializeField] private bool showLine;

    public override void OnStart()
    {
        base.OnStart();
        UpdateLine();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        UpdateLine();
    }

    private void UpdateLine()
    {
        if(line == null)
        {
            line = gameObject.GetComponentInChildren<LineRenderer>();
        }
        line.enabled = isHolo || showLine;
        if (line.enabled)
        {
            if (holoPlayer != null)
            {
                SetDirection(holoPlayer);
            }
            Vector3 myPos = gameObject.transform.localPosition;
            Vector3[] positions = { myPos, myPos + LaunchVector(NORTH_V, launchAngle, lineLength) };
            line.SetPositions(positions);
        }
    }

    public override void OnBreak()
    {
        // Do nothing
    }

    /// <summary>
    /// SpringTraps can target players and enemies
    /// </summary>
    /// <param name="target">The target to test</param>
    /// <returns>True if and only if the target is a player or an enemy</returns>
    public override bool CanTarget(GameObject target)
    {
        return Util.isEnemy(target) || Util.isPlayer(target);
    }

    /// <summary>
    /// Sets the rotation of this trap to roughly match that of the given object (usually a player).
    /// The rotation will be "snapped" to the nearest of the four cardinal directions.
    /// Additionally, if this is a hologram, a reference to this object will be stored and the rotation will be updated each frame.
    /// </summary>
    /// <param name="player">The object to match in rotation</param>
    public void SetRotation(GameObject player)
    {
        if (isHolo)
        {
            holoPlayer = player;
            UpdateLine();
        }
        else
        {
            SetDirection(player);
        }
    }

    private void SetDirection(GameObject obj)
    {
        SetDirection(obj.transform.rotation);
    }

    private void SetDirection(Quaternion roughDir)
    {
        Quaternion leader = NORTHQ;
        Vector3 leader_v = NORTH_V;
        float minAngle = Quaternion.Angle(leader, roughDir);
        Quaternion[] dirs = { SOUTHQ, EASTQ, WESTQ };
        Vector3[] vecs = { SOUTH_V, EAST_V, WEST_V };
        for(int i=0; i<dirs.Length; i++)
        {
            Quaternion dir = dirs[i];
            float angle = Quaternion.Angle(dir, roughDir);
            if(angle < minAngle)
            {
                leader = dir;
                leader_v = vecs[i];
                minAngle = angle;
            }
        }
        gameObject.transform.rotation = leader;
        direction = leader_v;
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
        victim.GetComponent<Rigidbody>().AddForce(LaunchVector(direction, angle, mag));
        //victim.GetComponent<Rigidbody>().AddForce(Vector3.forward*1000000000f);
    }

    private Vector3 LaunchVector(Vector3 dir, float angle, float mag)
    {
        angle *= Mathf.Deg2Rad;
        Vector3 res = dir * Mathf.Cos(angle);
        res.y = Mathf.Sin(angle);
        return res * mag;
    }
}
