using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCannonball : MonoBehaviour {

    public float cannonDamage = 2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController_Rewired>().takeDamage(cannonDamage);
            Destroy(gameObject);
        }
    }
}
