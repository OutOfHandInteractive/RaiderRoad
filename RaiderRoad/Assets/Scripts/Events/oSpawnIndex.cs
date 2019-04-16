using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oSpawnIndex : MonoBehaviour
{
    public int index;
    public GameObject manager;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "RV")
        {
            Debug.Log("collision detected");
            manager.GetComponent<obstacleSpawner>().rvIndex(index);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
