using UnityEngine;
using System;
using System.Collections;

public class bobbing : MonoBehaviour
{
    float originalY;

    public float freq = 8f;
    public float floatStrength = 0.05f;

    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + Mathf.Sin(Time.fixedTime * Mathf.PI * freq) * floatStrength, transform.position.z);
    }
}