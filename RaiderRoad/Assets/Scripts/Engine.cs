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
    public bool isOccupied = false;
    /// <summary>
    /// Start() hook that just initializes the health bar
    /// </summary>
    public override void OnStart()
    {
        base.OnStart();
        UpdateHealthBar();
    }

    //Durability functions

    /// <summary>
    /// Takes RV damage as durability damage and updates the health bar
    /// </summary>
    /// <param name="damage"></param>
    public void TakeRVDamage(float damage)
    {
        DurabilityDamage(damage);
        //Debug.Log("Engine Health" + currDur);
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        myHealthTrans.localScale = new Vector3(currDur / durability, 1f, 1f); //reflect on health bar
    }

    public override void OnBreak()
    {
        // Nothing
    }

    public override void OnUpdate()
    {
        // Nothing
    }
}