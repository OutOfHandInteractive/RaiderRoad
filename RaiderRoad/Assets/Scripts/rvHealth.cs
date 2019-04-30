using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rvHealth : MonoBehaviour {

	//-------------------- Public Variables --------------------
	// references
	public ParticleSystem collision;

	// gameplay values
	public float maxHealth;

    public GameObject[] poiNode; //1 to 3 correspond to the spaces of the RV front to back

    // ------------------- Private Variables -------------------
    private float currentHealth;

    // Camera Shake
    private CameraShake vCamShake;

    // ------------------- Unity Functions ---------------------
    private void Start() {
		currentHealth = maxHealth;
        vCamShake = GameObject.FindGameObjectWithTag("MainVCam").GetComponent<CameraShake>();
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals("Obstacle")) {
            damagePOI(20f);
            Instantiate(collision, other.gameObject.transform.position, Quaternion.identity, gameObject.transform);
            Destroy(other.gameObject);

            /*Debug.Log("You Hit an Obstacle");
			takeDamage(1);
			Instantiate(collision, other.gameObject.transform.position, Quaternion.identity, gameObject.transform);
			Destroy(other.gameObject);*/
        }
	}

    // -------------------- Getters and Setters --------------------
    /* OLD Damage System
    public void takeDamage(float damage) {
        GameManager g = GameManager.GameManagerInstance;
        currentHealth -= damage;
        g.updateRVHealth(currentHealth);
        if (currentHealth <= 0f)
        {
            g.LossGame();
        }
	}*/

	public float getHealth() {
		return currentHealth;
	}

    public void damagePOI(float damage)
    {
        vCamShake.Shake(.5f, 10f, .5f);

        GameObject[] engines = GameObject.FindGameObjectsWithTag("Engine");
        Debug.Log("Loop Start");

        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j < engines.Length; j++)
            {
                Engine POIscript = engines[j].GetComponent<Engine>();
                if (POIscript.myNode == poiNode[i])
                {
                    //Debug.Log("You Hit an Obstacle");
                    POIscript.TakeRVDamage(damage);
                    i = 2; //break out of first loop
                    break;
                }
            } 
        }

        checkPOI();
    }

    private void checkPOI()
    {
        GameObject[] engines = GameObject.FindGameObjectsWithTag("Engine");
        if (engines.Length <= 0)
        {
            GameManager g = GameManager.GameManagerInstance;
            g.LossGame();
        }
    }
}

/*  _____________To Do:_____________
 *  - take damage and apply it to POI based on their nodes (search for engines, if its node matches node 1, apply damage, else move on to next 
 *  - after taking damage, check status of each engine (likely just checking if its still there); need to do same thing when it's broken by enemies or players (so make it public function)\
 *  - Give components internal HP
 *  - Display HP on parts
 *  - Pass durabiltiy to drop, then back to item when placed
 *  
 *  Assignment to new placable isn't working.
 */
