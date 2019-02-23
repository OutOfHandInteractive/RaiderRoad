using UnityEngine;
using System.Collections;

/// <summary>
/// This is the parent class for all build nodes. Currently this only holds the bare minimum, but eventually we should unite the subclasses to reduce duplication.
/// </summary>
public abstract class AbstractBuildNode : MonoBehaviour
{
    /// <summary>
    /// Boolean flag that indicates whether this node is currently occupied.
    /// </summary>
    public bool occupied = false;
}
