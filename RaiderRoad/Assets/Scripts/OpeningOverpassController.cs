using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OpeningOverpassController : MonoBehaviour
{
    private sceneManagerScript sceneManage;
    private GameManager g;
    private float speed = -35; //get this from chunkspawner
    [SerializeField] private float OverSpawnDelay = 1f;
    [SerializeField] private float RoofFallStr = 5f;
    [SerializeField] private float RoofTorqueStr = 5f;
    private float myTimer = 0f;
    [SerializeField] private Transform RvRoofTrans;
    [SerializeField] private GameObject RvRoofVFX;
    private int oldCullMask;
    [SerializeField] private CameraShake CameraShakeScr;

    private AudioSource MyAudioSource;
    [SerializeField] private GameObject StartCameraAngle;
    [SerializeField] private GameObject MainCameraAngle;

    // Start is called before the first frame update
    void Start()
    {
        sceneManage = sceneManagerScript.Instance;
        g = GameManager.GameManagerInstance;

        //check if should play opening anim
        if (sceneManage.PlayOpenAnim) {
            RvRoofTrans.gameObject.SetActive(true);
            //turn off so it doesn't play on restarts or later sections
            sceneManage.SetOpenAnim(false);

            //stop player input
            g.GetComponent<GameManager>().pauseInput = true;

            //Remove UI from camera view
            oldCullMask = Camera.main.cullingMask;
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));

            MyAudioSource = GetComponent<AudioSource>();

        } else {
            MainCameraAngle.SetActive(true);
            StartCameraAngle.SetActive(false);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(myTimer < OverSpawnDelay) {
            myTimer += Time.deltaTime;
        } else {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RV")
        {
            //throw roof
            Rigidbody roofRB = RvRoofTrans.GetComponent<Rigidbody>();
            roofRB.AddForce(RvRoofTrans.forward * -RoofFallStr);
            roofRB.AddTorque(RvRoofTrans.right * -RoofTorqueStr);

            //polish effects
            Instantiate(RvRoofVFX, RvRoofTrans.position, RvRoofVFX.transform.rotation);
            CameraShakeScr.Shake(.9f, 12f, 6f);

            MyAudioSource.Play();

            StartCoroutine(TransitionToGame());
        }
    }

    IEnumerator TransitionToGame()
    {
        yield return new WaitForSeconds(0.9f);
        MainCameraAngle.SetActive(true);
        yield return new WaitForSeconds(1.7f);
        //re-enable input
        g.GetComponent<GameManager>().pauseInput = false;

        //Show UI in camera
        Camera.main.cullingMask = oldCullMask;

        //destroy objects
        Destroy(RvRoofTrans.gameObject, 8);
        Destroy(gameObject, 8);
    }

}
