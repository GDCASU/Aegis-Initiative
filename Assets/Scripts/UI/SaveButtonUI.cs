using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script with the Save Button UI to handle profile selection,
/// creation, and deletion
/// </summary>
public class SaveButtonUI : MonoBehaviour
{
    public MenuUI menuUI;

    public Text buttonText;
    public Button deleteButton;

    public int saveID = -1;

    /// <summary>
    /// Method called when instantiated to set references
    /// </summary>
    /// <param name="saveID">Index/ID of the button in the UI</param>
    public void Initialize(MenuUI menuUI, int saveID)
    {
        this.menuUI = menuUI;
        this.saveID = saveID;
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        //Adjust UI for an empty profile
        if (ProfileManager.instance.profiles[saveID].profileID == -1)
        {
            buttonText.text = "New Game...";
            
            deleteButton.gameObject.SetActive(false);
        }
        //Adjusts UI for a saved profile
        else
        {
            buttonText.text = ProfileManager.instance.profiles[saveID].name;
            deleteButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Method called by UI when the save button has been selected
    /// </summary>
    public void OnSelectSaveButton()
    {
        ProfileManager.instance.currentProfileIndex = saveID;

        //Starts profile creation process
        if (ProfileManager.instance.CurrentProfile.profileID == -1)
        {
            menuUI.nameInputField.text = "";
            menuUI.SwitchPanels(4);
        }
        else print("Profile: " + ProfileManager.instance.CurrentProfile.name + " selected");
    }

    /// <summary>
    /// Method called by UI when the player tries to delete the profile
    /// </summary>
    public void DeleteProfile()
    {
        ProfileManager.instance.DeleteProfile(saveID);
        UpdateUI();
    }
}
