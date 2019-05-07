using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTestScript : MonoBehaviour
{
    Animator myAnim;

    public float leftHandIKPosWeight = 0;
    public float leftHandIKRotWeight = 0;
    public Transform leftHandIKObj;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorIK(int layerIndex)
    {
        myAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKPosWeight);
        myAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKRotWeight);
        myAnim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKObj.position);
        myAnim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKObj.rotation);
    }
}
