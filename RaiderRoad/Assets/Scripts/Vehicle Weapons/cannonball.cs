using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonball : MonoBehaviour {

	public float damage;
	public float travelTime;

	private Vector3 target;
	private Vector3 source;
	private Vector3 dir;
	private Vector3 horizVelocity;
	private float timeRemaining;
	
	// Update is called once per frame
	void Update () {
		transform.position += horizVelocity * Time.deltaTime;
		timeRemaining -= Time.deltaTime;

		if(timeRemaining <= 0) {
			Destroy(gameObject);
		}
	}

	public void launch(Vector3 _target, Vector3 _source) {
		target = _target;
		source = _source;
		dir = target - source;
		horizVelocity = dir / travelTime;

		timeRemaining = travelTime;
	}
}
