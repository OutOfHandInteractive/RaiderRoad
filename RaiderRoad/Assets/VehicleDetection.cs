using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDetection : MonoBehaviour
{
    public GameObject steeringWheel;
    public enum Side {left, right};
    public Side side;

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
