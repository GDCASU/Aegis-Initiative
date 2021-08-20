using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCopilots : MonoBehaviour
{
    public static SelectedCopilots singleton;

    public CopilotPassiveMechanic passive;
    public GameObject passiveCopilot;
    public CopilotActiveMechanic active;
    public GameObject activeCopilot;
    private void Awake() 
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        gameObject.AddComponent(GameManager.singleton.active.GetType());
        gameObject.AddComponent(GameManager.singleton.passive.GetType());
        active = GetComponent<CopilotActiveMechanic>();
        passive = GetComponent<CopilotPassiveMechanic>();
        active.CopyInfo(GameManager.singleton.active);
        passive.CopyInfo(GameManager.singleton.passive);
        activeCopilot = Instantiate(GameManager.singleton.activeCopilot);
        passiveCopilot = Instantiate(GameManager.singleton.passiveCopilot);
        activeCopilot.name = GameManager.singleton.activeCopilot.name;
        passiveCopilot.name = GameManager.singleton.passiveCopilot.name;
    }
}
