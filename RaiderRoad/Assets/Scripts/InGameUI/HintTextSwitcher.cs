using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HintTextSwitcher : MonoBehaviour
{

    public float readTime;
    public List<string> hints;

    private int index = 0;
    private float changeTime = 0f;
    private List<string> randomized;

    private Text textComp;

    // Use this for initialization
    void Start()
    {
        textComp = GetComponent<Text>();

        var rand = new System.Random();
        var copy = new List<string>(hints);
        
        randomized = new List<string>();
        while(copy.Count > 0)
        {
            int i = rand.Next(copy.Count);
            string hint = copy[i];
            copy.RemoveAt(i);
            randomized.Add(hint);
        }
        textComp.text = randomized[index];
    }

    // Update is called once per frame
    void Update()
    {
        changeTime += Time.deltaTime;
        //Debug.Log("changeTime: " + changeTime);
        if(changeTime >= readTime)
        {
            index = (index + 1) % randomized.Count;
            changeTime = 0;
            textComp.text = randomized[index];
        }
    }
}
