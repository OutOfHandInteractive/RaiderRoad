using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for Engine parts
/// </summary>
public class Engine : DurableConstructGen<PoiNode> {

    /// <summary>
    /// Transform for the health bar for obstacles/raider vehicles on the RV
    /// </summary>
    public Transform myHealthTrans;
    private Vector3 myHealthScale;
    public bool isOccupied = false;
    public ParticleSystem sparks;
    /// <summary>
    /// Start() hook that just initializes the health bar
    /// </summary>
    public override void OnStart()
    {
        base.OnStart();
        myHealthScale = myHealthTrans.localScale;
        UpdateHealthBar();
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        // tell POI node that battery was hit
        myNode.GetComponent<PoiNode>().PoiHit();
    }

    //Durability functions

    /// <summary>
    /// Takes RV damage as durability damage and updates the health bar
    /// </summary>
    /// <param name="damage"></param>
    public void TakeRVDamage(float damage)
    {
        DurabilityDamage(damage);
        sparks.Play(true);
        //Debug.Log("Engine Health" + currDur);
        UpdateHealthBar();

        // tell POI node that battery was hit
        if(currDur > 0f) {
            myNode.GetComponent<PoiNode>().PoiHit();
        } else {
            myNode.GetComponent<PoiNode>().PoiMissing();
        }
    }
    
    //heal durability by set value
    public void EngineHeal(float heal)
    {
        currDur += heal;
        if (currDur > durability) {
            currDur = durability;
        }
        CheckDur();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        myHealthTrans.localScale = new Vector3 (myHealthScale.x, myHealthScale.y, (currDur / durability) * myHealthScale.z); //reflect on health bar (multiplied by original health scale)
    }

    public override void OnBreak()
    {
        myNode.GetComponent<PoiNode>().PoiMissing();
    }

    public override void CheckDur()
    {
        if (isPlaced() && currDur <= 0f)
        {
            myNode.GetComponent<PoiNode>().PoiMissing();
        }

        base.CheckDur();
    }

    public override void OnUpdate()
    {
        // Nothing
    }

}