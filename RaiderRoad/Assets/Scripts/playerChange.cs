using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerChange : MonoBehaviour {

	public GameObject rv;
    private GameObject player;
    private GameObject view;
	public GameObject steeringwheel;
    public float cooldown = 3.0f;
    private float count = 0.0f;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player") && count <= 0.0f) 
		{
            gameObject.GetComponent<BoxCollider>().enabled = false;
            player = other.gameObject;
            player.transform.position = transform.position;
            view = player.transform.Find("View").gameObject;
            rv.GetComponent<carscript2> ().enabled = true;
			rv.GetComponent<SimpleCarScript> ().enabled = true;
			other.GetComponent<PlayerController_Rewired> ().enabled = false;
            view.GetComponent<PlayerPlacement_Rewired> ().enabled = false;
			view.GetComponent<BoxCollider> ().enabled = false;
		}
	}

    void Update()
    {
        if (count > 0.0f)
        {
            count -= Time.deltaTime;
        }
    }

    public void exitSteering ()
	{
        count = cooldown;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        rv.GetComponent<carscript2> ().enabled = false;
		rv.GetComponent<SimpleCarScript> ().enabled = false;
		player.GetComponent<PlayerController_Rewired> ().enabled = true;
		view.GetComponent<PlayerPlacement_Rewired> ().enabled = true;
		view.GetComponent<BoxCollider> ().enabled = true;
	}
}
