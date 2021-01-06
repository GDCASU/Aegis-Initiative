using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;
using UnityEngine.UI;

public class RemarkManager : MonoBehaviour
{
    private GameObject passivePilot;
    private GameObject activePilot;

    public GameObject DialogueUI;
    public GameObject nameText;
    public GameObject imageBlock;

    public GameObject tempFeebee;

    float timer = 5.0f;
    public Flowchart tempChart;

    // Start is called before the first frame update
    void Start()
    {
        nameText.GetComponent<Text>().text = "Feebee";
        //tempChart = tempFeebee.GetComponent<RemarkList>().pilotFlowchart;
        tempChart.FindBlock("Taking Damage").CommandList[0].GetComponent<Fungus.FadeUI>().targetObjects[0] = DialogueUI;
        tempChart.ExecuteBlock("Taking Damage");
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0)
        {
            //tempChart.FindBlock("Taking Damage").CommandList[0].GetComponent<Fungus.FadeUI>().targetObjects[0] = DialogueUI;
            //tempChart.FindBlock("Test_Level_Remark");
            //tempChart.ExecuteBlock("Test_Level_Remark");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void TakingDamageDialogue()
    {

    }
}
