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

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in oSpawnsParent.transform)      //get obstacle spawn points
        {
            //Debug.Log(child);
            ospawnPoints.Add(child);
        }
        StartCoroutine(startupDelay());

    }

    IEnumerator startupDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(obstacleSpawn());
    }

    IEnumerator obstacleSpawn()
    {
        oSpawn();
        yield return new WaitForSeconds(obstDelay);    
    }

    public void oSpawn()
    {
        int i = Random.Range(0, ospawnPoints.Count);
        Vector3 spawnPoint = ospawnPoints[i].transform.position;
        if (obstacle != null)
        {
            GameObject newObstacle = Instantiate(obstacle, spawnPoint, Quaternion.identity);    /////need obstacle prefab
            newObstacle.GetComponentInChildren<eventObject>().setCluster(this.gameObject);
            newObstacle.transform.Rotate(0f, 90f, 0f);    //kinda a bullshit fix for now - i'll explain and fix better at testing
        }
        else
        {
            Debug.LogError("No obstacle object assigned! Did someone fuck up the prefab?");
        }
    }
}
