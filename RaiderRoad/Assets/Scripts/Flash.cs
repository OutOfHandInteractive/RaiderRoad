using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour {

    //Public varibales
    public float upTime;
    public float downTime;

    private float cooldown;
    private bool isUp = true;
    private SpriteRenderer sr;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        cooldown = upTime;
    }

    // Update is called once per frame
    void Update ()
    {
        if (cooldown <= 0.0f)
        {
            if (isUp)
            {
                isUp = false;
                cooldown = downTime;
                sr.enabled = false;
            }
            else
            {
                isUp = true;
                cooldown = upTime;
                sr.enabled = true;
            }
        }

        cooldown -= Time.deltaTime;
	}
}
