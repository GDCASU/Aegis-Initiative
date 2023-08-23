using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField]
    private GameObject HideUiObject;

    public void ClickBackButton()
    {
        HideUiObject.SetActive(false);
    }
}
