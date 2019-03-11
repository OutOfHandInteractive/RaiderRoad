using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for cannonballs fired by Raiders
/// </summary>
public class enemyCannonball : AbstractCannonball
{
    
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
        if(Util.isPlayer(collision.gameObject))
        {
            collision.gameObject.GetComponent<PlayerController_Rewired>().takeDamage(cannonDamage);
            Explode();
        }
        else if(collision.gameObject.tag == "road" || Util.IsRV(collision.gameObject))
        {
            Explode();
        }
    }
}
