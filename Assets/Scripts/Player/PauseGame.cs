using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public bool isPaused = false;
    public bool Esc = false;

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButton(PlayerInput.PlayerButton.Pause))
        {
            isPaused = !isPaused;
            Pause();
        }
        Esc = InputManager.GetButton(PlayerInput.PlayerButton.Pause);
    }

    void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
