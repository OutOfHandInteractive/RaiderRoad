using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects whether there are enemies at the RV's sides.
/// This is used for pushing enemies against the walls and preventing the RV from going through these same walls.
/// This was done entirely as a response to the game not being designed with a physics first mentality, so physics are hand simulated and values get clamped.
/// This is all handled through couting enemies through OnTriggerEnter and OnTriggerExit
/// </summary>
public class VehicleDetection : MonoBehaviour
{
    // Michael H
    //-------- public variables -----------
    public GameObject steeringWheel;
    public enum Side {left, right};
    public Side side;

    //-------- private variables ----------
    private Driving DS;

    private void Start()
    {
        DS = steeringWheel.GetComponent<Driving>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "eVehicle")
        {
            if(side == Side.left)
            {
                DS.enemyCountL++;
            }
            else if(side == Side.right)
            {
                DS.enemyCountR++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "eVehicle")
        {
            if (side == Side.left)
            {
                DS.enemyCountL--;
            }
            else if (side == Side.right)
            {
                DS.enemyCountR--;
            }
        }
    }
}
