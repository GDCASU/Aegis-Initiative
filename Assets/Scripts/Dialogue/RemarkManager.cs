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
    //[HideInInspector]
    public bool dialogueRunning = false;
    public bool importantDialogueActive = false;

    private GameObject passivePilot;
    private GameObject activePilot;
    private Flowchart passiveChart;
    private Flowchart activeChart;

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

    void Start()
    {
        passivePilot = PlayerHealth.singleton.GetComponent<SelectedCopilots>().passive.gameObject;
        activePilot = PlayerHealth.singleton.GetComponent<SelectedCopilots>().active.gameObject;

        //THIS IS HARD CODED
        //CHANGE LATER
        //HARDCODED
        //WEE WOO WEE WOO
        //CHANGE LATER AWAY FROM FIND AAAAAAAHHHHHHHHH!!!!!!!!!!!!!!!
        passiveChart = GameObject.Find("DaddyLongLegs").GetComponent<SetupFlowchart>().sceneFlowchart;
        activeChart = GameObject.Find("Feebee").GetComponent<SetupFlowchart>().sceneFlowchart;
    }

    //Random float landed in proper range
    private bool DialogueSuccessful()
    {
        bool result = false;
        float rand = Random.Range(0.0f, 100.0f);

        if (!importantDialogueActive && rand <= chanceToSpeak)
        {
            result = true;
        }

        return result;
    }
    //choose passive or active pilot
    public void ActivatePilotDialogue(string _input)
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

    public void ActivateHiddenDialogue(string _input, GameObject _pilot)
    {
        ExecuteDialogue(_input, _pilot);
    }

    public void ActivateStoryDialogue(string _input, GameObject _pilot)
    {
        ExecuteDialogue(_input, _pilot);
    }

    private void ExecuteDialogue(string _input, GameObject _pilot)
    {
        //make sure old dialogue does not effect new dialogue
        if (lastBlock != null)
        {
            if (lastBlock.IsExecuting())
            {
                lastBlock.Stop();
            }
        }

        lastBlock = storyFlowchart.FindBlock(_input);
        foreach(Command currCommand in lastBlock.CommandList)
        {
            switch(currCommand)
            {
                case Say info:
                    info._Character = _pilot.GetComponent<Character>(); //choose character
                    info.Portrait = _pilot.GetComponent<CopilotInfo>().fullBody; //CHANGE TO DESIRED PORTRAIT WHEN MORE ARE MADE
                    break;
                default:
                    break;
            }
        }
        storyFlowchart.ExecuteBlock(_input);
    }

    public void SetImportantDialogue(bool input)
    {
        importantDialogueActive = input;
    }

    /// <summary>
    /// Random remark functions to call are below here --------------------------------
    /// </summary>
    public void EnteringStage()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotDialogue(" Entering Stage");
        }
    }

    public void ExitingStage()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotDialogue(" Exiting Stage");
        }
    }

    public void TakingDamage()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotDialogue("Taking Damage");
        }
    }

    public void CollectingPickUps()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotDialogue(" Collecting Pick-Ups");
        }
    }

    public void DefeatingEnemies()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotDialogue(" Defeating Enemies");
        }
    }

    public void LoseStage()
    {
        if (DialogueSuccessful())
        {
            ActivatePilotDialogue(" Lose Stage");
        }
    }
}
