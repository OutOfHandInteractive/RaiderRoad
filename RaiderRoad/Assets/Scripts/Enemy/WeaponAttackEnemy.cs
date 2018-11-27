using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackEnemy : EnemyAI {

    private GameObject fireFX;
    private GameObject cMunnitions;
    private GameObject proj;
    private GameObject cObject;
    private VehicleAI eVehicle;
    private GameObject cannon;
    private GameObject barrel;
    private GameObject flamethrower;
    private bool fired = false;
    private ParticleSystem fireInstance;
    private bool created = false;
    public void StartWeapon(GameObject enemy, VehicleAI vehicle, GameObject munnitions, GameObject fire, string side)
    {
        cObject = enemy;
        eVehicle = vehicle;
        cMunnitions = munnitions;
        fireFX = fire;
        if (cObject.transform.parent.tag == "Cannon")
        {
            Transform cannonBody = eVehicle.GetComponentInChildren<cannon>().transform.Find("Cannon_Body");
            cannon = cannonBody.gameObject;
            barrel = cannonBody.Find("Barrel").gameObject;
        }
        else if (cObject.transform.parent.tag == "Fire")
        {
            Transform flameBody = eVehicle.GetComponentInChildren<flamethrower>().transform.Find("FlamethrowerBodyWrapper");
            Debug.Log(flameBody);
            flamethrower = flameBody.Find("FlameThrower_Body").gameObject;
            barrel = flameBody.Find("FlameThrower_Body").gameObject.transform.Find("Barrel").gameObject;
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

    // Update is called once per frame
    public void Weapon()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);

        //flamethrower.transform.LookAt(player.transform.position);

        if (cObject.transform.parent.tag == "Cannon")
        {
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
        proj = Object.Instantiate(cMunnitions.gameObject, barrel.transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody>().velocity = CannonVelocity(player, 75f);
        Object.Destroy(proj, 3f);
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
        fireInstance.gameObject.transform.rotation = barrel.transform.rotation;
        fireInstance.Play();
    }

    public GameObject getWeapon()
    {
        if (cannon != null)
        {
            return cannon;
        }
        else if (flamethrower != null)
        {
            return flamethrower;
        }
        return null;
    }

    public void LookAtPlayer(GameObject weapons)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);
        weapons.transform.LookAt(player.transform.position);
    }
}
