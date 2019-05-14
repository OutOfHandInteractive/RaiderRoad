using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This enemy just wants to die
/// </summary>
public class DeathEnemy : MonoBehaviour {

    /// <summary>
    /// Performs the raider death actions (ritual?)
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="drop"></param>
    public void Death(GameObject enemy, GameObject drop, ParticleSystem fx)
    {
        spawnDrop(drop, enemy);
        stealDrop(enemy);
		Instantiate(fx.gameObject, transform.position, Quaternion.identity);
        Destroy(enemy);
    }

    void spawnDrop(GameObject drop, GameObject enemy)
    {
        Debug.Log("Wall dropped!");
        if(transform.parent != null) //REALLY SHOULDN'T STAY THIS WAY IMO
        {
            GameObject item = Instantiate(drop, new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z), 
                Quaternion.identity, transform.parent.transform);
            item.name = "Wall Drop";
        }
    }
    void stealDrop(GameObject enemy)
    {
        foreach (Transform child in enemy.transform)
        {
            if (child.tag == "Drops")
            {
                child.parent = null;
            }
        }
    }
}
