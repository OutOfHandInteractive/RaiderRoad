using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKplayerHandler : MonoBehaviour
{
    //Sorry to give IK its own script. The IK function only works on animator gameobject by design

    Animator myAni;

    //Anim IKs
    private Transform leftHandIKTran;
    private Transform rightHandIKTran;
    private float handIKPosWeight;
    private float handIKRotWeight;

    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHandIKs(Transform LeftHandTran, Transform RightHandTran, float PosWeight, float RotWeight)
    {
        leftHandIKTran = LeftHandTran;
        rightHandIKTran = RightHandTran;
        handIKPosWeight = PosWeight;
        handIKRotWeight = RotWeight;
    }

    //Function for hand IKs
    void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("IK play");

        myAni.SetIKPositionWeight(AvatarIKGoal.LeftHand, handIKPosWeight);
        myAni.SetIKRotationWeight(AvatarIKGoal.LeftHand, handIKRotWeight);
        myAni.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTran.position);
        myAni.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTran.rotation);

        myAni.SetIKPositionWeight(AvatarIKGoal.RightHand, handIKPosWeight);
        myAni.SetIKRotationWeight(AvatarIKGoal.RightHand, handIKRotWeight);
        myAni.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTran.position);
        myAni.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTran.rotation);
    }
}
