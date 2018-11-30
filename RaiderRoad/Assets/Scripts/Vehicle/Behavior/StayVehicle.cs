using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StayVehicle : MonoBehaviour {

    private List<Transform> load;
    private int loadPoints = 0;
    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private GameObject player;
    private string cSide;
    private bool calledRadio = false;
    public void StartStay(NavMeshAgent agent, GameObject enemy, string side)
    {
        cEnemy = agent;
        cObject = enemy;
        load = new List<Transform>();
        cSide = side;
        cEnemy.speed = 6f;
        if (side.Equals("left"))
        {
            player = GameObject.Find("EnemyLload");
        }
        else
        {
            player = GameObject.Find("EnemyRload");
        }
        foreach (Transform child in player.transform)
        {
            load.Add(child);
        }
    }

    public GameObject GetObject()
    {
        return cObject;
    }

    public string Side()
    {
        return cSide;
    }

    private int CountEnemiesOnBoard()
    {
        int res = 0;
        for(int i=0; i<cObject.transform.childCount; i++)
        {
            GameObject obj = cObject.transform.GetChild(i).gameObject;
            if(obj.tag == "Enemy")
            {
                res++;
            }
        }
        return res;
    }

    public override string ToString()
    {
        return "StayVehicle";
    }

    public void Stay()
    {
        //Stop completely when next to spot
        //cEnemy.autoBraking = true;
        if (load.Count == 0)
            return;
        //Randomly choose to load left or right side

        //Go to loading area
        cEnemy.SetDestination(load[loadPoints].position);

        loadPoints = Random.Range(0, load.Count);

        bool leave = false;
        int extantEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (extantEnemies <= 0)
        {
            leave = true;
        }
        else if(cEnemy.remainingDistance < 0.5f)
        {
            // At loading area
            if (!calledRadio)
            {
                if(this == null)
                {
                    Debug.Log("This shit is retarded");
                }
                Debug.Log("Calling radio: " + this.ToString());
                Radio.GetRadio().ReadyForEvac(this);
                calledRadio = true;
            }
            else if(CountEnemiesOnBoard() >= System.Math.Min(5, extantEnemies)) //TODO this limit should depend on size of vehicle
            {
                Debug.Log("All loaded up and ready to go!");
                leave = true;
            }
            //Debug.Log("Waiting for enemies");
        }
        if (leave)
        {
            if (calledRadio)
            {
                Radio.GetRadio().EvacLeaving(this);
            }
            StartCoroutine(waitToLeave());
        }
    }

    IEnumerator waitToLeave()
    {
        cObject.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterLeave();

    }
}
