using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float TimeToDestroy = 5f;
    private float MyTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MyTimer += Time.deltaTime;
        if(MyTimer >= TimeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
