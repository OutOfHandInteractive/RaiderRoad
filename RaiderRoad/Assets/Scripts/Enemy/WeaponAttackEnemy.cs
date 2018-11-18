using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackEnemy : AbstractEnemyAI {

    private GameObject cMunnitions;
    private GameObject proj;
    private GameObject cObject;
    private VehicleAI eVehicle;
    private GameObject cannon;
    private GameObject barrel;
    private GameObject flame;
    private GameObject flamethrower;
    private bool fired = false;
    public void StartWeapon(GameObject enemy, VehicleAI vehicle, GameObject munnitions)
    {
        cObject = enemy;
        eVehicle = vehicle;
        cMunnitions = munnitions;
        if (cObject.transform.parent.tag == "Cannon")
        {
            cannon = GameObject.Find("Cannon_Body");
            barrel = GameObject.Find("Barrel");
        }
        else if (cObject.transform.parent.tag == "Fire")
        {
            flamethrower = GameObject.Find("FlameThrower_Body");
            flame = flamethrower.transform.GetChild(3).gameObject;
        }
    }
	
	// Update is called once per frame
	public void Weapon () {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);
        
        //flamethrower.transform.LookAt(player.transform.position);


        if (cObject.transform.parent.tag == "Cannon")
        {
            cannon.transform.LookAt(player.transform.position);
            if (!fired)
            {
                CannonShoot();
                fired = true;
            }
        }
        else if (cObject.transform.parent.tag == "Fire")
        {
            flamethrower.transform.LookAt(player.transform.position);
            Flames();
        }


    }

    void CannonShoot()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);
        proj = Instantiate(cMunnitions.gameObject, barrel.transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody>().velocity = CannonVelocity(player, 75f);
        Destroy(proj, 3f);
    }

    Vector3 CannonVelocity(GameObject player, float angle)
    {

        var dir = player.transform.position - barrel.transform.position;  // get target direction
        var h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        var dist = dir.magnitude;  // get horizontal distance
        var a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
                                   // calculate the velocity magnitude
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }

    void Flames()
    {
        flame.SetActive(true);
    }
}
