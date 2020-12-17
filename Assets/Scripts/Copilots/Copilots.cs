using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copilots : MonoBehaviour
{
    public static Copilots singleton;
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
