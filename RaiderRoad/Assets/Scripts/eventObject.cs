using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventObject : MonoBehaviour {

	private int difficulty;
    [SerializeField]
    private GameObject originCluster;

    public void setCluster(GameObject cluster)
    {
        originCluster = cluster;
        //Debug.Log("added to cluster" + originCluster);
    }
	
	public void setDifficulty(int _diff) {
		difficulty = _diff;
	}

	public int getDifficulty() {
		return difficulty;
	}

    void OnDestroy()
    {
        Debug.Log("gone");
        if(originCluster != null)
        {
            originCluster.GetComponent<EventCluster>().updatePercent();
        }
        else
        {
            Debug.LogWarning("Vehicle had no origin cluster!");
        }
    }
}
