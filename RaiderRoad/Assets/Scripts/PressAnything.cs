using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnything : MonoBehaviour
{
    public string scene;

    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
