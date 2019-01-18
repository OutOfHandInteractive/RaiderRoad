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

	// ------------------- Unity Functions ---------------------
	private void Start() {
		currentHealth = maxHealth;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals("Obstacle")) {
			Debug.Log("You Hit an Obstacle");
			takeDamage(1);
			Instantiate(collision, other.gameObject.transform.position, Quaternion.identity, gameObject.transform);
			Destroy(other.gameObject);
		}
	}

	// -------------------- Getters and Setters --------------------
	public void takeDamage(float damage) {
        GameManager g = GameManager.GameManagerInstance;
        currentHealth -= damage;
        g.updateRVHealth(currentHealth);
        if (currentHealth <= 0f)
        {
            g.LossGame();
        }
	}

	public float getHealth() {
		return currentHealth;
	}

    private void damagePOI(float damage)
    {
        GameObject[] engines = GameObject.FindGameObjectsWithTag("Engine");

        for (int i = 0; i > 2; i++)
        {
            for (int j = 0; j > engines.Length; j++)
            {
                Engine POIscript = engines[j].GetComponent<Engine>();
                if (POIscript.myNode == poiNode[i])
                {
                    POIscript.durability -= damage;
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
 *  -
 *  
 */
