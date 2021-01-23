using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggers : MonoBehaviour
{
    //for active or passive
    public enum DialoguePilot
    {
        active,
        passive
    };

    //for selection emotion artwork
    //add more emotions later
    public enum Emotion
    {
        Default,
        Worried,
        etc
    };

    //for hidden story dialogue if player has that pilot active
    public enum HiddenPilot
    {
        Any,
        DaddyLongLegs,
        Feebee,
        TestNonExistantPilot
    };

    [Header("Choose Active or Passive")]
    public DialoguePilot chosenPilot;

    [Header("Emotion for Pilot")]
    public Emotion pilotEmotion;

    [Header("Hidden Dialogue Choice")]
    public HiddenPilot hiddenPilot;
}
