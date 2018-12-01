using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject drop;
    //hits is for destroying by hand to remove an ill placed wall
    //health is the durability from attacks by raiders
    public int hits;
    public float health;

    public bool isHolo = false;
    private Material myMat; //reference material of gameObject
    public GameObject myNode; //node it spawned from

    public List<GameObject> disabledNodes = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        myMat = gameObject.GetComponentInChildren<Renderer>().material;
        if (isHolo) MakeHolo();
    }

    // Update is called once per frame
    void Update()
    {
        if (hits <= 0 || health <= 0)
        {
            spawnDrop();
        }
    }

    void spawnDrop()
    {
        GameObject item = Instantiate(drop, new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), Quaternion.identity);
        item.name = drop.name;
        if(myNode.GetComponent<BuildNode>().occupied == true)
        {
            myNode.GetComponent<BuildNode>().occupied = false; // set node to unoccupied again
        }
        Destroy(this.gameObject);
    }

    public void Damage(float damage)
    {
        health -= damage;
    }

    void MakeHolo() // a function for making wall material holographic
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Color tempColor = myMat.color;
        tempColor.a = 0.4f;
        myMat.color = tempColor;
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
