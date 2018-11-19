using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemy : MonoBehaviour {

    public void Death(GameObject enemy)
    {
        Destroy(enemy);
    }
}
