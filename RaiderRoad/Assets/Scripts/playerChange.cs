using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerChange : MonoBehaviour {

	public GameObject[] players;
	public GameObject steeringwheel;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			players [0].GetComponent<carscript2> ().enabled = true;
			players [0].GetComponent<SimpleCarScript> ().enabled = true;
			players [1].GetComponent<PlayerController_Rewired> ().enabled = false;
			players [2].GetComponent<PlayerPlacement_Rewired> ().enabled = false;
			players [2].GetComponent<BoxCollider> ().enabled = false;
		}
	}
		
//	void Update () 
//	{
//		if (Input.GetKeyDown ("space")) 
//		{
//
//			//steeringwheel.SetActive (false);
//		}
//	}

	public void exitSteering ()
	{
		players [0].GetComponent<carscript2> ().enabled = false;
		players [0].GetComponent<SimpleCarScript> ().enabled = false;
		players [1].GetComponent<PlayerController_Rewired> ().enabled = true;
		players [2].GetComponent<PlayerPlacement_Rewired> ().enabled = true;
		players [2].GetComponent<BoxCollider> ().enabled = true;
	}
}
