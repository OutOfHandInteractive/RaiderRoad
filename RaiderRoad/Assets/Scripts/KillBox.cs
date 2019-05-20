using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    #region System Functions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController_Rewired playerController = collision.gameObject.GetComponent<PlayerController_Rewired>();
            playerController.RoadRash();
        }
    }
    #endregion
}
