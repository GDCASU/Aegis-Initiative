﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class SetupFlowchart : MonoBehaviour
{
    public Flowchart pilotFlowchart;

    [HideInInspector]
    public Flowchart sceneFlowchart;

    private GameObject dialogueUI;

    private void Start()
    {
        sceneFlowchart = Instantiate(pilotFlowchart);
        dialogueUI = GameObject.Find("Pilot_Dialogue");
        SetupCommands();
    }

    private void SetupCommands()
    {
        //go through each block in the flowchart
        foreach(Block currentBlock in sceneFlowchart.GetComponents<Block>())
        {
            string portraitToChoose = currentBlock.BlockName; //string for portrait later maybe??? temp variable could be something else

            //go through each command in the current block to set them up
            foreach(Command currentCommand in currentBlock.CommandList)
            {
                switch (currentCommand)
                {
                    case FadeUI info:
                        info.targetObjects[0] = dialogueUI; //set UI to fade
                        break;
                    case Say info:
                        info._Character = gameObject.GetComponent<Character>(); //choose character
                        info.Portrait = gameObject.GetComponent<CopilotInfo>().fullBody; //CHANGE TO DESIRED PORTRAIT WHEN MORE ARE MADE
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
