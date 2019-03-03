using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for weapons like cannons and flamethrowers
/// </summary>
public class Weapon : ConstructableGen<BuildNode> {

    /// <summary>
    /// List of nodes close around the weapon that should be disabled so as not to cause overlap. Populated automatically
    /// </summary>
    public List<GameObject> disabledNodes = new List<GameObject>();

	private Interactable interactableWeapon;


    private GameObject myAttacker = null;

    public override void OnStart()
    {
		interactableWeapon = GetComponentInChildren<Interactable>();
    }

    public override void OnUpdate()
    {
        // Do nothing
    }

    /// <summary>
    /// Hook to detach an interacting player and spawn particles
    /// </summary>
    public override void OnBreak()
    {
        if (myAttacker.GetComponent<PlayerController_Rewired>() != null)
        {
            myAttacker.GetComponent<PlayerController_Rewired>().clearInteractable();
        }

		// have player drop weapon so they don't get stuck
		interactableWeapon.Leave();
	}

    /// <summary>
    /// Extension to Damage() that takes a source
    /// </summary>
    /// <param name="damage">The damage to take</param>
    /// <param name="attackingObj">The source of the damage</param>
    public void Damage(float damage, GameObject attackingObj)
    {
        Damage(damage);
        myAttacker = attackingObj;
    }

    /// <summary>
    /// Detects and disables nearby nodes so they can't have overlapping weapons
    /// </summary>
    public void DisableNear(){
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 10, Quaternion.LookRotation(gameObject.transform.forward));
        //Debug.Log(hitColliders.Length.ToString());
        foreach(Collider c in hitColliders){
            if(c.name == "xNode")
            {
                BuildNode hit = c.GetComponent<BuildNode>();
                if (hit.canPlaceWeapon){
                    disabledNodes.Add(hit.gameObject);
                    hit.canPlaceWeapon = false;
                    //Debug.Log("Removed ability to place weapon");
                }
            }
            
        }
        //Debug.Log("count: " + disabledNodes.Count);
    }

    private void OnDestroy()
    {
        foreach(GameObject node in disabledNodes)
        {
            node.GetComponent<BuildNode>().canPlaceWeapon = true;
        }
    }
    /*
private void OnDrawGizmosSelected()
{
   Gizmos.color = Color.red;
   Gizmos.DrawWireCube(transform.position, transform.localScale);
}
*/
}
