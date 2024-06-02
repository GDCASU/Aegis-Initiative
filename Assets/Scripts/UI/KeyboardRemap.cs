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
                    if (Input.GetKeyDown(vKey) && CanSetKeybind(vKey))
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
        SetKeyName(button);
    }

    private void SetKeyName(KeyCode newKey)
    {
        // use human readable keyname if it exists
        if (InputManager.mouseButtonToNameMap.ContainsKey(newKey))
        {
            keyName = InputManager.mouseButtonToNameMap[newKey];
        }
        else
        {
            keyName = newKey.ToString();
        }
        textUI.text = keyName;
    }

    private bool CanSetKeybind(KeyCode newKey)
    {
        var allBinds = InputManager.allKeybinds[InputManager.InputMode.keyboard];
        bool isCurrKey = allBinds[action] == newKey;
        bool isBoundToOtherAction = allBinds.ContainsValue(newKey);
        bool result = !isBoundToOtherAction || isCurrKey;

        return result;
    }
    public void SetButton(KeyCode passed)
    {
        InputManager.SetKeybind(InputManager.InputMode.keyboard, action, passed);
        SetKeyName(passed);
    }

}
