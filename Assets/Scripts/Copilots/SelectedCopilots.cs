using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCopilots : MonoBehaviour
{
    public CopilotPassiveMechanic passive;
    public CopilotActiveMechanic active;
    void Start()
    {
        gameObject.AddComponent(GameManager.singleton.active.GetType());
        gameObject.AddComponent(GameManager.singleton.passive.GetType());
        active = GetComponent<CopilotActiveMechanic>();
        passive = GetComponent<CopilotPassiveMechanic>();
        active.CopyInfo(GameManager.singleton.active);
        passive.CopyInfo(GameManager.singleton.passive);
        Instantiate(GameManager.singleton.activeCopilot).name = GameManager.singleton.activeCopilot.name;
        Instantiate(GameManager.singleton.passiveCopilot).name = GameManager.singleton.passiveCopilot.name;
    }
}
