﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StayVehicle : MonoBehaviour {

    private List<Transform> attackList;
    private GameObject WallsRV;
    private GameObject attackPosition;
    private int loadPoints = 0;
    private NavMeshAgent cEnemy;
    private GameObject cObject;
    private GameObject player;
    private VehicleAI.Side cSide;
    private bool calledRadio = false;
    private bool leave = false;
    public void StartStay(NavMeshAgent agent, GameObject enemy, VehicleAI.Side side, int stickPoint)
    {
        cEnemy = agent;
        cObject = enemy;
        attackList = new List<Transform>();
        cSide = side;
        //Find random attack point
        if (side == VehicleAI.Side.Left)
        {
            WallsRV = GameObject.Find("NodesLeft");
            attackPosition = GameObject.Find("AttackLeft");
        }
        else
        {
            WallsRV = GameObject.Find("NodesRight");
            attackPosition = GameObject.Find("AttackRight");
        }

        //Get all building points
        foreach (Transform child in WallsRV.transform)
        {
            attackList.Add(child);
        }
        if (attackList.Count == 0)
            return;
        loadPoints = stickPoint;
    }

    public GameObject GetObject()
    {
        return cObject;
    }

    public VehicleAI.Side Side()
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
        cEnemy.radius = .1f;
        Debug.Log("STAY STATE");
        //Stop completely when next to spot
        //cEnemy.autoBraking = true;

        //Randomly choose to load left or right side

        //Go to loading area
        //cEnemy.SetDestination(attackList[loadPoints].position);

        float time = Mathf.SmoothStep(0, 1, 4 * Time.deltaTime);
        //Have agent go to different points
        /*cEnemy.SetDestination(patrols[wanderPoints].position);
        //Choose random patrol point
        */
        cObject.transform.position = Vector3.Lerp(cObject.transform.position, attackList[loadPoints].position, time);



        GameObject[] extantEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i=0;i<extantEnemies.Length;i++)
        {
            if(extantEnemies[i].transform.root.tag != "RV")
            {
                StartCoroutine(waitFailSafe());
            }
            if((extantEnemies.Length <= 0 || extantEnemies[i].transform.root.tag == "eVehicle") && (extantEnemies[i].GetComponent<StatefulEnemyAI>().GetState() == StatefulEnemyAI.State.Escape))
            {
                leave = true;
            }
        }
        if (extantEnemies.Length <= 0)
        {
            leave = true;
        }
        else if(Vector3.Distance(cObject.transform.position, attackList[loadPoints].position) < 1f)
        {
            cObject.transform.position = attackList[loadPoints].transform.position;
            //cEnemy.GetComponent<NavMeshAgent>().isStopped = true;
            Debug.Log("TRUE");
            // At loading area
            if (!calledRadio)
            {
                if(this == null)
                {
                }
                Debug.Log("Calling radio: " + this.ToString());
                Radio.GetRadio().ReadyForEvac(this);
                calledRadio = true;
            }
            else if(CountEnemiesOnBoard() >= System.Math.Min(5, extantEnemies.Length)) //TODO this limit should depend on size of vehicle
            {
                Debug.Log("All loaded up and ready to go!");
                leave = true;
            }
            if (leave)
            {
                Debug.Log("LEAVING");
                if (calledRadio)
                {
                    Radio.GetRadio().EvacLeaving(this);
                }
                StartCoroutine(waitToLeave());
            }
            //Debug.Log("Waiting for enemies");
        }
        if (leave)
        {
            Debug.Log("LEAVING");
            if (calledRadio)
            {
                Radio.GetRadio().EvacLeaving(this);
            }
            StartCoroutine(waitToLeave());
        }

    }

    IEnumerator waitToLeave()
    {
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterWander();
        yield return new WaitForSeconds(5);
        cObject.GetComponent<VehicleAI>().EnterLeave();

    }

    IEnumerator waitFailSafe()
    {
        yield return new WaitForSeconds(20);
        leave = true;
    }
}
