using UnityEngine;
using System.Collections;

/// <summary>
/// Non-generic superclass for all "Constructable" game objects (walls, traps, weapons, etc.).
/// Implementers should use the genericized Constructable<> subclass, passing in the specific BuildNode type for the object.
/// This class exists for purposes of fetching constructables with GameObject.GetComponent<>() due to C#'s retarded Generics system that doesn't allow Java style Wildcards.
/// </summary>
public abstract class Constructable : MonoBehaviour
{
	// -------------- public variables ------------------
	// references

    /// <summary>
    /// The itemdrop that corresponds to this object. Usually set in the editor
    /// </summary>
    public GameObject drop;

    /// <summary>
    /// The node this object was spawned from
    /// </summary>
	public GameObject myNode;

	// gameplay values

    /// <summary>
    /// The number of hits it takes to destroy this (Deprecated)
    /// </summary>
	public int hits;

    /// <summary>
    /// The amount of health this object has
    /// </summary>
	public float health;

    /// <summary>
    /// Boolean flag that indicates whether this object is merely a hologram
    /// </summary>
	public bool isHolo = false;

    /// <summary>
    /// Boolean flag that indicates whether this object is currently being attacked by a raider
    /// </summary>
    //public bool isOccupied = false;

	// -------------- nonpublic variables ----------------
	[SerializeField] protected ParticleSystem objectBreakParticles;

    // Use this for initialization
    void Start()
    {
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
            BreakParticles();
            spawnDrop();
        }
    }

    /// <summary>
    /// This hook method is called in Start() after initialization
    /// </summary>
    public abstract void OnStart();

    /// <summary>
    /// This hook method is called in Update() before health is checked
    /// </summary>
    public abstract void OnUpdate();

    /// <summary>
    /// This hook method is called in Update() if the object is going to be destroyed. It is called just before the drop is spawned.
    /// </summary>
	public abstract void OnBreak();

    /// <summary>
    /// This method is for taking damage from attacks. The damage is subtracted from the health.
    /// </summary>
    /// <param name="damage">The damage to take</param>
    public virtual void Damage(float damage)
    {
        health -= damage;
    }

    /// <summary>
    /// Checks if the object has been placed on a node
    /// </summary>
    /// <returns>True if and only if myNode is valid and occupied</returns>
    public bool isPlaced()
    {
        return myNode != null && GetNodeComp(myNode).occupied;
    }

    /// <summary>
    /// Optional hook that is called with the drop that has been spawned. This allows the implementer to add information to the drop. Does nothing by default.
    /// </summary>
    /// <param name="item">THe drop object</param>
    public virtual void OnDrop(GameObject item)
    {
        // Do nothing by default
    }

    protected virtual void BreakParticles()
    {
        Instantiate(objectBreakParticles, transform.position, Quaternion.identity);
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

    /// <summary>
    /// Abstract method that gets the build node component out of the given game object
    /// </summary>
    /// <param name="myNode">The object to extract the component from</param>
    /// <returns>The build node component</returns>
    protected abstract AbstractBuildNode GetNodeComp(GameObject myNode);

    private void MakeHolo() // a function for making material holographic
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        foreach(Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            if(! (renderer is LineRenderer))
            {
                Material myMat = renderer.material;
                Color tempColor = myMat.color;
                tempColor.a = 0.4f;
                myMat.color = tempColor;
            }
        }
    }
}

/// <summary>
/// This is the proper generic class for constructables. Provides greater type safety than the other class.
/// </summary>
/// <typeparam name="N">The type of build node this object needs</typeparam>
public abstract class ConstructableGen<N> : Constructable where N : AbstractBuildNode
{
    /// <summary>
    /// Uses GetComponent<> to get the correct component from the object.
    /// </summary>
    /// <param name="myNode">The object to look in</param>
    /// <returns>The correct build node of type N</returns>
    protected override AbstractBuildNode GetNodeComp(GameObject myNode)
    {
        return myNode.GetComponent<N>();
    }
}
