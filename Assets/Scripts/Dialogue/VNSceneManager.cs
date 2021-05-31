using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VNSceneManager : MonoBehaviour
{
    GameObject flowchart;

    private void Awake()
    {
        flowchart = GameObject.Find("Flowchart").gameObject;

        //flowchart.GetComponent<SetupFlowchart>().sceneFlowchart.ExecuteBlock(
          //  ProfileManager.instance.CurrentProfile)
    }
}
