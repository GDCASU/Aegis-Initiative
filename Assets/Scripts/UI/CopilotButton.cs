using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CopilotButton : MonoBehaviour
{
    CopilotInfo copilot;
    private Image portrait;
    public Button button;
    public void Select()=> CopilotUI.singleton.CharacterSelected(copilot);

    public void SetButton()
    {
        portrait.sprite = copilot.portrait;
        if (!copilot.copilotData.isUnlocked) button.interactable = false;
    }

}
