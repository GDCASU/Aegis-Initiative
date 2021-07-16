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
    public void InitiateButton()
    {   
        button = InputManager.playerButtons[action].xboxKey;
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
            PlayerAction actn = InputManager.playerButtons[action];
            actn.xboxKey = passed;
            InputManager.playerButtons[action] = actn;
            xboxCodes.Remove(keyName);
            keyName = passed.ToString();
            xboxCodes.Add(keyName);
            GetComponentInChildren<Text>().text = keyName;
        }        
    }
    public IEnumerator timerRemaping()
    {
        for (int x = 0; x < 10; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        remaping = true;
    }
}

