using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float speed;    //set to -35 to match road speed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed); //once bs is sorted out, will replace with vector3.forward
        if(this.transform.position.z < -35f){
            Destroy(gameObject);
        }
    }
    //this comment is a test
}
