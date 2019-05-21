using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Speed variable")]
    [SerializeField]
    private float speed;    //set to -35 to match road speed

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    /// <summary>
    /// Moves the obstacle down the road, destroying it once it reaches a certain threshold
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed); //once bs is sorted out, will replace with vector3.forward
        if(transform.position.z < -30f)
        {
            foreach (PlayerController_Rewired pc in gameObject.GetComponentsInChildren<PlayerController_Rewired>())
            {
                pc.transform.parent = null;
                pc.takeDamage(Constants.PLAYER_ROAD_DAMAGE);
                pc.transform.position = GameObject.Find("player1Spawn").transform.position;
            }
        }
        if(this.transform.position.z < -35f){
            Destroy(gameObject);
        }
    }
    //this comment is a test
}
