using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public CopilotMechanic active;
    public CopilotMechanic passive;
    public GameObject activeCopilot;
    public GameObject passiveCopilot;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    public void ChangeActive(System.Type type, CopilotMechanic info)
    {
        if (active != null) RemoveActive();
        activeCopilot = info.gameObject;
        gameObject.AddComponent(type);
        active = GetComponent<CopilotActiveMechanic>();
        active.CopyInfo(info);
        active.enabled = false;
    }
    public void ChangePassive(System.Type type, CopilotMechanic info)
    {
        if (passive != null) RemovePassive();
        passiveCopilot = info.gameObject;
        gameObject.AddComponent(type);
        passive = GetComponent<CopilotPassiveMechanic>();
        passive.CopyInfo(info);
        passive.enabled = false;
    }
    public void RemoveActive()
    {
        Destroy(active);
        activeCopilot = null;
    }
    public void RemovePassive()
    {
        Destroy(passive);
        activeCopilot = null;
    }
}

public enum TypeOfMechanic
{
    Buff,
    AOE,
    Action,
    Vision,
    Create,
    Transformative,
    Event,
};
public enum Copilots
{
    DaddyLongLegs = -1,
    Feebee = 0
};