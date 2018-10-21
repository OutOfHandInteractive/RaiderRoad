using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEnemy : MonoBehaviour {
    //Current game object
    private GameObject cObject;
   
    //Set enemy to this script
    public void StartWait(GameObject enemy)
    {
        cObject = enemy;
    }

    public void Wait()
    {
        //Enter board state after 15 seconds
        cObject.GetComponent<EnemyAI>().Invoke("EnterBoard", 15f);
    }
}
