using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CopilotButton : MonoBehaviour
{
    public CopilotInfo copilotInfo;
    public Image portrait;
    public Button button;
    public void Select()=> CopilotUI.singleton.CharacterSelected(copilotInfo);

    public void SetButton()
    {
        portrait.sprite = copilotInfo.portrait;
        if (!copilotInfo.copilotData.isUnlocked) button.interactable = false;
    }

}
