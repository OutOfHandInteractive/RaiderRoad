using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiElementFollowObject : MonoBehaviour
{
    public float yPosOffset = 10f;

    private RectTransform uiElement;
    private RectTransform canvasRect;
    private GameObject objToFollow;
    private GameObject rvObj;
    private bool objSet = false;

    void Start()
    {
        uiElement = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (objSet)
        {
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(new Vector3(objToFollow.transform.position.x, rvObj.transform.position.y, rvObj.transform.position.z));
            Vector2 screenPos = new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f), (canvasRect.sizeDelta.y - yPosOffset) - (canvasRect.sizeDelta.y * 0.5f));

            uiElement.anchoredPosition = screenPos;
        }
    }

    public void SetObject(GameObject obj)
    {
        objToFollow = obj;
    }

    public void SetCanvas(RectTransform canvas)
    {
        canvasRect = canvas;
    }

    public void SetRv(GameObject rv)
    {
        rvObj = rv;
        objSet = true;
    }
}
