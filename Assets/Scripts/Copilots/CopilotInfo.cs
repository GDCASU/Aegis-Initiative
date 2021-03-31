using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class CopilotInfo : MonoBehaviour
{
    public Character character;
    public CopilotData copilotData;
    public Sprite[] portraits;
    public Sprite portrait;
    public Sprite fullBody;

    [Header("Active")]
    public CopilotActiveMechanic copilotActive;

    [Header("Passive")]
    public CopilotPassiveMechanic copilotPassive;

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
