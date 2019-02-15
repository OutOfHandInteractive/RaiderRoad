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
        originCluster.GetComponent<EventCluster>().updatePercent();
    }
}
