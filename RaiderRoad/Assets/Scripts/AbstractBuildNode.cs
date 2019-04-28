using UnityEngine;
using System.Collections;

/// <summary>
/// This is the parent class for all build nodes. Currently this only holds the bare minimum, but eventually we should unite the subclasses to reduce duplication.
/// </summary>
public abstract class AbstractBuildNode : MonoBehaviour
{
    public static void SetOutlineActive(GameObject item, float active)
    {
        foreach (Renderer renderer in item.GetComponentsInChildren<Renderer>())
        {
            Material tempMat = renderer.material;
            if (tempMat.shader.name == "Outlined/Uniform")
            {
                Material outline = Instantiate(tempMat);
                renderer.material = outline;
                outline.SetFloat("_Active", active);
            }
        }
    }

    public static void SetOutlineActiveOverride(GameObject item, float active, Color overrideColor)
    {
        foreach (Renderer renderer in item.GetComponentsInChildren<Renderer>())
        {
            Material tempMat = renderer.material;
            if (tempMat.shader.name == "Outlined/Uniform")
            {
                Material outline = Instantiate(tempMat);
                renderer.material = outline;
                //override existing outline color
                outline.SetColor("_OutlineColor", overrideColor);
                outline.SetFloat("_Active", active);
            }
        }
    }

    /// <summary>
    /// Boolean flag that indicates whether this node is currently occupied.
    /// </summary>
    public bool occupied = false;
}
