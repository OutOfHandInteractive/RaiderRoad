using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

    public GameObject drop;
    //hits is for destroying by hand to remove an ill placed wall
    //health is the durability from attacks by raiders
    public int hits;
    public float breakHealth;

    //Health for obstacles/raider vehicles on the RV
    public float durability;

    public bool isHolo = false;
    private Material myMat; //reference material of gameObject
    public GameObject myNode; //node it spawned from

    // Use this for initialization
    void Start()
    {
        myMat = gameObject.GetComponent<Renderer>().material;
        if (isHolo) MakeHolo();
    }

    // Update is called once per frame
    void Update()
    {
        if (hits <= 0 || breakHealth <= 0) //can probably move this outside update
        {
            spawnDrop();
        }
    }

    void spawnDrop()
    {
        GameObject item = Instantiate(drop, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        item.name = "Engine Drop";
        myNode.GetComponent<PoiNode>().occupied = false; // set node to unoccupied again
        Destroy(this.gameObject);
    }

    public void Damage(float damage)
    {
        breakHealth -= damage;
    }

    void MakeHolo() // a function for making wall material holographic
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Color tempColor = myMat.color;
        tempColor.a = 0.4f;
        myMat.color = tempColor;
    }
}