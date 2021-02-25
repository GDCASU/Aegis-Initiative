using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public List<GameObject> panels;

    public int currentPanel = 1;

    [Header("General Menu")]
    #region General Menu
    #endregion

    [Header("Main Menu")]
    #region Main Menu
    public GameObject startGame;
    #endregion

    [Header("Load Sves")]
    #region Load Saves
    public GameObject scrollViewContent;
    public GameObject saveButtonPrefab;
    public InputField nameInputField;
    #endregion

    private void Start()
    {
        LoadSaves();
    }

    void Update()
    {
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.UI_Cancel))
        {
            switch (currentPanel)
            {
                case 0:
                    CancelExitPrompt();
                    break;
                case 1:
                    ShowExitPrompt();       
                    break;
                case 2:
                    SwitchPanels(currentPanel, 1);
                    break;
                case 3:
                    SwitchPanels(currentPanel, 1);
                    break;
            }
        }
    }
    public void SwitchPanels(int panelToDeactivate, int panelToActivate)
    {
        panels[panelToDeactivate].SetActive(false);
        panels[panelToActivate].SetActive(true);
        currentPanel = panelToActivate;
    }
    public void CancelExitPrompt()
    {
        SwitchPanels(currentPanel, 1);  //1 is the regular Tittle Menu UI
    }
    public void ShowExitPrompt()
    {
        SwitchPanels(currentPanel, 0);  //0 Is the "are you sure?" prompt
    }
    public void Settings()
    {
        SwitchPanels(currentPanel, 3);  //3 is the Settings panel
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadSaves()
    {
        for(int x = 0; x < ProfileManager.instance.profileCount; x++)
        {
            GameObject newButton = Instantiate(saveButtonPrefab, scrollViewContent.transform);

            newButton.GetComponent<SaveButtonUI>().Initialize(this, x);
        }
    }
    public void SelecSave()
    {
        SwitchPanels(currentPanel, 2);

        //Updates each save button in case they become unsynced to their associated profiles
        SaveButtonUI[] saveUIList = scrollViewContent.GetComponentsInChildren<SaveButtonUI>();
        foreach (SaveButtonUI saveButton in saveUIList) saveButton.UpdateUI();
    }

    /// <summary>
    /// Method called by UI to exit the save select panel
    /// </summary>
    public void ExitSelectSave() => SwitchPanels(currentPanel, 1);

    /// <summary>
    /// Method called by UI in the Create Profile Panel when the
    /// player tries to create a new profile
    /// </summary>
    public void CreateProfile()
    {
        if (!string.IsNullOrEmpty(nameInputField.text))
        {
            ProfileManager.instance.CurrentProfile.profileID = ProfileManager.instance.currentProfileIndex;
            ProfileManager.instance.CurrentProfile.name = nameInputField.text;
            ProfileManager.instance.SaveCurrentProfile();

            //TEST CODE. LATER THIS SHOULD SWAP SCENES BUT FOR NOW I'M SWAPPING PANELS TO HELP TEST
            SwitchPanels(currentPanel, 1);
        }
        
    }
    
    /// <summary>
    /// Method called by UI in the Create Profile Panel when the player cancels creating
    /// a new profile
    /// </summary>
    public void CancelCreateProfile()
    {
        SwitchPanels(currentPanel, 2);
        ProfileManager.instance.currentProfileIndex = -1;
        nameInputField.text = "";
    }
}
