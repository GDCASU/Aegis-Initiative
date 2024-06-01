using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;
using System;
namespace PlayerInput {
    /// <summary>
    /// Enum for all Player axis actions (float vales)
    /// </summary>
    public enum PlayerAxis
    {
        MoveHorizontal,
        MoveVertical,
        CameraHorizontal,
        CameraVertical,
        UI_Horizontal,
        UI_Vertical,
        None,
    };

    /// <summary>
    /// Enum for all Player button actions (bool values)
    /// </summary>
    [Serializable]
    public enum PlayerButton
    {
        ActiveAbility,
        Shoot,
        UI_Submit,
        UI_Cancel,
        Pause,
        None,
    }; 

    [Serializable]
    public struct PlayerAction 
    {
        public KeyCode keyboardKey;
        public KeyCode xboxKey;
    }
}

public class InputManager : MonoBehaviour {
    public readonly static string keybindFilePath = "/keybinds.data";

    public enum InputMode
    {
        keyboard,
        controller,
        both
    }
    public static int inputType;
    public static InputMode inputMode = InputMode.keyboard;
    // there is literally no reason for this to exist ffs
    [SerializeField]
    public static PlayerAction[] playerActions = new PlayerAction[5];

    public static Dictionary<PlayerButton, KeyCode> defaultKeyboardBinds = new Dictionary<PlayerButton, KeyCode> {
            {PlayerButton.ActiveAbility, KeyCode.LeftShift},
            {PlayerButton.Shoot, KeyCode.Mouse0},
            {PlayerButton.UI_Submit, KeyCode.KeypadEnter},
            {PlayerButton.UI_Cancel, KeyCode.Escape},
            {PlayerButton.Pause, KeyCode.Escape},
    };

    public static Dictionary<PlayerButton, KeyCode> defaultControllerBinds = new Dictionary<PlayerButton, KeyCode> {
            {PlayerButton.ActiveAbility, KeyCode.JoystickButton2},
            {PlayerButton.Shoot, KeyCode.JoystickButton0},
            {PlayerButton.UI_Submit, KeyCode.JoystickButton0},
            {PlayerButton.UI_Cancel, KeyCode.JoystickButton1},
            {PlayerButton.Pause, KeyCode.JoystickButton7},
    };

    public static Dictionary<PlayerButton, KeyCode> keyboardBinds = defaultKeyboardBinds;
    public static Dictionary<PlayerButton, KeyCode> controllerBinds = defaultControllerBinds;

    public static Dictionary<InputMode, Dictionary<PlayerButton, KeyCode>> allKeybinds = new Dictionary<InputMode, Dictionary<PlayerButton, KeyCode>> {
            {InputMode.controller, controllerBinds},
            {InputMode.keyboard, keyboardBinds},
    };

    public static Dictionary<KeyCode, string> playerXboxButtons = new Dictionary<KeyCode, string> {
        {KeyCode.JoystickButton0, "A"},
        {KeyCode.JoystickButton1, "B"},
        {KeyCode.JoystickButton2, "X"},
        {KeyCode.JoystickButton3, "Y"},
        {KeyCode.JoystickButton4,"Left Bumper"},
        {KeyCode.JoystickButton5, "Right Bumper"},
        {KeyCode.JoystickButton6, "Back"},
        {KeyCode.JoystickButton7, "Start"},
        {KeyCode.JoystickButton8, "L3"},
        {KeyCode.JoystickButton9, "R3"},
    };
    public static Dictionary<PlayerButton, PlayerAction> playerButtons = new Dictionary<PlayerButton, PlayerAction> {};
    public static Dictionary<PlayerAxis, string > joyAxis = new Dictionary <PlayerAxis, string> {
        {PlayerAxis.MoveHorizontal, "JoystickX1"},
        {PlayerAxis.MoveVertical, "JoystickY1"},
        {PlayerAxis.CameraHorizontal, "JoystickT1"},
        {PlayerAxis.CameraVertical, "JoystickZ1"},
        {PlayerAxis.UI_Horizontal, "JoystickX1"},
        {PlayerAxis.UI_Vertical, "JoystickY1"},
    };

    public static Dictionary<PlayerAxis, string > mouseAxis = new Dictionary <PlayerAxis, string> {
        {PlayerAxis.MoveHorizontal, "KeyboardX"},
        {PlayerAxis.MoveVertical, "KeyboardY"},
        {PlayerAxis.CameraHorizontal, "MouseX"},
        {PlayerAxis.CameraVertical, "MouseY"},
        {PlayerAxis.UI_Horizontal, "KeyboardX"},
        {PlayerAxis.UI_Vertical, "KeyboardY"},
    };

    private void Start()
    {
        allKeybinds = LoadKeybinds();
    }

    public static bool GetButtonDown(PlayerButton button) {
        return Input.GetKeyDown(allKeybinds[inputMode][button]);
    }

    public static bool GetButtonUp (PlayerButton button) {
        return Input.GetKeyUp(allKeybinds[inputMode][button]);
    }

    public static bool GetButton (PlayerButton button) {
        return Input.GetKey(allKeybinds[inputMode][button]);
    }
    public static float GetAxis (PlayerAxis axis) {
        var mouse = mouseAxis.ContainsKey(axis) ? Input.GetAxis(mouseAxis[axis]) : 0;
        var controller = joyAxis.ContainsKey(axis) ? Input.GetAxis(joyAxis[axis]) : 0;
        
        return (inputMode == InputMode.both && controller != 0) || inputMode == InputMode.controller ? controller : mouse;
    }

    public static void SaveKeybinds ()
    {
        SaveManager.SaveContent(allKeybinds, keybindFilePath);
    }

    public static Dictionary<InputMode, Dictionary<PlayerButton, KeyCode>> LoadKeybinds ()
    {
        var keybinds = SaveManager.LoadContent(keybindFilePath);

        if(keybinds == null)
        {
            SaveManager.SaveContent(allKeybinds, keybindFilePath);
            return allKeybinds;
        }

        return keybinds as Dictionary<InputMode, Dictionary<PlayerButton, KeyCode>>;
    }
}