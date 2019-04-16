using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleSpawner : MonoBehaviour
{

    public float startDelay = 30f;
    public float obstDelay = 23f;
    [SerializeField]
    private GameObject obstacle;
    public GameObject oSpawnsParent;
    [SerializeField]
    private List<Transform> ospawnPoints;
<<<<<<< HEAD
<<<<<<< HEAD
    private int startPos = 2;   //initial "lane" the RV starts in
    [SerializeField]
    private int rvPos = 2;  //default "lane" index
    private int upperBound;
    private int lowerBound;
=======
>>>>>>> parent of ce0b938... framework for limiting
=======
>>>>>>> parent of ce0b938... framework for limiting

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in oSpawnsParent.transform)      //get obstacle spawn points
        {
            //Debug.Log(child);
            ospawnPoints.Add(child);
        }
        StartCoroutine(startup());

    }

    IEnumerator startup()
    {
        yield return new WaitForSeconds(startDelay);
<<<<<<< HEAD
        //start spawning obstacles
        while (true)
        {
            oSpawn();
            yield return new WaitForSeconds(obstDelay);
        }
=======
        StartCoroutine(obstacleSpawn());
    }

    IEnumerator obstacleSpawn()
    {
        oSpawn();
        yield return new WaitForSeconds(obstDelay);    
>>>>>>> parent of ce0b938... framework for limiting
    }

    public void oSpawn()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        //Debug.Log("oSpawn called");
        //GameObject obstacle = new GameObject();
        //int randNum = UnityEngine.Random.Range(0,1);
        //if(randNum == 0){   //small obstacle
            //obstacle = smallObstacle;
        //}
        //else{
            //obstacle = largeObstacle;
        //}
        
        if(rvPos == 0){
            //i = Random.Range(rvPos,rvPos + 1);
            upperBound = rvPos + 1;
            lowerBound = rvPos;
        }else if(rvPos == 4){
            //i = Random.Range(rvPos - 1, rvPos);
            upperBound = rvPos;
            lowerBound = rvPos - 1;
        }else{
            //i = Random.Range(rvPos - 1,rvPos + 1); // ospawnPoints.Count);        
            upperBound = rvPos + 1;
            lowerBound = rvPos - 1;
        }
        int i = Random.Range(lowerBound, upperBound);
        Debug.Log("i = "+ i);
=======
        int i = Random.Range(0, ospawnPoints.Count);
>>>>>>> parent of ce0b938... framework for limiting
        Vector3 spawnPoint = ospawnPoints[i].transform.position;
        if (obstacle != null)
        {
<<<<<<< HEAD
            GameObject newObstacle = Instantiate(smallObstacle, spawnPoint, Quaternion.identity);    /////need obstacle prefab
            newObstacle.transform.Rotate(0f, -90f, 0f);    //kinda a bullshit fix for now - i'll explain and fix better at testing
            //newObstacle.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
            
=======
            GameObject newObstacle = Instantiate(obstacle, spawnPoint, Quaternion.identity);    /////need obstacle prefab
            newObstacle.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
            newObstacle.transform.Rotate(0f, 90f, 0f);    //kinda a bullshit fix for now - i'll explain and fix better at testing
>>>>>>> parent of ce0b938... framework for limiting
=======
        int i = Random.Range(0, ospawnPoints.Count);
        Vector3 spawnPoint = ospawnPoints[i].transform.position;
        if (obstacle != null)
        {
            GameObject newObstacle = Instantiate(obstacle, spawnPoint, Quaternion.identity);    /////need obstacle prefab
            newObstacle.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
            newObstacle.transform.Rotate(0f, 90f, 0f);    //kinda a bullshit fix for now - i'll explain and fix better at testing
>>>>>>> parent of ce0b938... framework for limiting
        }
        else
        {
            Debug.LogError("No obstacle object assigned! Did someone fuck up the prefab?");
        }
    }
}
