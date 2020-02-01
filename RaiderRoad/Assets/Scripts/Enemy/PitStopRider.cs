using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PitStopRider : MonoBehaviour
{

    private StatefulEnemyAI ai;
    private NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        ai = GetComponent<StatefulEnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(ai.GetState() != StatefulEnemyAI.State.Fight &&  GameObject.FindGameObjectsWithTag(Constants.PLAYER_TAG).Length > 0)
        {
            ai.EnterFight();
        }
    }
}
