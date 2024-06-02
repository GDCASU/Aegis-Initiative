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
                    if (Input.GetKeyDown(vKey) && !DoesKeybindExist(vKey))
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
        textUI.text = keyName;
    }

    private void SetKeyName(KeyCode newKey)
    {
        if (InputManager.xboxButtonToNameMap.ContainsKey(newKey))
        {
            keyName = InputManager.xboxButtonToNameMap[newKey];
        }
        else
        {
            keyName = newKey.ToString();
        }
    }
    public void Remaping()
    {
        StartCoroutine(timerRemaping());
    }
    private bool DoesKeybindExist(KeyCode key)
    {
        var allBinds = InputManager.allKeybinds[InputManager.InputMode.controller];
        bool isCurrKey = allBinds[action] == key;
        bool isBoundToOtherAction = allBinds.ContainsValue(key);
        return isBoundToOtherAction && !isCurrKey;
    }
    public void SetButton(KeyCode passed)
    {
        List<string> xboxCodes = GameObject.Find("Player 1 Camera").GetComponentInChildren<PauseMenu>().xboxCodes;
        if (InputManager.xboxButtonToNameMap.ContainsKey(passed))
        {
            InputManager.allKeybinds[InputManager.InputMode.controller][action] = passed;
            xboxCodes.Remove(keyName);
            SetKeyName(passed);
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

