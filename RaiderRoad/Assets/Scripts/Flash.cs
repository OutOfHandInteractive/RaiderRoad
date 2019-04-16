using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {

    //Public varibales
    public float upTime;
    public float downTime;

    private float cooldown;
    private bool isUp = true;
    private Image img;

    void Start()
    {
        img = gameObject.GetComponent<Image>();
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
                img.enabled = false;
            }
            else
            {
                isUp = true;
                cooldown = upTime;
                img.enabled = true;
            }
        }

        cooldown -= Time.deltaTime;
	}
}
