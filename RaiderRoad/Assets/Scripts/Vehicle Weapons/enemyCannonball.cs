using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for cannonballs fired by Raiders
/// </summary>
public class enemyCannonball : AbstractCannonball
{    
    public float cannonDamage = 2f;
    public float splashRadius = 3f;
    private float cooldown = .1f;
    private bool canCollide = false;

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
    }

    void OnCollisionEnter(Collision collision)
    { 
        //if(Util.isPlayer(collision.gameObject) || collision.gameObject.tag == "wall" || collision.gameObject.tag == "floor" || Util.IsRV(collision.gameObject) || Util.isEnemy(collision.gameObject))
        if (canCollide)
        {
            //collision.gameObject.GetComponent<PlayerController_Rewired>().takeDamage(cannonDamage);
            Collider[] splashTargets = Physics.OverlapSphere(transform.position, splashRadius);
            foreach (Collider target in splashTargets)
            {
                int layerMask = 1 << 10; // Ignore Layer NavMesh
                layerMask = ~layerMask;

                RaycastHit hit;
                Vector3 dir = (target.transform.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.Log(hit.collider + " and " + target);
                    if (hit.collider == target)
                    {
                        if (Util.IsPlayer(target.gameObject))
                        {
                            target.gameObject.GetComponent<PlayerController_Rewired>().takeDamage(cannonDamage);
                        }
                        else if (target.tag == "wall")
                        {
                            target.GetComponent<Wall>().Damage(cannonDamage);
                        }
                        else if (target.tag == "weapon")
                        {
                            target.GetComponent<Weapon>().Damage(cannonDamage);
                        }
                    }
                }
            }
            Explode();
        }
    }
}
