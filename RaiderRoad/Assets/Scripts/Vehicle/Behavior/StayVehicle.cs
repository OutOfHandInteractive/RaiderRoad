using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StayVehicle {

    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private Transform player;
    private string cSide;
    private bool calledRadio = false;
    public void StartStay(NavMeshAgent agent, GameObject enemy)
    {
        cEnemy = agent;
        cObject = enemy;
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

    public void Stay(string side)
    {
        cSide = side;
        //Stop completely when next to spot
        cEnemy.autoBraking = true;

        //Randomly choose to load left or right side
        if (side.Equals("left"))
        {
            player = GameObject.FindGameObjectWithTag("EnemyL").transform;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("EnemyR").transform;
        }
        //Go to loading area
        cEnemy.SetDestination(player.transform.position);

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
            cObject.GetComponent<VehicleAI>().EnterLeave();
        }
    }
}
