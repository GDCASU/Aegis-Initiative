using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;
using UnityEngine.UI;

public class RemarkManager : MonoBehaviour
{
    public static RemarkManager singleton;
    
    [Header("Booleans")]
    [HideInInspector]
    public bool importantDialogueActive = false;

    private GameObject passivePilot;
    private GameObject activePilot;
    public Flowchart passiveChart;
    public Flowchart activeChart;

    private Block lastBlock;

    [Header("Adjustments")]
    [SerializeField]
    private float chanceToSpeak = 100.0f;

    [Header("Flowchart")]
    public Flowchart storyFlowchart;

    private void Awake() //set singletone
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    //Random float landed in proper range
    private bool DialogueSuccessful()
    {
        passiveChart = SelectedCopilots.singleton.passiveCopilot.GetComponent<SetupFlowchart>().sceneFlowchart;
        activeChart = SelectedCopilots.singleton.activeCopilot.GetComponent<SetupFlowchart>().sceneFlowchart;

        bool result = false;
        float rand = Random.Range(0.0f, 100.0f);

        if (!importantDialogueActive && rand <= chanceToSpeak)
        {
            result = true;
        }

        return result;
    }
    //choose passive or active pilot
    public void ActivatePilotRemark(string _input)
    {
        //make sure old dialogue does not effect new dialogue
        if(lastBlock != null)
        {
            if(lastBlock.IsExecuting())
            {
                lastBlock.Stop();
            }
        }

        int temp = Random.Range(0, 2);
        if (temp == 0)
        {
            //string blockToCall = passivePilot.name + _input;
            lastBlock = passiveChart.FindBlock(_input);
            passiveChart.ExecuteBlock(_input);
        }
        else
        {
            //string blockToCall = activePilot.name + _input;
            lastBlock = activeChart.FindBlock(_input);
            activeChart.ExecuteBlock(_input);
        }
    }

    public void SetImportantDialogue(bool input)
    {
        importantDialogueActive = input;
    }

    /// <summary>
    /// STORY DIALOGUE BELOW HERE -----------------------------
    /// </summary>
    /// <param name="_input"></param>
    /// <param name="_pilot"></param>

    public void ExecuteDialogue(string _input, GameObject _pilot, DialogueTriggers.Emotion _pilotEmotion)
    {
        //make sure old dialogue does not effect new dialogue
        if (lastBlock != null)
        {
            if (lastBlock.IsExecuting())
            {
                lastBlock.Stop();
            }
        }

        lastBlock = storyFlowchart.FindBlock(_input); //grab executing block
        //change character/portrait to active or passive
        foreach (Command currCommand in lastBlock.CommandList)
        {
            switch (currCommand)
            {
                case Say info:
                    info._Character = _pilot.GetComponent<Character>(); //choose character
                    info.Portrait = Resources.Load<Sprite>(@"Sprites\Icons\" + _pilot.GetComponent<Character>().NameText + "_" + _pilotEmotion.ToString() + "_i");
                    //info.Portrait = _pilot.GetComponent<CopilotInfo>().fullBody; //CHANGE TO DESIRED PORTRAIT WHEN MORE ARE MADE
                    break;
                default:
                    break;
            }
        }
        storyFlowchart.ExecuteBlock(_input);
    }

    //-----------------------------------------------

    /// <summary>
    /// Random remark functions to call are below here --------------------------------
    /// </summary>
    public void EnteringStage()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotRemark("Entering Stage");
        }
    }

    public void ExitingStage()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotRemark("Exiting Stage");
        }
    }

    public void TakingDamage()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotRemark("Taking Damage");
        }
    }

    public void CollectingPickUps()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotRemark("Collecting Pick-Ups");
        }
    }

    public void DefeatingEnemies()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotRemark("Defeating Enemies");
        }
    }

    public void LoseStage()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotRemark("Lose Stage");
        }
    }
}
