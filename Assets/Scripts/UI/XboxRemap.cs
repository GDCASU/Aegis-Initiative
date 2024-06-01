using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerInput;
using System.Linq;

public class XboxRemap : MonoBehaviour
{
    public PlayerButton action;
    KeyCode button;
    int index;
    public string keyName;
    bool remapping;
    public Text textUI;
    private void Start()
    {
        InitiateButton();
    }

    public void Update()
    {
        if (remapping)
        {
             textUI.text = "";
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                textUI.text = keyName;
                return;
            }
            if (Input.anyKey)
            {
                foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(vKey))
                    {
                        SetButton(vKey);
                        remapping = false;
                    }
                }
            }
        }
    }
    public void InitiateButton()
    {   
        button = InputManager.allKeybinds[InputManager.InputMode.controller][action];
        keyName = InputManager.playerXboxButtons[button];
        keyName = button.ToString();
        textUI.text = keyName;
    }
    public void Remaping()
    {
        StartCoroutine(timerRemaping());
    }
    public void SetButton(KeyCode passed)
    {
        List<string> xboxCodes = GameObject.Find("Player 1 Camera").GetComponentInChildren<PauseMenu>().xboxCodes;
        foreach (string xKey in xboxCodes)
        {
            if (passed.ToString() == xKey)
            {
                return;
            }
        }
        if (InputManager.playerXboxButtons.ContainsKey(passed))
        {
            InputManager.allKeybinds[InputManager.InputMode.controller][action] = passed;
            xboxCodes.Remove(keyName);
            keyName = passed.ToString();
            xboxCodes.Add(keyName);
            GetComponentInChildren<Text>().text = keyName;
        }
        InputManager.SaveKeybinds();
    }
    public IEnumerator timerRemaping()
    {
        for (int x = 0; x < 10; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        remapping = true;
    }
}

