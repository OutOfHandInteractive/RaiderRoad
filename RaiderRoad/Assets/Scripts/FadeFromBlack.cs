using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFromBlack : MonoBehaviour
{
    //black fade in variables
    private Image BlackOverlay;
    private float AlphaVal = 1f;
    [SerializeField] private float BlackFadeDur = 1f;

    // Start is called before the first frame update
    void Start()
    {
        BlackOverlay = GetComponent<Image>();
        AlphaVal = 1f;
        BlackOverlay.color = new Color(0f, 0f, 0f, AlphaVal);
    }

    // Update is called once per frame
    void Update()
    {
        //Always fade up from black
        if(AlphaVal > 0)
        {
            AlphaVal -= Time.deltaTime / BlackFadeDur;
            BlackOverlay.color = new Color(0f, 0f, 0f, AlphaVal);
            Debug.Log("Poop");
        }
    }
}
