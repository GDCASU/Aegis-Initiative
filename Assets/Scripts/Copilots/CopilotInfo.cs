using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class CopilotInfo : MonoBehaviour
{
    public Character character;
    public CopilotData copilotData;
    public Sprite portrait;

    [Header("Active")]
    #region Active
    public string active;
    public string activeDescription;
    public Sprite activeIcon;
    #endregion

    [Header("Passive")]
    #region Passive
    public string passive;
    public string passiveDescription;
    public Sprite passiveIcon;

    public void CopyInfo(CopilotInfo copilotInfo)
    {
        character = copilotInfo.character;
        copilotData = copilotInfo.copilotData;
        portrait = copilotInfo.portrait;

        active = copilotInfo.active;
        activeDescription = copilotInfo.activeDescription;
        activeIcon = copilotInfo.activeIcon;

        passive = copilotInfo.passive;
        passiveDescription = copilotInfo.passiveDescription;
        passiveIcon = copilotInfo.passiveIcon;
    }
    #endregion
}
