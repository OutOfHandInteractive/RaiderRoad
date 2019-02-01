﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Non-generic superclass for all "Constructable" game objects (walls, traps, weapons, etc.).
/// Implementers should use the genericized Constructable<> subclass, passing in the specific BuildNode type for the object.
/// This class exists for purposes of fetching constructables with GameObject.GetComponent<>() due to C#'s retarded Generics system that doesn't allow Java style Wildcards.
/// </summary>
public abstract class Constructable : MonoBehaviour
{
    public GameObject drop;
    //hits is for destroying by hand to remove an ill placed wall
    //health is the durability from attacks by raiders
    public int hits;
    public float health;

    public bool isHolo = false;
    private Material myMat; //reference material of gameObject
    public GameObject myNode; //node it spawned from

    // Use this for initialization
    void Start()
    {
        myMat = gameObject.GetComponent<Renderer>().material;
        if (isHolo) MakeHolo();
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
        if (hits <= 0 || health <= 0) //can probably move this outside update
        {
            OnBreak();
            spawnDrop();
        }
    }

    public abstract void OnStart();

    public abstract void OnUpdate();

    public abstract void OnBreak();

    public virtual void Damage(float damage)
    {
        health -= damage;
    }

    public virtual void OnDrop(GameObject item)
    {
        // Do nothing by default
    }

    private void spawnDrop()
    {
        GameObject item = Instantiate(drop, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        item.name = drop.name;
        OnDrop(item);
        if(myNode != null)
        {
            // set node to unoccupied again
            GetNodeComp(myNode).occupied = false;
        }
        Destroy(this.gameObject);
    }

    protected abstract AbstractBuildNode GetNodeComp(GameObject myNode);

    private void MakeHolo() // a function for making material holographic
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Color tempColor = myMat.color;
        tempColor.a = 0.4f;
        myMat.color = tempColor;
    }
}

public abstract class Constructable<N> : Constructable where N : AbstractBuildNode
{
    protected override AbstractBuildNode GetNodeComp(GameObject myNode)
    {
        return myNode.GetComponent<N>();
    }
}