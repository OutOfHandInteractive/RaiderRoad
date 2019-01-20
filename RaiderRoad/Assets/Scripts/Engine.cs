using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : DurableConstruct<PoiNode> {

    //Health for obstacles/raider vehicles on the RV
    public Transform myHealthTrans;

    public override void OnStart()
    {
        base.OnStart();
        UpdateHealthBar();
    }

    //Durability functions
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

}