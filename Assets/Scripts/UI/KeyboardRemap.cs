using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerInput;

public class KeyboardRemap : MonoBehaviour
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

    private void OnDisable()
    {
        textUI.text = keyName;
        SetRemapping(false);
    }

    public void Update()
    {
        if (remapping)
        {
            textUI.text = "";
            if (Input.anyKey)
            {
                foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(vKey))
                    {
                        SetButton(vKey);
                        SetRemapping(false);
                    }
                }
            }
        }
    }
    public void SetRemapping(bool _remapping)
    {
        remapping = _remapping;
    }
    public void InitiateButton()
    {
        button = InputManager.allKeybinds[InputManager.InputMode.keyboard][action];
        keyName = button.ToString();
        textUI.text = keyName;
    }
    public void SetButton(KeyCode passed)
    {
        List<string> keyboardCodes = PauseMenu.singleton.keyboardCodes;

        foreach (string key in keyboardCodes)
        {
            if (passed.ToString() == key)
            {
                return;
            }
        }
        InputManager.allKeybinds[InputManager.InputMode.keyboard][action] = passed;
        keyboardCodes.Remove(keyName);
        keyName = passed.ToString();
        keyboardCodes.Add(keyName);
        GetComponentInChildren<Text>().text = keyName;
        InputManager.SaveKeybinds();
    }

}
