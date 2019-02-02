using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemy : MonoBehaviour {

    public void Death(GameObject enemy, GameObject drop)
    {
        spawnDrop(drop, enemy);
        stealDrop(enemy);
        Destroy(enemy);
    }

    void spawnDrop(GameObject drop, GameObject enemy)
    {
        Debug.Log("Wall dropped!");
        GameObject item = Instantiate(drop, new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z), Quaternion.identity);
        item.name = "Wall Drop";
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
