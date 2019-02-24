using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for constructable walls
/// </summary>
public class Wall : ConstructableGen<BuildNode>
{
    public bool isOccupied = false;
    public override void OnStart()
    {
        // Do nothing
    }

    public override void OnUpdate()
    {
        // Do nothing
    }

    /// <summary>
    /// On break hook to spawn particles
    /// </summary>
    public override void OnBreak()
    {
		Instantiate(objectBreakParticles, transform.position, Quaternion.identity);
	}
}
