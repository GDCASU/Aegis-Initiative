using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class CopilotInfo : MonoBehaviour
{
    public Character character;
    public GameManager.Copilots copilot;
    public CopilotData copilotData;
    public Sprite portrait;
    public Sprite fullBody;

    [Header("Active")]
    #region Active
    public CopilotActiveMechanic copilotActive;
    //public string active;
    //public string activeDescription;
    //public Sprite activeIcon;
    #endregion

    [Header("Passive")]
    #region Passive
    public CopilotPassiveMechanic copilotPassive;
    //public string passive;
    //public string passiveDescription;
    //public Sprite passiveIcon;
    #endregion
    public void CopyInfo(CopilotInfo copilotInfo)
    {
        character = copilotInfo.character;
        copilotData = copilotInfo.copilotData;
        fullBody = copilotInfo.fullBody;
        portrait = copilotInfo.portrait;

        copilotActive.CopyInfo(copilotInfo.copilotActive);
        copilotPassive.CopyInfo(copilotInfo.copilotPassive);
    }
}
