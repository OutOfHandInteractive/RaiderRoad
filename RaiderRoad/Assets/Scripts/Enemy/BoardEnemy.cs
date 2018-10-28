using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoardEnemy : JumpEnemy {

    //enemy, rigidbody,rv, angle to jump, if enemy jumped, chance to take action, current side 
    private int action = Random.Range(0, 100);


    public override void StartJump(GameObject enemy, Rigidbody rb, string side)
    {
        base.StartJump(enemy, rb, side);
        //Set rv, enemy, rigidbody, current side, and angle to jump


    }

    private Vector3 GetTarget(Vector3 planePos)
    {
        Transform floor = GameObject.Find("FloorWhole").transform;
        Transform closest = null;
        float minDist = 1 / 0f;
        foreach (Transform tile in floor)
        {
            Vector3 tilePos = new Vector3(tile.position.x, 0, tile.position.z);
            float dist = Vector3.Distance(tilePos, planePos);
            if (closest == null || dist < minDist)
            {
                closest = tile;
                minDist = dist;
            }
        }
        return closest.position;
    }

    public void Board()
    {
        //RV destination position
        Vector3 planePos = new Vector3(cObject.transform.position.x, 0, cObject.transform.position.z);
        Vector3 pos = GetTarget(planePos);
        float zSign = cSide.Equals("left") ? 1 : -1;

        Jump(pos, zSign);

        //50% chance to go into Destroy State or Fight State
        string actionStr = (action < 50) ? "EnterDestroy" : "EnterFight";
        EnemyAI ai = cObject.GetComponent<EnemyAI>();
        if(action < 50)
        {
            ai.EnterDestroy();
        }
        else
        {
            ai.EnterFight();
        }
        
    }
}
