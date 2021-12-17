using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleBehaviour : MonoBehaviour
{
    public Camera playerCam;
    public Transform shipWorldReference;    //This is a gameobject within the 3D world that I can use to make a positional reference to the front of the ship

    private RectTransform rectTransform;
    private float imageWidth;
    private float imageHeight;

    private void Awake()
    {
        playerCam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        imageWidth = rectTransform.rect.width;
        imageHeight = rectTransform.rect.height;
    }

    private void LateUpdate()
    {
        if (playerCam && shipWorldReference)
        {
            Vector2 screenPosition = playerCam.WorldToScreenPoint(shipWorldReference.transform.position);
            screenPosition -= new Vector2(imageWidth / 2.0f, imageHeight / 2.0f);
            screenPosition.x = Mathf.Floor(screenPosition.x);
            screenPosition.y = Mathf.Floor(screenPosition.y);
            rectTransform.anchoredPosition = screenPosition;
        }
    }
}
