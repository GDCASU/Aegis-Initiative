using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;
using UnityEngine.UI;

public class RemarkManager : MonoBehaviour
{
    public static RemarkManager singleton;

    private GameObject passivePilot;
    private GameObject activePilot;

    [SerializeField]
    private float chanceToSpeak = 100.0f;

    public Flowchart flowChart;

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
        //tempChart = tempFeebee.GetComponent<RemarkList>().pilotFlowchart;
        //tempChart.FindBlock("Taking Damage").CommandList[0].GetComponent<Fungus.FadeUI>().targetObjects[0] = DialogueUI;
    }
    //Random float landed in proper range
    private bool DialogueSuccessful()
    {
        bool result = false;
        float rand = Random.Range(0.0f, 100.0f);

        if(rand <= chanceToSpeak)
        {
            result = true;
        }

        return result;
    }
    //choose passive or active pilot
    private void ActivateDialogue(string _input)
    {
        int temp = Random.Range(0, 2);
        Debug.Log(temp);
        if (temp == 0)
        {
            passiveRemark(_input);
        }
        else
        {
            activeRemark(_input);
        }
    }

    private void passiveRemark(string _input)
    {
        string blockToCall = passivePilot.name + _input;
        flowChart.ExecuteBlock(blockToCall);
    }

    private void activeRemark(string _input)
    {
        string blockToCall = activePilot.name + _input;
        flowChart.ExecuteBlock(blockToCall);
    }

    public void EnteringStage()
    {
        if (DialogueSuccessful())
        {
            ActivateDialogue(" Entering Stage");
        }
    }

    public void ExitingStage()
    {
        if (DialogueSuccessful())
        {
            ActivateDialogue(" Exiting Stage");
        }
    }

    public void TakingDamage()
    {
        if(DialogueSuccessful())
        {
            ActivateDialogue(" Damage");
        }
    }

    public void CollectingPickUps()
    {
        if (DialogueSuccessful())
        {
            ActivateDialogue(" Collecting Pick-Ups");
        }
    }

    public void DefeatingEnemies()
    {
        if (DialogueSuccessful())
        {
            ActivateDialogue(" Defeating Enemies");
        }
    }

    public void LoseStage()
    {
        if (DialogueSuccessful())
        {
            ActivateDialogue(" Lose Stage");
        }
    }
}
