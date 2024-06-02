using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CopilotUI : MonoBehaviour
{
    public static CopilotUI singleton;

    private CopilotInfo selected;
    private GameObject selectedPrefab;
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
    public GameObject frederickPrefab;
    public GameObject mushroomFriendPrefab;
    public GameObject spaceGirlPrefab;
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
        GameObject tempPrefab = feebeePrefab;//just to have a default
        foreach (CopilotData copilot in ProfileManager.instance.GetCurrentCopilotList())
        {
            GameObject tempButton = Instantiate(copilotButtonPrefab);
            CopilotButton tempCopilotButton = tempButton.GetComponent<CopilotButton>();
            switch (copilot.name)
            {
                case Copilots.Feebee:
                    tempPrefab = feebeePrefab;
                    break;
                case Copilots.DaddyLongLegs:
                    tempPrefab = daddyLongLegs;
                    break;
                case Copilots.Frederick:
                    tempPrefab = frederickPrefab;
                    break;
                case Copilots.MushroomFriend:
                    tempPrefab = mushroomFriendPrefab;
                    break;
                case Copilots.SpaceGirl:
                    tempPrefab = spaceGirlPrefab;
                    break;
            }
            tempPrefab.GetComponent<CopilotInfo>().copilotData = copilot;
            tempCopilotButton.SetButton(tempPrefab);
            tempButton.transform.SetParent(scrollViewContent.transform);
            tempButton.transform.localScale = Vector3.one;
        }
        start.interactable = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    /// <summary>
    /// Method used to pop up the selection panel and give a copilot's info as well as allow the player to choose their passive and active abilities.  Called  by a Unity UI button
    /// </summary>
    /// <param name="copilotPrefab">The Prefab of the selected copilot</param>
    public void CharacterSelected(GameObject copilotPrefab)
    {
        SoundManager.singleton.PlayOneShot(SoundManager.sfxMap[SoundManager.SFX.Select], transform.position, SoundManager.VolumeType.sfx);
        selectedPrefab = copilotPrefab;
        selected = copilotPrefab.GetComponent<CopilotInfo>();
        selectionName.text = selected.character.name;
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
        if (GameManager.singleton.active!=null && selected.copilotActive.abilityName == GameManager.singleton.active.abilityName)
        {
            ClearSelectedCopilotAbilities();
        }
        passive.text = selected.copilotPassive.abilityName;
        passiveIcon.sprite = selected.copilotPassive.icon;
        GameManager.singleton.ChangePassive(selected.copilotPassive.GetType(),selected.copilotPassive);
        SoundManager.singleton.PlayOneShot(SoundManager.sfxMap[SoundManager.SFX.Select], transform.position, SoundManager.VolumeType.sfx);
        passiveSelected = true;
        if (activeSelected && passiveSelected) start.interactable = true;

    }
    /// <summary>
    /// Method used to assign the currently selected copilot's active ability, this blockes their passive from being able to be chosen as well.  Called  by a Unity UI button
    /// </summary>
    public void SelectActive()
    {
        if (GameManager.singleton.passive != null && 
            selected.copilotPassive.abilityName == GameManager.singleton.passive.abilityName)
            ClearSelectedCopilotAbilities();
        active.text = selected.copilotActive.abilityName;
        activeIcon.sprite = selected.copilotActive.icon;
        GameManager.singleton.ChangeActive(selected.copilotActive.GetType(), selected.copilotActive);
        SoundManager.singleton.PlayOneShot(SoundManager.sfxMap[SoundManager.SFX.Select], transform.position, SoundManager.VolumeType.sfx);
        activeSelected = true;
        if (activeSelected && passiveSelected) start.interactable = true;
    }
    /// <summary>
    /// Method used to start the selected scene after selecting a active an a passive.  Called  by a Unity UI button
    /// </summary>
    public void StartGame()
    {
        SoundManager.singleton.PlayOneShot(SoundManager.sfxMap[SoundManager.SFX.Select], transform.position, SoundManager.VolumeType.sfx);
        if (ProfileManager.instance.CurrentProfile.currentStage < 4) LevelChanger.singleton.FadeOutToLevel(GameManager.singleton.levels[ProfileManager.instance.CurrentProfile.currentStage]);
        else LevelChanger.singleton.FadeOutToLevel(GameManager.singleton.levels[GameManager.singleton.levelSelected]);
    }
    /// <summary>
    /// Method used to pop up the "Are you sure?" prompt before returning to the Hub UI.  Called  by a Unity UI button
    /// </summary>
    public void GoBackToHubPrompt()
    {
        SoundManager.singleton.PlayOneShot(SoundManager.sfxMap[SoundManager.SFX.Select], transform.position, SoundManager.VolumeType.sfx);
        goBackPrompt.SetActive(true);
    }
    /// <summary>
    /// Method used to go back to the Hub UI and resetting the CopilotsUI to their initial state.  Called  by a Unity UI button
    /// </summary>
    public void GoBackToHubPromptConfirm()
    {
        SoundManager.singleton.PlayOneShot(SoundManager.sfxMap[SoundManager.SFX.Select], transform.position, SoundManager.VolumeType.sfx);
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
        SceneManager.LoadScene("MainMenuUI", LoadSceneMode.Single);
    }
    /// <summary>
    /// Method used to ge back to the CopilotUI and turn off the "Are you sure?" prompt.  Called  by a Unity UI button
    /// </summary>
    public void GoBackToHubPromptCancel() 
    {
        SoundManager.singleton.PlayOneShot(SoundManager.sfxMap[SoundManager.SFX.Select], transform.position, SoundManager.VolumeType.sfx);
        goBackPrompt.SetActive(false);
    }
    /// <summary>
    /// Method used to reset the selected active and passive
    /// </summary>
    public void ClearSelectedCopilotAbilities()
    {
        GameManager.singleton.RemoveActive();
        GameManager.singleton.RemovePassive();
        active.text = "Active Role";
        passive.text = "Passive Role";
        passiveSelected = false;
        activeSelected = false;
        passiveIcon.sprite = null;
        activeIcon.sprite = null;
    }
}
