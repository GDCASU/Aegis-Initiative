using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void SwitchPanels(int panelToDeactivate, int panelToActivate)
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
        for(int x =0; x<10;x++)
        {
            GameObject tempButton = Instantiate(saveButtonPrefab);
            tempButton.transform.SetParent(scrollViewContent.transform);
            tempButton.transform.localScale = Vector3.one;
        }
    }
    public void SelecSave()
    {
        SwitchPanels(currentPanel, 2);
    }
}
