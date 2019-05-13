using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skid : MonoBehaviour
{
    private Skidmarks skidmarksController;
    private int lastSkid = -1;
    private Transform parent;

    private float count = 0;
    private float skidDuration;
    private float skidIntensity = 1f;
    private float intensity = .5f;
    private bool skid = false;

    void Update()
    {
        if (skid)
        {
            int layerMask = ~((1 << 2) | (1 << 10)); // Ignore Layer NavMesh
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, layerMask))
            {
                if (parent != hit.transform.parent.GetChild(2))
                {
                    parent = hit.transform.parent.GetChild(2);
                    lastSkid = -1;
                }

                if (parent.transform.Find("SkidmarkController"))
                {
                    skidmarksController = parent.transform.Find("SkidmarkController").GetComponent<Skidmarks>();
                    lastSkid = skidmarksController.AddSkidMark(parent.InverseTransformPoint(hit.point), hit.normal, intensity, lastSkid);
                }
            }
        }

        if (count > 0)
        {
            intensity = (skidIntensity * (count / skidDuration));
            count -= Time.deltaTime;
        }
        else
        {
            skid = false;
        }
    }

    public void TireSkid(float duration, float newIntensity)
    {
        skid = true;
        skidDuration = duration;
        count = duration;
        skidIntensity = newIntensity;
        intensity = newIntensity;
    }
}
