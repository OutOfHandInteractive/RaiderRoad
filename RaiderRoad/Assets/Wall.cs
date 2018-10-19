using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public GameObject drop;
    //hits is for destroying by hand to remove an ill placed wall
    //health is the durability from attacks by raiders
    public int hits;
    public float health;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(hits <= 0 || health <= 0)
        {
            spawn();
        }
	}

    void spawn()
    {
        Instantiate(drop, new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), Quaternion.identity);
        Destroy(this.gameObject);
    }
}
