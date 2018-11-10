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
	private float vertSpeed;
	private float timeRemaining;

	// Projectile motion
	private const float ANGLE = 45;
	private const float G = -9.81f;
	private float timeElapsed;
	private float oldY;

	// Update is called once per frame
	void Update () {
		transform.position += horizVelocity * Time.deltaTime;
		timeRemaining -= Time.deltaTime;
		timeElapsed = travelTime - timeRemaining;

		//transform.position += new Vector3(0f, deltaY(), 0f);

		if(timeRemaining <= 0) {
			Destroy(gameObject);
		}
	}

	public void launch(Vector3 _target, Vector3 _source) {
		target = _target;
		source = _source;
		dir = target - source;
		horizVelocity = dir / travelTime;
		vertSpeed = horizVelocity.magnitude * Mathf.Tan(ANGLE);

		timeRemaining = travelTime;
	}

	public float deltaY() {
		Debug.Log(vertSpeed);

		return (float)(source.y + (vertSpeed * timeElapsed) + (0.5 * G * Mathf.Pow(timeElapsed, 2))) - transform.position.y;
	}

	public float getMaxRange(float h) {
		return (muzzleVelocity / G) * Mathf.Sqrt(Mathf.Pow(muzzleVelocity, 2) + (2 * G * h));
	}
}
