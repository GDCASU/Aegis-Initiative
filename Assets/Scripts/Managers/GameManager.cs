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
        active = gameObject.AddComponent(type) as CopilotActiveMechanic;
        active.CopyInfo(info);
        active.enabled = false;
    }
    public void ChangePassive(System.Type type, CopilotMechanic info)
    {
        if (passive != null) RemovePassive();
        passiveCopilot = info.gameObject;
        passive = gameObject.AddComponent(type) as CopilotPassiveMechanic;
        passive.CopyInfo(info);
        passive.enabled = false;
    }
    public void RemoveActive()
    {
        Destroy(active);
        active = null;
        activeCopilot = null;
    }
    public void RemovePassive()
    {
        Destroy(passive);
        passive = null;
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
    Feebee = 0,
    Frederick = 1,
    MushroomFriend = 2,
    SpaceGirl = 3,
};