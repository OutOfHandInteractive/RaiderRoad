using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarScript : MonoBehaviour {

	public GameObject frontleft;
	public GameObject frontright;
	public GameObject backleft;
	public GameObject backright;

	public List<AxleInfo> axleInfos;
	public float maxMotorTorque;
	public float maxSteeringAngle;

	[System.Serializable]
	public class AxleInfo
	{
		public WheelCollider leftWheel;
		public WheelCollider rightWheel;
		public bool motor;
		public bool steering;
	}
		
//	public void Update()
//	{
//		transform.Translate (Vector3.forward * 20 * Time.deltaTime);
//	}

	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) 
		{
			return;
		}

		Transform visualWheel = collider.transform.GetChild (0);

		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose (out position, out rotation);

		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
	}

	public void FixedUpdate()
	{
		float motor = maxMotorTorque + (1000 * gameObject.GetComponent<carscript2>().VerticalAxis());
		float steering = maxSteeringAngle * gameObject.GetComponent<carscript2>().HorizontalAxis();

		foreach (AxleInfo axleInfo in axleInfos) 
		{
			if (axleInfo.steering) 
			{
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) 
			{
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
			ApplyLocalPositionToVisuals (axleInfo.leftWheel);
			ApplyLocalPositionToVisuals (axleInfo.rightWheel);
		}
	}
}


