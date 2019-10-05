using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class rvSpawnPoint : MonoBehaviour
{

    public ParticleSystem myParticle;

    public Image respawnCircle;
    public float time;
    public float totalTime;

    // Start is called before the first frame update
    void Start()
    {
        respawnCircle = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartRespawnTimer(float timerLength)
    {
        time = 0f;
        totalTime = timerLength;
        respawnCircle.fillAmount = 1f;
        StartCoroutine("TimerCountdown");
    }

    IEnumerator TimerCountdown()
    {
        while (time < totalTime)
        {
            time += Time.deltaTime;
            respawnCircle.fillAmount = (totalTime - time)/totalTime;

            //put ui element in position of spawnpoint
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            respawnCircle.gameObject.GetComponent<RectTransform>().position = screenPos;
            yield return null;
        }

        //spawn spawn particle
        Instantiate(myParticle, transform.position, transform.rotation);
    }
}
