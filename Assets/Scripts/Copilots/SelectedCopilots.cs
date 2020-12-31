using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCopilots : MonoBehaviour
{
    public static SelectedCopilots singleton;
    public string active;
    public string passive;
    public CopilotPassiveMechanic pass;
    public CopilotActiveMechanic act;
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
        //gameObject.AddComponent(GameManager.singleton.passive.GetType());
        //gameObject.AddComponent(GameManager.singleton.active.GetType());
    }
}
