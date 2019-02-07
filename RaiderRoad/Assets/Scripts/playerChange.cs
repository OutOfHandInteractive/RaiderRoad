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
    private bool occupied = false;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player") && count <= 0.0f && !occupied) 
		{
            player = other.gameObject;
            player.GetComponentInChildren<PlayerController_Rewired>().myAni.SetFloat("speed", 0);
            int id = player.GetComponent<PlayerController_Rewired>().GetId();
            transform.parent.gameObject.GetComponent<carscript2>().SetId(id);
            player.transform.position = transform.position;
            view = player.transform.Find("View").gameObject;
			rv.GetComponent<carscript2> ().enabled = true;
			rv.GetComponent<SimpleCarScript> ().enabled = true;
			player.GetComponent<PlayerController_Rewired> ().enabled = false;
			view.GetComponent<PlayerPlacement_Rewired> ().enabled = false;
			view.GetComponent<BoxCollider> ().enabled = false;

            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            occupied = true;
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
		rv.GetComponent<carscript2> ().enabled = false;
		rv.GetComponent<SimpleCarScript> ().enabled = false;
		player.GetComponent<PlayerController_Rewired> ().enabled = true;
		view.GetComponent<PlayerPlacement_Rewired> ().enabled = true;
		view.GetComponent<BoxCollider> ().enabled = true;

        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        occupied = false;
    }
}
