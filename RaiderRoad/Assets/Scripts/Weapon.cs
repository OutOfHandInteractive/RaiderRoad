using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Constructable<BuildNode> {

    public List<GameObject> disabledNodes = new List<GameObject>();

    private GameObject myAttacker = null;

    public override void OnStart()
    {
        // Do nothing
    }

    public override void OnUpdate()
    {
        // Do nothing
    }

    public override void OnBreak()
    {
        //Debug.Log(myAttacker.GetComponent<PlayerController_Rewired>() + "jdsfijdfidsfjdiofjdsifds");
        if (myAttacker.GetComponent<PlayerController_Rewired>() != null)
        {
            myAttacker.GetComponent<PlayerController_Rewired>().clearInteractable();
        }
    }

    public void Damage(float damage, GameObject attackingObj)
    {
        Damage(damage);
        myAttacker = attackingObj;
    }

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
