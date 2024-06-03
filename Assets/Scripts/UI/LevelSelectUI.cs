using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectUI : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Mushroom()
    {
        GameManager.singleton.levelSelected = 0;
        SceneManager.LoadScene("CopilotUI");
    }
    public void Volcano()
    {
        GameManager.singleton.levelSelected = 1;
        SceneManager.LoadScene("CopilotUI");
    }
    public void Asteroid()
    {
        GameManager.singleton.levelSelected = 2;
        SceneManager.LoadScene("CopilotUI");
    }
    public void Pirate()
    {
        GameManager.singleton.levelSelected = 3;
        SceneManager.LoadScene("CopilotUI");
    }

    public void BackToMainMenu()
    {
        SoundManager.singleton.PlayOneShot(SoundManager.sfxMap[SoundManager.SFX.Select], transform.position, SoundManager.VolumeType.sfx);
        SceneManager.LoadScene("MainMenuUI", LoadSceneMode.Single);
    }
}
