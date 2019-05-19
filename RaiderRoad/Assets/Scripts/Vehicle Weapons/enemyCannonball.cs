﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for cannonballs fired by Raiders
/// </summary>
public class enemyCannonball : AbstractCannonball
{    
    public float cannonDamage = 2f;
    public float splashRadius = 1.5f;
    public Renderer warning;
    public float upTime;
    public float downTime;

    private float cooldown = .1f;
    private bool canCollide = false;
    private bool isUp = true;
    private float warningCooldown = 0f;

    private void Update()
    {
        if (!canCollide)
        {
            if (cooldown > 0.0f)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                canCollide = true;
            }
        }

        // Flashing warning
        if (warningCooldown > 0f)
        {
            warningCooldown -= Time.deltaTime;
        }
        else
        {
            if (isUp)
            {
                warning.enabled = false;
                warningCooldown = downTime;
            }
            else
            {
                warning.enabled = true;
                warningCooldown = upTime;
            }
            isUp = !isUp;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if(Util.isPlayer(collision.gameObject) || collision.gameObject.tag == "wall" || collision.gameObject.tag == "floor" || Util.IsRV(collision.gameObject) || Util.isEnemy(collision.gameObject))
        if (canCollide)
        {
            //collision.gameObject.GetComponent<PlayerController_Rewired>().takeDamage(cannonDamage);
            Collider[] splashTargets = Physics.OverlapSphere(transform.position, splashRadius);
            List<GameObject> hits = new List<GameObject>();
            foreach (Collider target in splashTargets)
            {

                int layerMask = 1 << 10; // Ignore Layer NavMesh
                layerMask = ~layerMask;

                RaycastHit hit;
                Vector3 dir = (target.transform.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, dir, out hit, splashRadius, layerMask))
                {
                    Debug.Log(hit.collider + " and " + target);
                    if (hit.collider.gameObject == target.gameObject)
                    {
                        hits.Add(target.gameObject);
                    }
                }
            }
            foreach(GameObject target in hits)
            {
                float damage = cannonDamage * (1 - (Vector3.Distance(transform.position, target.transform.position) / splashRadius));
                if (Util.isPlayer(target))
                {
                    target.GetComponent<PlayerController_Rewired>().takeDamage(damage);
                }
                else
                {
                    Constructable constr = target.GetComponent<Constructable>();
                    if(constr != null)
                    {
                        constr.Damage(damage);
                    }
                }
            }
            Explode();
        }
    }
}
