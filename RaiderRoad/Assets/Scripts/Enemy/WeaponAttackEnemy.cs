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
    private flamethrower flamer;
    private GameObject flamethrowerBody;
    private bool fired = false;
    private bool firing = false;
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
            flamer = eVehicle.GetComponentInChildren<flamethrower>();
            Transform flameBody = flamer.transform.Find("FlamethrowerBodyWrapper");
            Debug.Log(flameBody);
            flamethrowerBody = flameBody.Find("FlameThrower_Body").gameObject;
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
                StartCoroutine(WaitToShoot());
                fired = true;
            }
        }
        else if (cObject.transform.parent.tag == "Fire")
        {
            /*
            if (!fired)
            {
                StartCoroutine(FlameCycle());
                fired = true;
                firing = true;
            }
            */
            Flames();
        }


    }

    void CannonShoot()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);
        proj = Object.Instantiate(cMunnitions.gameObject, barrel.transform.position, Quaternion.identity);
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

    IEnumerator FlameCycle()
    {
        Debug.Log("BUUUUUUURRRRNNNN!!!!");
        yield return new WaitForSeconds(flamer.overheatTime);
        Debug.Log("BARREL'S TOO HOT!!");
        firing = false;
        yield return new WaitForSeconds(flamer.overheatCooldown);
        fired = false;
    }

    void Flames()
    {
        //fireInstance.gameObject.transform.rotation = barrel.transform.rotation;
        flamer.SetRotation(barrel.transform.rotation);
        flamer.CheckOverheat();
        if (!flamer.isOverheated())
        {
            flamer.StartFiring();
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

    public void LookAtPlayer(GameObject weapons)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Closest(cObject.transform.position, players);
        Vector3 targetPosition = new Vector3(player.transform.position.x, weapons.transform.position.y, player.transform.position.z);
        cObject.transform.LookAt(targetPosition);
        weapons.transform.LookAt(targetPosition);
    }
}
