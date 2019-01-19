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
    public float durability; //original amount
    private float currDur = -1f; //current durability
    public Transform myHealthTrans;

    public bool isHolo = false;
    private Material myMat; //reference material of gameObject
    public GameObject myNode; //node it spawned from

    // Use this for initialization
    void Start()
    {
        myMat = gameObject.GetComponent<Renderer>().material;
        if (isHolo) MakeHolo();

        if(currDur < 0f) currDur = durability; //if current durability was not assigned before start, assume full health
        myHealthTrans.localScale = new Vector3(currDur / durability, 1f, 1f); //reflect on health bar
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
        //get the item in the drop (which will be a new engine), set it's durabilty to this
        item.GetComponent<ItemDrop>().myItemDur = currDur; //give new drop item correct durabilty

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

    //Durability functions
    public void TakeRVDamage(float damage)
    {
        currDur -= damage; //subtract damage
        //Debug.Log("Engine Health" + currDur);
        myHealthTrans.localScale = new Vector3(currDur / durability, 1f, 1f); //reflect on health bar

        if (currDur <= 0f)
        {
            myNode.GetComponent<PoiNode>().occupied = false; // set node to unoccupied again
            Destroy(gameObject);
        }
    }


    public void SetDurability(float newDur)
    {
        currDur = newDur;
    }

    public float GetDurability()
    {
        return currDur;
    }
}