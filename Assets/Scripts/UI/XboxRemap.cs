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
    private void OnDisable()
    {
        textUI.text = keyName;
        remapping = false;
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
                        remapping = false;
                    }
                }
            }
        }
    }
    public void InitiateButton()
    {   
        button = InputManager.allKeybinds[InputManager.InputMode.controller][action];
        SetKeyName(button);
    }

    private void SetKeyName(KeyCode newKey)
    {
        // use human readable keyname if it exists
        if (InputManager.xboxButtonToNameMap.ContainsKey(newKey))
        {
            keyName = InputManager.xboxButtonToNameMap[newKey];
        }
        else
        {
            keyName = newKey.ToString();
        }
        textUI.text = keyName;
    }
    public void Remaping()
    {
        StartCoroutine(timerRemaping());
    }
    private bool CanSetKeybind(KeyCode newKey)
    {
        var allBinds = InputManager.allKeybinds[InputManager.InputMode.controller];
        bool isCurrKey = allBinds[action] == newKey;
        bool isBoundToOtherAction = allBinds.ContainsValue(newKey);
        bool result = !isBoundToOtherAction || isCurrKey;

        return result;
    }
    public void SetButton(KeyCode passed)
    {
        if (InputManager.xboxButtonToNameMap.ContainsKey(passed))
        {
            InputManager.SetKeybind(InputManager.InputMode.controller, action, passed);
            SetKeyName(passed);
        }
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

