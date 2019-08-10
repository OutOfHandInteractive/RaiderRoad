using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class GasInteract : Interactable
{
    // ----------------------
    // Public variables
    // ----------------------
    public List<Transform> batteries = new List<Transform>();
    public float chargeTime;
    public Text chargeTimeText;

    // ----------------------
    // Private variables
    // ----------------------
    private bool interacting;
    private bool charging;
    private float counter = 0.0f;
    private LineRenderer lr;


    void Start()
    {
        inUse = false;
        user = null;
        userPlayerId = -1;
        cooldownTimer = cooldown;

        interacting = false;
        charging = false;

        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);

    }

    void Update()
    {
        if (interacting)
        {
            lr.SetPosition(1, user.gameObject.transform.position);
        }

        if (charging)
        {
            if (counter > 0.0f)
            {
                counter -= Time.deltaTime;
            }
            else
            {
                counter = 0.0f;
                charging = false;
                batteries.RemoveAt(0);

                Leave();
            }
        }

        chargeTimeText.text = counter.ToString("0.0");

        if (batteries.Count == 0)
        {
            chargeTimeText.text = "You Win";
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("ChargeBox"))
        {
            charging = true;
            counter = chargeTime;
            lr.SetPosition(1, batteries[0].position);

            transform.SetParent(null);
            transform.position = lr.GetPosition(0);

            interacting = false;

        }
    }

    // ------------------- Interaction Methods ---------------------

    public override void Interact(PlayerController_Rewired player)
    {
        if (!inUse)
        {
            user = player;
            inUse = true;
            userPlayerId = user.playerId;
            interacting = true;

            transform.SetParent(user.transform);
        }
    }

    public override void Leave()
    {
        if (user != null)
        {
            cooldownTimer = cooldown;
            user.unsetInteractingFlag();
            user = null;
            userPlayerId = -1;

            interacting = false;

            transform.SetParent(null);
            transform.position = lr.GetPosition(0);
        }

        if (!charging)
        {
            inUse = false;
            lr.SetPosition(1, transform.position);
        }
    }

    public override bool Occupied()
    {
        return inUse;
    }
}
