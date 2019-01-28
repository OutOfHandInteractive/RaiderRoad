using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoardEnemy : JumpEnemy {

    //enemy, rigidbody,rv, angle to jump, if enemy jumped, chance to take action, current side 
    private int action;


    public override void StartJump(GameObject enemy, Rigidbody rb, string side)
    {
        base.StartJump(enemy, rb, side);
        action = Random.Range(0, 100);
        //Set rv, enemy, rigidbody, current side, and angle to jump


    }

    private Vector3 GetTarget(Vector3 planePos)
    {
        return Closest(planePos, GameObject.FindGameObjectsWithTag("floor")).transform.position;
    }

    public void Board()
    {
        //RV destination position
        Vector3 planePos = new Vector3(cObject.transform.position.x, 0, cObject.transform.position.z);
        Vector3 pos = GetTarget(planePos);
        float zSign = cSide.Equals("left") ? 1 : -1;

        Jump(pos, zSign);

        //40% chance to go into Destroy State or Fight State, 20% to go into steal
        string actionStr = (action < 50) ? "EnterDestroy" : "EnterFight";
        StatefulEnemyAI ai = cObject.GetComponent<StatefulEnemyAI>();
        if(cObject.transform.parent.tag == "RV")
        {
            if (action < 40)
            {
                ai.EnterDestroy();
            }
            else if (action > 40 && action < 80)
            {
                ai.EnterFight();
            }
            else
            {
                ai.EnterSteal();
            }
        }

        
    }
}
