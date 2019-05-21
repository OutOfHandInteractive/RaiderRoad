using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class CameraPoint : MonoBehaviour
{

    public GameObject[] targets;
    public float yOffset;
    public Vector2 bottomLeft;
    public Vector2 topRight;
    public float smoothTime = 0.5f;
    public float minZoom = 1f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;
    public CinemachineVirtualCamera cam;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (targets.Length == 0)
        {
            targets = GameObject.FindGameObjectsWithTag("Player").Concat(GameObject.FindGameObjectsWithTag("PlayerSpawn")).ToArray();
        }

        MovePoint();
        ZoomCamera();
    }

    /// <summary>
    /// Uses the average position of player characters as a point for the camera to follow.
    /// </summary>
    void MovePoint()
    {
        Vector3 average = GetAverage();
        average.y = yOffset;
        Vector3 newPosition;
        if (average.x > bottomLeft.x && average.x < topRight.x && average.z > bottomLeft.y && average.z < topRight.y)
        {
            newPosition = average;
        }
        else
        {
            newPosition = transform.position;
            if (average.x < bottomLeft.x || average.x > topRight.x)
            {
                newPosition.z = average.z;
            }
            else if (average.z < bottomLeft.y || average.z > topRight.y)
            {
                newPosition.x = average.x;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    /// <summary>
    /// Gets the average position of the player characters.
    /// </summary>
    /// <returns>
    /// A Vector3 which is the location of the average position of players.
    /// </returns>
    Vector3 GetAverage()
    {
        if (targets[0] == null)
        {
            targets = GameObject.FindGameObjectsWithTag("Player").Concat(GameObject.FindGameObjectsWithTag("PlayerSpawn")).ToArray();
        }
        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
            {
                bounds.Encapsulate(targets[i].transform.position);
            }
        }

        return bounds.center;
    }

    /// <summary>
    /// Uses the size of the bounding box containing the player positions to determine the zoom of the camera.
    /// </summary>
    void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetSize() / zoomLimiter);

        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, newZoom, Time.deltaTime);
    }

    /// <summary>
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

        return bounds.size.x;
    }
}
