using UnityEngine;
using System.Collections;

public class UniformScale : MonoBehaviour
{
    
    public float scale = 1;

    // Use this for initialization
    void Start()
    {
        transform.localScale *= scale;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
