using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnAxis : MonoBehaviour {
    
    public float speed = 50f;
    
    public enum Axis {X, Y, Z};
    
    public Axis myAxis = Axis.X; 
    
    
    void Update()
    {
        if (myAxis == Axis.X)
            transform.Rotate(Vector3.right, speed * Time.deltaTime);
        else if (myAxis == Axis.Y) 
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
        else if (myAxis == Axis.Z) 
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
    
}