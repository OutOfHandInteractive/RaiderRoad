using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject[] targets;
    public GameObject[] enemies;
    public bool reactToEnemies = true;
    public Vector3 offset;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public float smoothTime = 0.5f;
    public float minZoom = 1f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets.Length == 0)
        {
            targets = GameObject.FindGameObjectsWithTag("Player");
        }
        else
        {
            if (reactToEnemies)
                enemies = GameObject.FindGameObjectsWithTag("eVehicle");

            MoveCamera();
            ZoomCamera();
        }
    }

    void MoveCamera()
    {
        Vector3 average = GetAverage();
        average += offset;
        Vector3 newPosition;
        if (average.x > bottomLeft.x && average.x < topRight.x && average.z > bottomLeft.y && average.z < topRight.y)
        {
            newPosition = average;
            newPosition.y = offset.y;
        }
        else
        {
            newPosition = transform.position;
        }

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetSize() / zoomLimiter);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetSize()
    {
        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].transform.position);
        }
        if (enemies.Length != 0 && reactToEnemies)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Debug.Log("Bye Bye enemy");
                bounds.Encapsulate(enemies[i].transform.position);
            }
        }
        
        return bounds.size.x;
    }

    Vector3 GetAverage()
    {
        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].transform.position);
        }
        if (enemies.Length != 0 && reactToEnemies)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                bounds.Encapsulate(enemies[i].transform.position);
            }
        }

        return bounds.center;
    }
}
