using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEnemy : EnemyAIState {
    //Current game object
    private VehicleAI cVehicle;
    //Set enemy to this script

    protected override void OnEnter(StateContext context)
    {
        base.OnEnter(context);
        cVehicle = master.Vehicle;
    }

    public override void UpdateState()
    {
        if(gameObject != null) master.getAnimator().Running = false;
        if (gameObject.transform.parent != null)
        {
            if(gameObject.transform.parent.tag == "RV")
            {
                master.EnterFight();
            }
        }
        Debug.Log(cVehicle.getState());
        //Enter board state after 15 seconds
        if (cVehicle.getState() == VehicleAI.State.Stay)
        {
            Debug.Log("SYAAAAAAAAAAAAAAAAAAAAAAAAY");
            master.EnterBoard();
        }
    }

    public override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if (other.gameObject.tag == "EnemyInteract")
        {
            transform.parent = other.transform;
        }
    }

    public override Color StateColor()
    {
        return Color.white;
    }

    public override StatefulEnemyAI.State State()
    {
        return StatefulEnemyAI.State.Wait;
    }
}
