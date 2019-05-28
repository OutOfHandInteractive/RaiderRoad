using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles the audio for the RV.
/// </summary>
public class RVAudio : CarAudio
{
    /// <summary>
    /// Skids are done in a subobject so they don't interfere with honking
    /// </summary>
    private SkidAudio skid;

    protected override void OnStart()
    {
        base.OnStart();
        skid = GetComponentInChildren<SkidAudio>();
    }

    public void Skid()
    {
        skid.Skid();
    }
}
