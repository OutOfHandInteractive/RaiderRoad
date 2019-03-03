using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for enemies posted on vehicle weapons.
/// </summary>
public class WeaponAttackEnemy : EnemyAI {

    private GameObject fireFX;
    private GameObject cMunnitions;
    private GameObject proj;
    private GameObject cObject;
    private VehicleAI eVehicle;
    private GameObject cannon;
    private GameObject barrel;
    private flamethrower flamer;
	private bool isFiringFlamethrower = false;
    private GameObject flamethrowerBody;
    private bool fired = false;
    private bool firing = false;
    private ParticleSystem fireInstance;
    private bool created = false;

    /// <summary>
    /// Initializes this state
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="vehicle"></param>
    /// <param name="munnitions"></param>
    /// <param name="fire"></param>
    /// <param name="side"></param>
    public void StartWeapon(GameObject enemy, VehicleAI vehicle, GameObject munnitions, GameObject fire, string side)
    {
        cObject = enemy;
        eVehicle = vehicle;
        cMunnitions = munnitions;
        fireFX = fire;
        if (vehicle.GetComponentInChildren<HasWeapon>().gameObject.tag == "Cannon")
        {
			Transform getCannon = eVehicle.GetComponentInChildren<cannon>().transform.Find("model");
			Transform cannonBody = null;	// this is BAD
			foreach (Transform child in getCannon) {
				if (child.CompareTag("WeaponMount")) {
					cannonBody = child.transform;
				}
			}
            cannon = cannonBody.gameObject;

			foreach (Transform child in cannon.transform) {
				if (child.CompareTag("WeaponBarrel")) {
					barrel = child.gameObject;
				}
			}
        }
        else if (vehicle.GetComponentInChildren<HasWeapon>().gameObject.tag == "Fire")
        {
            flamer = eVehicle.GetComponentInChildren<flamethrower>();
			Transform getFlamethrower = eVehicle.GetComponentInChildren<flamethrower>().transform.Find("FlameThrowerWrapper").Find("model"); // this is WORSE
			Transform flamethrower = null;
			foreach (Transform child in getFlamethrower) {
				if (child.CompareTag("WeaponMount")) {
					flamethrower = child.transform;
				}
			}
			flamethrowerBody = flamethrower.gameObject;

			foreach (Transform child in flamethrowerBody.transform) {
				if (child.CompareTag("WeaponBarrel")) {
					barrel = child.gameObject;
				}
			}


			if (!created)
            {
                if (side.Equals("right"))
                {
                    fireInstance = Object.Instantiate(fireFX, barrel.transform.position, fireFX.transform.rotation, barrel.transform).GetComponent<ParticleSystem>();
                }
                else
                {
                    fireInstance = barrel.GetComponentInChildren<ParticleSystem>();
                }
                //fireInstance.transform.rotation = 
                fireInstance.Stop();
                created = true;
            }

        }
    }

    /// <summary>
    /// Perform the Weapon actions
    /// </summary>
    public void Weapon()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);

        //flamethrower.transform.LookAt(player.transform.position);

        Transform parent = eVehicle.GetComponentInChildren<HasWeapon>().transform;
        Debug.Log(parent);
        if(parent == null)
        {
            gameObject.GetComponent<StatefulEnemyAI>().EnterDeath();
        }
        else if (parent.tag == "Cannon")
        {
            Debug.Log("SHOOOOOOOOOOOOOOOOOOOT CANNNNNNNNNNNNON");
            if (!fired)
            {
                StartCoroutine(WaitToShoot());
                fired = true;
            }
        }
        else if (parent.tag == "Fire")
        {
            Debug.Log("SHOOOOOOOOOOOOOOOOOOOT FIREEEEEEEEEEEEEEEE");
            Flames();
        }


    }

    void CannonShoot()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);
        proj = Instantiate(cMunnitions.gameObject, barrel.transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody>().velocity = CannonVelocity(player, 75f);
    }

    IEnumerator WaitToShoot()
    {
        CannonShoot();
        yield return new WaitForSeconds(3f);
        fired = false;
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
        //fireInstance.gameObject.transform.rotation = barrel.transform.rotation;
        Debug.Log(barrel);
        Debug.Log(flamer);
        flamer.SetRotation(barrel.transform.rotation);
        flamer.CheckOverheat();
        //flamer.GetComponentInChildren<flamethrowerDamage>().enabled = false;
        if (!flamer.isOverheated() && !isFiringFlamethrower)	// yeah we cant do this. should probably be more like if there is a player in range
        {
            flamer.StartFiringEnemy();
			isFiringFlamethrower = true;
            //flamer.GetComponentInChildren<flamethrowerDamage>().enabled = true;
        }
		else if (flamer.isOverheated() && isFiringFlamethrower) {
			isFiringFlamethrower = false;
		}
    }

    public GameObject getWeapon()
    {
        if (cannon != null)
        {
            return cannon;
        }
        else if (flamethrowerBody != null)
        {
            return flamethrowerBody;
        }
        return null;
    }

    /// <summary>
    /// Find and target the nearest player. Rotates the weapon to face them.
    /// </summary>
    /// <param name="weapons"></param>
    public void LookAtPlayer(GameObject weapons)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);
        if(player != null)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, weapons.transform.position.y, player.transform.position.z);
            cObject.transform.LookAt(targetPosition);
            weapons.transform.LookAt(targetPosition);
        }
    }
}
