using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This enemy just wants to destroy things and pick fights
/// </summary>
public class DestroyEnemy : EnemyAI {
    //enemy, speed
    private GameObject cObject;
    private int action;
	private bool isDestroying = false;

    /// <summary>
    /// TODO: Ernest please explain this
    /// </summary>
    public bool engineKill = false;

    private GameObject[] walls;
    private GameObject[] engines;
    private GameObject wall;
    private GameObject engine;
    private NavMeshAgent agent;

    /// <summary>
    /// Initializes this state
    /// </summary>
    /// <param name="enemy">This enemy (Deprecated)</param>
    public void StartDestroy(GameObject enemy, NavMeshAgent _agent)
    {
        cObject = enemy;
        agent = _agent;
        action = Random.Range(0, 100);
        walls = GameObject.FindGameObjectsWithTag("Wall");
        engines = GameObject.FindGameObjectsWithTag("Engine");
        //Debug.Log(engines[0]);
        wall = Closest(cObject.transform.position, walls);
        engine = Closest(cObject.transform.position, engines);
        //Debug.Log(engine);
    }

    /// <summary>
    /// Performs the destroy actions
    /// </summary>
    public void Destroy() {
        //Set wall gameobject
        //Set movement speed of enemy
        float movement = speed * Time.deltaTime;

        if(cObject.GetComponent<StatefulEnemyAI>().getDamaged()) {
			cObject.GetComponent<StatefulEnemyAI>().ExitDestroyState();
			cObject.GetComponent<StatefulEnemyAI>().EnterFight();
        }

        //If there are no more walls, go to Fight state, else keep going for walls
        if (engineKill && cObject.GetComponent<lightEnemy>()) {
			cObject.GetComponent<StatefulEnemyAI>().ExitDestroyState();
			cObject.GetComponent<StatefulEnemyAI>().EnterSteal();
        }
        else if (engineKill && cObject.transform.parent != null) {
			cObject.GetComponent<StatefulEnemyAI>().ExitDestroyState();
			cObject.GetComponent<StatefulEnemyAI>().EnterFight();
        }
		else {
            //Find destroyable and go to it
            ChanceDestroy(walls, engines, movement);
        }
    }

    /// <summary>
    /// TODO Explain this too, please, Erndog
    /// </summary>
    /// <param name="walls"></param>
    /// <param name="engines"></param>
    /// <param name="movement"></param>
    public void ChanceDestroy(GameObject[] walls, GameObject[] engines, float movement) {
        if(action < 90) {
            if(walls.Length <= 0) {
                Debug.Log(engine);
                if(engines.Length <= 0 || engine == null) {
					cObject.GetComponent<StatefulEnemyAI>().ExitDestroyState();
					cObject.GetComponent<StatefulEnemyAI>().EnterFight();
                }
                else {
                    Debug.Log(engine);
                    agent.SetDestination(engine.transform.position);
                }
            }
            else {
				if (wall) {
					agent.SetDestination(wall.transform.position);
				}
				else {
					walls = GameObject.FindGameObjectsWithTag("Wall");
					wall = Closest(cObject.transform.position, walls);
				}
			}
        }
        else {
            if (engines.Length <= 0 || engine == null) {
				cObject.GetComponent<StatefulEnemyAI>().ExitDestroyState();
				cObject.GetComponent<StatefulEnemyAI>().EnterFight();
            }
            else {
                agent.SetDestination(engine.transform.position);
            }
        }
        cObject.GetComponent<StatefulEnemyAI>().getAnimator().SetBool("Running", true);
    }

	#region Getters and Setters
	public void SetIsDestroying(bool _isDestroying) {
		isDestroying = _isDestroying;
	}
	#endregion
}
