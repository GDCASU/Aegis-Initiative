using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using UnityEngine.UI;
using PlayerInput;
using FMODUnity;
using System.Runtime.Serialization;

public class PauseMenu : MonoBehaviour
{
    private string Select = "event:/SFX/UI/Select";

    public List<GameObject> panels;
    public List<string> keyboardCodes;
    public List<string> xboxCodes;
    public int currentPanel;
    public GameObject pauseMenuCanvas;

    [Header("Pause Menu")]
    #region Pause Menu
    //UI playerUI;
    public GameObject HUD;
    public GameObject reticle;
    public GameObject copilotUI;
    public GameObject settingsUiObject;
    private bool isPaused = false;
    #endregion

    // Options stuff
    [Serializable]
    public struct OptionsData
    {
        public bool fullscreen;
        public int resolution_x;
        public int resolution_y;
        public int quality;
        public Dictionary<PlayerButton, PlayerAction> playerControls;
        public float musicVolume;
        public float sfxVolume;
        public int controlType;
        public float[] extraFloat; // padding
        public bool[] extraBool; // padding
        public string[] extraString; // padding
    }

    //Steam release

    //public List<AudioSource> sfx = new List<AudioSource>();
    //public AudioSource music;
    //public static float musicVolume = 0.5f;
    //public static float sfxVolume = 0.5f;
    public static PauseMenu singleton;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        //music = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        keyboardCodes = new List<string>();
        xboxCodes = new List<string>();
        currentPanel = 0;

        keyboardCodes.Add("W");
        keyboardCodes.Add("S");
        keyboardCodes.Add("D");
        keyboardCodes.Add("A");
        currentPanel = 0;

        // Fix for steam release
        //var pOptions = GameManager.options;

        //var resolutions = Screen.resolutions;
        //var resDropdown = generalSettings.transform.Find("ResolutionDropdown").GetComponent<Dropdown>();

        //resDropdown.ClearOptions();
        //var options = resolutions.Select((r, i) => {
        //    string o = r.width + " x " + r.height;
        //    if (r.width == pOptions.resolution_x && r.height == pOptions.resolution_y) resDropdown.value = i;
        //    return o;
        //}).ToList();

        //resDropdown.AddOptions(options);
        //resDropdown.RefreshShownValue();

        //// Set quality dropdown
        //var qualityDropdown = generalSettings.transform.Find("QualityDropdown");
        //qualityDropdown.GetComponent<Dropdown>().value = pOptions.quality;

        //// fullscreen
        //var fullscreenToggle = generalSettings.transform.Find("FullscreenToggle");
        //fullscreenToggle.GetComponent<Toggle>().isOn = pOptions.fullscreen;

        //// input type
        //var inputDropdown = controlSettings.transform.Find("InputDropdown");
        //inputDropdown.GetComponent<Dropdown>().value = pOptions.controlType;

        // music
        var musicSlider = settingsUiObject.transform.Find("MusicSlider");
        musicSlider.GetComponent<Slider>().value = SoundManager.singleton.currentMusicVolume;

        // sfx
        var sfxSlider = settingsUiObject.transform.Find("SFXSlider");
        sfxSlider.GetComponent<Slider>().value = SoundManager.singleton.currentSfxVolume;
    }
    private void Update()
    {
        if (InputManager.GetButtonDown(PlayerButton.Pause) && SceneManager.GetActiveScene().name != "MainMenuUI")
        {
            if (PlayerInfo.singleton)
            {
                if (!isPaused) Pause();
                else
                {
                    if (InputManager.GetButtonDown(PlayerButton.UI_Cancel))
                    {
                        if (currentPanel == 0) ResumeGame();
                        else SwitchPanels(0);
                    }
                }
            }      
        }
    }
    public void Pause()
    {
        isPaused = true;
        //playerUI.enabled = false;
        Time.timeScale = 0;
        HUD.SetActive(false);
        reticle.SetActive(false);
        copilotUI.SetActive(false);
        pauseMenuCanvas.SetActive(true);

        //if(player!=null && player.InputMethod!=InputManager.InputMethod.XboxController) Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentPanel = 0;
        //print(panels[currentPanel].transform.GetChild(1).transform.GetChild(0).gameObject);
        //EventSystem.current.SetSelectedGameObject(panels[currentPanel].transform.GetChild(1).transform.GetChild(0).gameObject);
    }
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentPanel = 0;
        //playerUI.enabled = true;
        HUD.SetActive(true);
        reticle.SetActive(true);
        copilotUI.SetActive(true);
    }

    public void ControlSettings()
    {
        SwitchPanels(1);
        //EventSystem.current.SetSelectedGameObject(panels[currentPanel].transform.GetChild(0).gameObject);
    }

    public void GeneralSettings()
    {
        SwitchPanels(2);
        //EventSystem.current.SetSelectedGameObject(panels[currentPanel].transform.GetChild(0).gameObject);
    }
    public void GoToMainMenu()
    {
        SwitchPanels(3);
        //EventSystem.current.SetSelectedGameObject(panels[currentPanel].transform.GetChild(0).gameObject);
    }
    public void MainMenuPromptCancel()
    {
        SwitchPanels(0);
        //EventSystem.current.SetSelectedGameObject(panels[currentPanel].transform.GetChild(0).gameObject);
    }
    public void MainMenuPromptConfirm()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuUI");
    }

    public void SwitchControllerType(Dropdown change)
    {
        InputManager.inputMode = (InputManager.InputMode)change.value;
    }
    public void SwitchPanels(int panelToActivate)
    {
        SoundManager.singleton.PlayOneShot(Select, transform.position, SoundManager.VolumeType.sfx);
        panels[currentPanel].SetActive(false);
        panels[panelToActivate].SetActive(true);
        currentPanel = panelToActivate;
    }
    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    public void SetFullScreen(bool f)
    {
        Screen.fullScreen = f;
    }

    public void SetResolution(int i)
    {
        var res = Screen.resolutions[i];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    public void UpdateMusicVolume(float volume)
    {
        SoundManager.singleton.SetMusicVolume(volume);
    }
    public void UpdateHealt(int value)
    {
        HUD.GetComponent<Text>().text = "Health " + value;   
    }
    public void PlayerDied()
    {
        HUD.SetActive(false);
        reticle.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SwitchPanels(4);
    }
    public void RestartLevel()=> SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    //public void SetMusicVolume(float passed)
    //{
    //    music.volume = passed;
    //    musicVolume = passed;
    //}
    //public void SetEffectsVolume(float passed)
    //{
    //    for (int i = 0; i < sfx.Count(); i++)
    //    {
    //        sfx[i].volume = passed;
    //    }
    //    sfxVolume = passed;
    //}

    public void ClickBackButton()
    {
        MainMenuPromptCancel();
    }

}