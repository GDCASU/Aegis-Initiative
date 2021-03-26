using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Pause))
        {
            isPaused = !isPaused;
            Pause();
        }
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
