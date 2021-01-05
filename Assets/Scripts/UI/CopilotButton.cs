using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CopilotButton : MonoBehaviour
{
    public CopilotInfo copilotInfo;
    public Image portrait;
    public Button button;
    public GameObject copilotPrefab;
    public void Select()=> CopilotUI.singleton.CharacterSelected(copilotInfo);

    public void SetButton(GameObject prefab)
    {
        copilotPrefab = prefab;
        copilotInfo = copilotPrefab.GetComponent<CopilotInfo>();
        portrait.sprite = copilotInfo.portrait;
        name = copilotInfo.copilotData.name.ToString() + "Button";
        GetComponentInChildren<Text>().text = copilotInfo.copilotData.name.ToString();
        if (!copilotInfo.copilotData.isUnlocked) button.interactable = false;
    }
}
