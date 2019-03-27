using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackEnemy : EnemyAIState {

    private GameObject fireFX;
    private GameObject cMunnitions;
    private GameObject proj;
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

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        master.getAnimator().Running = false;
        eVehicle = master.Vehicle;
        cMunnitions = master.munnitions;
        fireFX = master.fire;
        if (eVehicle.GetComponentInChildren<HasWeapon>().gameObject.tag == "Cannon")
        {
            Transform getCannon = eVehicle.GetComponentInChildren<cannon>().transform.Find("model");
            Transform cannonBody = null;    // this is BAD
            foreach (Transform child in getCannon)
            {
                if (child.CompareTag("WeaponMount"))
                {
                    cannonBody = child.transform;
                }
            }
            cannon = cannonBody.gameObject;

            foreach (Transform child in cannon.transform)
            {
                if (child.CompareTag("WeaponBarrel"))
                {
                    barrel = child.gameObject;
                }
            }
        }
        else if (eVehicle.GetComponentInChildren<HasWeapon>().gameObject.tag == "Fire")
        {
            flamer = eVehicle.GetComponentInChildren<flamethrower>();
            Transform getFlamethrower = eVehicle.GetComponentInChildren<flamethrower>().transform.Find("FlameThrowerWrapper").Find("model"); // this is WORSE
            Transform flamethrower = null;
            foreach (Transform child in getFlamethrower)
            {
                if (child.CompareTag("WeaponMount"))
                {
                    flamethrower = child.transform;
                }
            }
            flamethrowerBody = flamethrower.gameObject;

            foreach (Transform child in flamethrowerBody.transform)
            {
                if (child.CompareTag("WeaponBarrel"))
                {
                    barrel = child.gameObject;
                }
            }


            if (!created)
            {
                if (master.Side == VehicleAI.Side.Right)
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

    public override void UpdateState()
    {
        Transform parent = eVehicle.GetComponentInChildren<HasWeapon>().transform;
        Debug.Log(parent);
        if(parent == null)
        {
            master.EnterDeath();
            return;
        }
        transform.position = parent.position;
        LookAtPlayer();
        if (parent.tag == Constants.CANNON_TAG)
        {
            Debug.Log("SHOOOOOOOOOOOOOOOOOOOT CANNNNNNNNNNNNON");
            if (!fired)
            {
                StartCoroutine(WaitToShoot());
                fired = true;
            }
        }
        else if (parent.tag == Constants.FIRE_TAG)
        {
            Debug.Log("SHOOOOOOOOOOOOOOOOOOOT FIREEEEEEEEEEEEEEEE");
            Flames();
        }
    }

    void CannonShoot()
    {
        GameObject player = ClosestLivingPlayer();
        Vector3 dir = (player.transform.position - barrel.transform.position);
        dir.y = 0.0f;
        Vector3 cannonBallSpeed = dir.normalized * 10f;
        proj = Instantiate(cMunnitions.gameObject, barrel.transform.position, Quaternion.identity);
        //proj.GetComponent<Rigidbody>().velocity = CannonVelocity(player, 75f);
        proj.GetComponent<Rigidbody>().velocity = cannonBallSpeed;
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

    private void LookAtPlayer()
    {
        GameObject weapons = getWeapon();
        GameObject player = ClosestLivingPlayer();
        if(player != null)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, weapons.transform.position.y, player.transform.position.z);
            gameObject.transform.LookAt(targetPosition);
            weapons.transform.LookAt(targetPosition);
        }
    }

    public override Color StateColor()
    {
        return Color.gray;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Weapon;
    }
}
