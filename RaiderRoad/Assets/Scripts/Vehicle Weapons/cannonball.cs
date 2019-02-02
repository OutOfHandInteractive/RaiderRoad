using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonball : MonoBehaviour {
	// ------------------- public variables --------------------
	// references
	public ParticleSystem explosion;

	// gameplay variables
	public float damage;
	public float splashDamage;
	public float splashRadius;
	public float travelTime;
	public float muzzleVelocity;

	// ------------------- private variables --------------------
	private Vector3 target;
	private Vector3 source;
	private Vector3 dir;
	private Vector3 horizVelocity;
	private float timeRemaining;

	// Projectile motion
	private float angle;
	private const float G = -9.81f;
	private float timeElapsed;
	private float xVel, yVel, zVel, xzVel;
	private float xzAng;
	private float oldY;

	// Update is called once per frame
	void Update () {
		timeRemaining -= Time.deltaTime;
		timeElapsed = travelTime - timeRemaining;

		transform.position += new Vector3(xVel*Time.deltaTime, deltaY(), zVel*Time.deltaTime);
        //Debug.Log("CAAAAAAAAAAAAAAAAAAAAAAAAAAAAANNON" + transform.position);
		if(timeRemaining <= 0) {
			Destroy(gameObject);
		}
	}

    private void OnTriggerEnter(Collider other) {
		GameObject directTarget = other.gameObject;
        Debug.Log("TAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAG" + other.gameObject.tag);


        if (directTarget.CompareTag("eVehicle") || directTarget.CompareTag("Destructable"))
        {
            directTarget.GetComponentInParent<VehicleAI>().takeDamage(damage);
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (directTarget.CompareTag("Enemy"))
        {
            directTarget.GetComponent<StatefulEnemyAI>().takeDamage(damage);
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (directTarget.CompareTag("road"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }


        Collider[] splashTargets = Physics.OverlapSphere(transform.position, splashRadius);

		for (int i = 0; i<splashTargets.Length; i++) {
			if (splashTargets[i].gameObject.CompareTag("eVehicle") && splashTargets[i] != other) {
				other.gameObject.GetComponentInParent<VehicleAI>().takeDamage(splashDamage);
			}
			else if (splashTargets[i].gameObject.CompareTag("Enemy") && splashTargets[i] != other) {
				other.gameObject.GetComponent<StatefulEnemyAI>().takeDamage(splashDamage);
			}
		}
	}


	#region Physics
	public void launch(Vector3 _target, Vector3 _source, Vector3 launchDir) {
		target = _target;
		source = _source;
		dir = target - source;

		// need to fix x/z velocity
		angle = angleOfReach(_target, _source);

		// facing to the right
		if (launchDir.x > 0) {
			xzAng = Mathf.Atan(dir.z / dir.x);
			xzVel = muzzleVelocity * Mathf.Cos(angle * Mathf.Deg2Rad);
			xVel = xzVel * Mathf.Cos(xzAng);
		}
		else {	// facing to the left
			xzAng = -Mathf.Atan(dir.z / dir.x);	// flip angle so it doesn't shoot backward
			xzVel = muzzleVelocity * Mathf.Cos(angle * Mathf.Deg2Rad);
			xVel = -xzVel * Mathf.Cos(xzAng); // make its x velocity negative
		}
		
		zVel = xzVel * Mathf.Sin(xzAng);
		yVel = muzzleVelocity * Mathf.Sin(angle * Mathf.Deg2Rad);

		timeRemaining = travelTime;
	}

	public float deltaY() {
		return (float)(source.y + (yVel * timeElapsed) + (0.5 * G * Mathf.Pow(timeElapsed, 2))) - transform.position.y;
	}

	public float angleOfReach(Vector3 _target, Vector3 _source) {
		// supposed to use positive G, but the constant is set to negative to make stuff more intuitive. can change if needed
		return (float)(0.5 * Mathf.Asin((-G * Vector3.Distance(_source, _target)) / Mathf.Pow(muzzleVelocity, 2)) * Mathf.Rad2Deg);
	}

	public float getMaxRange() {
		return (Mathf.Pow(muzzleVelocity, 2) / -G);
	}
	#endregion
}
