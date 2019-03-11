using UnityEngine;
using System.Collections;

/// <summary>
/// This is a superclass for cannonball types. Unites common functionality between the two types of cannonball
/// </summary>
public abstract class AbstractCannonball : MonoBehaviour
{
    public ParticleSystem explosion;
    public GameObject selfDestruct;

    /// <summary>
    /// Spawns particles and sound effects then destroys this object
    /// </summary>
    protected void Explode()
    {
        Instantiate(selfDestruct, transform.position, Quaternion.identity);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
