using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonball : MonoBehaviour {
	
	public float damage;
	public float travelTime;
	public float muzzleVelocity;

	private Vector3 target;
	private Vector3 source;
	private Vector3 dir;
	private Vector3 horizVelocity;
	private float timeRemaining;

	// Projectile motion
	public float angle;
	private const float G = -9.81f;
	private float timeElapsed;
	public float xVel, yVel, zVel, xzVel;
	public float xzAng;
	private float oldY;

	// Update is called once per frame
	void Update () {
		timeRemaining -= Time.deltaTime;
		timeElapsed = travelTime - timeRemaining;

		//transform.position += new Vector3(xVel*Time.deltaTime, deltaY(), zVel*Time.deltaTime);

		if(timeRemaining <= 0) {
			Destroy(gameObject);
		}
	}

	public void launch(Vector3 _target, Vector3 _source) {
		target = _target;
		source = _source;
		dir = target - source;

		// need to fix x/z velocity
		angle = angleOfReach(_target, _source);
		xzAng = Mathf.Atan(dir.z / dir.x);
		xzVel = muzzleVelocity * Mathf.Cos(angle * Mathf.Deg2Rad);
		xVel = xzVel * Mathf.Cos(xzAng);
		zVel = xzVel * Mathf.Sin(xzAng);
		Debug.Log(angle);
		yVel = muzzleVelocity * Mathf.Sin(angle * Mathf.Deg2Rad);

		timeRemaining = travelTime;
	}

	public float deltaY() {
		Debug.Log(yVel);

		return (float)(source.y + (yVel * timeElapsed) + (0.5 * G * Mathf.Pow(timeElapsed, 2))) - transform.position.y;
	}

	public float angleOfReach(Vector3 _target, Vector3 _source) {
		// supposed to use positive G, but the constant is set to negative to make stuff more intuitive. can change if needed
		return (float)(0.5 * Mathf.Asin((-G * Vector3.Distance(_source, _target)) / Mathf.Pow(muzzleVelocity, 2)) * Mathf.Rad2Deg);
	}

	public float getMaxRange() {
		return (Mathf.Pow(muzzleVelocity, 2) / -G);
	}
}
