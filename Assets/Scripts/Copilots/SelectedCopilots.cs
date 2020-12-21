using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCopilots : MonoBehaviour
{
    public static SelectedCopilots singleton;
    public string active;
    public string passive;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        active = "";
        passive = "";
    }
}
