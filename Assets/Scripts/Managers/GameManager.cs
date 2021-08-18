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
    public float musicVolume;
    public float sfxVolume;
    public Dictionary<int, string> levels;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
        levels = new Dictionary<int, string>();
        levels.Add(0, "MushroomStage");
        levels.Add(1, "VolcanoStage");
        levels.Add(2, "AsteroidStage");
        levels.Add(3, "PirateStage");
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
        passiveCopilot = null;
    }
    public void SaveProgress()
    {
        List<CopilotData> copilotList = ProfileManager.instance.CurrentProfile.CopilotList;
        CopilotData activeData = activeCopilot.GetComponent<CopilotInfo>().copilotData;
        CopilotData passiveData = passiveCopilot.GetComponent<CopilotInfo>().copilotData;

        for (int x = 0; x < copilotList.Count; x++)
        {
            if (copilotList[x].name == activeData.name) ProfileManager.instance.CurrentProfile.CopilotList[x] = activeData;
            else if (copilotList[x].name == passiveData.name) ProfileManager.instance.CurrentProfile.CopilotList[x] = passiveData;            
        }
    }
    public void AddCopilot(int number)
    {
        if (number == 1)ProfileManager.instance.CurrentProfile.AddCopilot(new CopilotData(Copilots.MushroomFriend, 0, 0, true));
        else if(number == 2 ) ProfileManager.instance.CurrentProfile.AddCopilot(new CopilotData(Copilots.Frederick, 0, 0, true));
        ProfileManager.instance.SaveCurrentProfile();
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