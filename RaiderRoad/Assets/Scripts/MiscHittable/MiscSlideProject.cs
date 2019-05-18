using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscSlideProject : MiscHittable
{
    [SerializeField] private slideController MySlideController;
    [SerializeField] private ParticleSystem objectBreakParticles;
    [SerializeField] private float CooldownTime;
    private float MyTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MyTimer < CooldownTime)
        {
            MyTimer += Time.deltaTime;
        }
    }

    public override void RegisterHit() {
        Instantiate(objectBreakParticles, transform.position, Quaternion.identity);

        if (MyTimer >= CooldownTime) {
            MySlideController.SkipToNextClip();
            MyTimer = 0f;
        }
    }
}
