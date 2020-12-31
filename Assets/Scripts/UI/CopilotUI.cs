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
    public Text activeName;
    public Button activeButton;
    public Text active;
    public Image activeIcon;
    public bool activeSelected;
    #endregion

    [Header("Passive")]
    #region Passive
    public Text passiveDescription;
    public Text passiveName;
    public Button passiveButton;
    public Text passive;
    public Image passiveIcon;
    public bool passiveSelected;
    #endregion

    [Header("Layout")]
    #region Layout
    public Button start;
    public Text selectionName;
    public Image fullBody;
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
    /// <summary>
    /// Method used to create UI buttons for all the character found in a saved file and adds them to the ScrollViewContent
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Method used to pop up the selection panel and give a characters info as well as allow the player to choose their passive and active abilities.  Called  by a Unity UI button
    /// </summary>
    /// <param name="copilotInfo">The CopilotInfo of the selected character</param>
    public void CharacterSelected(CopilotInfo copilotInfo)
    {
        selected = copilotInfo;
        selectionName.text = selected.copilotData.name;
        if (!selectionPanel.activeSelf) selectionPanel.SetActive(true);
        activeName.text = selected.copilotActive.abilityName;
        activeDescription.text = selected.copilotActive.description;
        passiveName.text = selected.copilotPassive.abilityName;
        passiveDescription.text = selected.copilotPassive.description;
        fullBody.sprite = selected.fullBody;
    }
    /// <summary>
    /// Method used to assign the currently selected copilot's passive ability, this blockes their active from being able to be chosen as well. Called  by a Unity UI button
    /// </summary>
    public void SelectPassive()
    {
        if (selected.copilotActive.abilityName == GameManager.singleton.active.abilityName) ClearSelectedCopilotAbilities();
        passive.text = selected.copilotPassive.abilityName;
        passiveIcon.sprite = selected.copilotPassive.icon;
        GameManager.singleton.passive.CopyInfo(selected.copilotPassive);
        //GameManager.singleton.selectedUIPassive = selected.copilotPassive.abilityName;

        passiveSelected = true;
        if (activeSelected && passiveSelected) start.interactable = true;

    }
    /// <summary>
    /// Method used to assign the currently selected copilot's active ability, this blockes their passive from being able to be chosen as well.  Called  by a Unity UI button
    /// </summary>
    public void SelectActive()
    {
        if (selected.copilotPassive.abilityName == GameManager.singleton.passive.abilityName) ClearSelectedCopilotAbilities();
        active.text = selected.copilotActive.abilityName;
        activeIcon.sprite = selected.copilotActive.icon;
        GameManager.singleton.active.CopyInfo(selected.copilotActive);
        //GameManager.singleton.selectedUIActive = selected.copilotActive.abilityName;
        activeSelected = true;
        if (activeSelected && passiveSelected) start.interactable = true;
    }
    /// <summary>
    /// Method used to start the selected scene after selecting a active an a passive.  Called  by a Unity UI button
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad,LoadSceneMode.Single);
    }
    /// <summary>
    /// Method used to pop up the "Are you sure?" prompt before returning to the Hub UI.  Called  by a Unity UI button
    /// </summary>
    public void GoBackToHubPrompt()
    {
        goBackPrompt.SetActive(true);
    }
    /// <summary>
    /// Method used to go back to the Hub UI and resetting the CopilotsUI to their initial state.  Called  by a Unity UI button
    /// </summary>
    public void GoBackToHubPromptConfirm()
    {
        selectionPanel.SetActive(false);
        selectionName.text = "";
        activeName.text = "";
        activeDescription.text = "";
        passiveName.text = "";
        passiveDescription.text = "";
        activeButton.interactable = true;
        passiveButton.interactable = true;
        ClearSelectedCopilotAbilities();
        fullBody.sprite = null;
        selected = null;
        copilotUI.SetActive(false);
        goBackPrompt.SetActive(false);
    }
    /// <summary>
    /// Method used to ge back to the CopilotUI and turn off the "Are you sure?" prompt.  Called  by a Unity UI button
    /// </summary>
    public void GoBackToHubPromptCancel() 
    {
        goBackPrompt.SetActive(false);
    }
    /// <summary>
    /// Method used to reset the selected active and passive
    /// </summary>
    public void ClearSelectedCopilotAbilities()
    {
        GameManager.singleton.selectedUIActive = "";
        GameManager.singleton.selectedUIPassive = "";
        active.text = "Active Role";
        passive.text = "Passive Role";
        passiveSelected = false;
        activeSelected = false;
        passiveIcon.sprite = null;
        activeIcon.sprite = null;
    }
}
