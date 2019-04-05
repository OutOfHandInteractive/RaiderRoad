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
        }

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

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

    void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetSize() / zoomLimiter);

        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, newZoom, Time.deltaTime);
    }

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
