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
    bool remaping;
    public Text textUI;


    private void Start()
    {
        InitiateButton();
    }
    public void Update()
    {
        if (remaping)
        {
            if (Input.anyKey)
            {
                foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(vKey))
                    {
                        SetButton(vKey);
                        remaping = false;
                    }
                }
            }
        }
    }
    public void Remaping()
    {
        remaping = true;
    }
    public void InitiateButton()
    {
        button = InputManager.playerButtons[action].keyboardKey;
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
        PlayerAction actn = InputManager.playerButtons[action];
        actn.keyboardKey = passed;
        InputManager.playerButtons[action]=actn;
        keyboardCodes.Remove(keyName);
        keyName = passed.ToString();
        keyboardCodes.Add(keyName);
        GetComponentInChildren<Text>().text = keyName;
    }

}
