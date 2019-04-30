using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleSpawner : MonoBehaviour
{

    public float startDelay = 30f;
    [SerializeField]
    private float obstDelay;
    //these two numbers can be tweaked later, but for now they prove the functionality
    private int obstLow = 17;
    private int obstHigh = 23;
    [SerializeField]
    private GameObject RV;
    [SerializeField]
    private GameObject smallObstacle;
    private float upperBound = 21f;
    private float lowerBound = -21f;
    private float spawnDistance = 120f;
    private float spawnRange = 15f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startup());
    }

    IEnumerator startup()
    {
        yield return new WaitForSeconds(startDelay);
        //start spawning obstacles
        while (true)
        {
            oSpawn();
            setObstDelay();
            yield return new WaitForSeconds(obstDelay);
        }
    }

    private void setObstDelay()
    {
        obstDelay = Random.Range(obstLow,obstHigh);
    }

    public void oSpawn()
    {
        //Debug.Log("oSpawn called");
        //GameObject obstacle = new GameObject();
        //int randNum = UnityEngine.Random.Range(0,1);
        //if(randNum == 0){   //small obstacle
            //obstacle = smallObstacle;
        //}
        //else{
            //obstacle = largeObstacle;
        //}

        float RVpos = RV.transform.position.x;
        float lo = RVpos - spawnRange;
        float hi = RVpos + spawnRange;

        if(lo <= lowerBound){
            lo = lowerBound;
        }else if (hi >= upperBound){
            hi = upperBound;
        }
        Vector3 spawnPoint = new Vector3(Random.Range(lo,hi),1f,spawnDistance);
        if (smallObstacle != null)
        {
            GameObject newObstacle = Instantiate(smallObstacle, spawnPoint, Quaternion.identity);    /////need obstacle prefab
            newObstacle.transform.Rotate(0f, -90f, 0f);    //kinda a bullshit fix for now - i'll explain and fix better at testing
            //newObstacle.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
            
        }
        else
        {
            Debug.LogError("No obstacle object assigned! Did someone fuck up the prefab?");
        }
    }
}
