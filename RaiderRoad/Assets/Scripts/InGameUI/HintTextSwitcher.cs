using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class HintTextSwitcher : MonoBehaviour
{

    public float readTime;
    public List<string> hints;

    private int index = 0;
    private List<string> randomized;

    private Text textComp;

    // Use this for initialization
    void Start() {
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

		StartCoroutine(SwitchText());
    }

    // Update is called once per frame
    IEnumerator SwitchText() {
		while (true)
        {
            textComp.text = randomized[index];
            index = (index + 1) % randomized.Count;

            yield return new WaitForSeconds(readTime);
		}
    }
}
