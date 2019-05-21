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

    /// <summary>
    /// DEPRECATED - Use flie CameraPoint.cs
    /// Uses the average position of player characters as a point for the camera to follow.
    /// </summary>
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

    /// <summary>
    /// DEPRECATED - Use flie CameraPoint.cs
    /// Uses the size of the bounding box containing the player positions to determine the zoom of the camera.
    /// </summary>
    void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetSize() / zoomLimiter);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    /// <summary>
    /// DEPRECATED - Use flie CameraPoint.cs
    /// Gets the size of the bounding box containing the player positions.
    /// </summary>
    /// <returns>
    /// A float that represents the size of the bounding box.
    /// </returns>
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

    /// <summary>
    /// DEPRECATED - Use flie CameraPoint.cs
    /// Gets the average position of the player characters.
    /// </summary>
    /// <returns>
    /// A Vector3 which is the location of the average position of players.
    /// </returns>
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
