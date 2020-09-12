using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleBehaviour : MonoBehaviour
{
    public Camera playerCam;
    public Transform shipWorldReference;    //This is a gameobject within the 3D world that I can use to make a positional reference to the front of the ship
    public Transform innerReticle;

    private RectTransform rectTransform;
    private float imageWidth;
    private float imageHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        imageWidth = rectTransform.rect.width;
        imageHeight = rectTransform.rect.height;
    }

    private void Update()
    {
        if(playerCam && shipWorldReference)
        {
            Vector2 screenPosition = playerCam.WorldToScreenPoint(shipWorldReference.transform.position);
            screenPosition -= new Vector2(imageWidth / 2, imageHeight / 2);
            rectTransform.anchoredPosition = screenPosition;
        }
    }
}
