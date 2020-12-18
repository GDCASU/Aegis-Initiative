using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CopilotUI : MonoBehaviour
{

    public static CopilotUI singleton;

    private CopilotInfo selected;
    public string sceneToLoad;

    [Header("Active")]
    #region Active
    public Text activeDescription;
    public Button activeButton;
    public Text active;
    public Image activeIcon;
    public bool activeSelected;
    #endregion

    [Header("Passive")]
    #region Passive
    public Text passiveDescription;
    public Button passiveButton;
    public Text passive;
    public Image passiveIcon;
    public bool passiveSelected;
    #endregion

    [Header("Layout")]
    #region Layout
    public Button start;
    public Image portrait;
    public GameObject copilotUI;
    public GameObject goBackPrompt;
    public GameObject selectionPanel;
    public GameObject scrollViewContent;
    public GameObject copilotButtonPrefab;
    #endregion

    [Header("Copilot Prefabs")]
    #region Copilot Prefabs
    public GameObject feebeePrefab;
    public GameObject daddyLongLegs;
    #endregion
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        foreach (CopilotData copilot in ProfileManager.instance.GetCurrentCopilotList())
        {
            GameObject tempCopilotButton = Instantiate(copilotButtonPrefab);
            CopilotInfo tempCopilotInfo = tempCopilotButton.GetComponent<CopilotButton>().copilotInfo;
            switch (copilot.name)
            {
                case "Feebee": 
                    feebeePrefab.GetComponent<CopilotInfo>().copilotData = copilot;
                    tempCopilotInfo.CopyInfo(feebeePrefab.GetComponent<CopilotInfo>());
                    break;
                case "Daddy Long Legs": 
                    daddyLongLegs.GetComponent<CopilotInfo>().copilotData = copilot;
                    tempCopilotInfo.CopyInfo(daddyLongLegs.GetComponent<CopilotInfo>());
                    break;
            }
            tempCopilotButton.GetComponent<CopilotButton>().SetButton();
            tempCopilotButton.transform.SetParent(scrollViewContent.transform);
            tempCopilotButton.transform.localScale = Vector3.one;
        }
        start.interactable = false;
    }
    public void CharacterSelected(CopilotInfo copilot)
    {
        selected = copilot;
        if (!selectionPanel.activeSelf) selectionPanel.SetActive(true);
        activeDescription.text = selected.activeDescription;
        passiveDescription.text = selected.passiveDescription;
        portrait.sprite = selected.portrait;
        if (selected.passive==passive.text)
        {
            activeButton.interactable = false;
        }
        else activeButton.interactable = true;
        if (selected.active==active.text)
        {
            passiveButton.interactable = false;
        }
        else passiveButton.interactable = true;

    }
    public void SelectPassive()
    {
        passive.text = selected.passive;
        passiveIcon.sprite = selected.passiveIcon;
        Copilots.singleton.passive = selected.passive;
        activeButton.interactable = false;
        passiveSelected = true;
        if (activeSelected && passiveSelected) start.interactable = true;
    }
    public void SelectActive()
    {
        active.text = selected.active;
        activeIcon.sprite = selected.activeIcon;
        Copilots.singleton.active = selected.active;
        passiveButton.interactable = false;
        activeSelected = true;
        if (activeSelected && passiveSelected) start.interactable = true;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad,LoadSceneMode.Single);
    }
    public void GoBackToHubPrompt()
    {
        goBackPrompt.SetActive(true);
    }
    public void GoBackToHubPromptConfirm()
    {
        selectionPanel.SetActive(false);
        activeButton.interactable = true;
        passiveButton.interactable = true;
        passiveSelected = false;
        activeSelected = false;
        active.text = "";
        passive.text = "";
        activeDescription.text = "";
        passiveDescription.text = "";
        portrait.sprite = null;
        passiveIcon.sprite = null;
        activeIcon.sprite = null;
        selected = null;
        goBackPrompt.SetActive(false);
        copilotUI.SetActive(false);
    }
    public void GoBackToHubPromptCancel() 
    {
        goBackPrompt.SetActive(false);
    }
}
