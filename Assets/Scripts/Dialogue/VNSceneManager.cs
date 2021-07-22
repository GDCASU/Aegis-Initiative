using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class VNSceneManager : MonoBehaviour
{
    Flowchart flowchart ;
    string copilotName;
    ProfileManager profileManager = ProfileManager.instance;

    private void Awake()
    {
        flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();

        copilotName = SceneManager.GetActiveScene().name;

        int vnNumber = GameManager.singleton.activeCopilot.GetComponent<CopilotInfo>().copilotData.vnScenesCompleted;
        //print(flowchart.GetComponent<SetupFlowchart>().sceneFlowchart.FindBlock(copilotName + vnNumber));
        //if dialogue box exists, execute it
        if (flowchart.FindBlock(copilotName + vnNumber))
        {
            //execute block with copilotname and current level of copilot
            flowchart.ExecuteBlock(copilotName + vnNumber);
        }
        else
        {
            //player has no VN scenes left
            //This should save the profile then load the next gameplay scene
            profileManager.SaveCurrentProfile();
            SceneManager.LoadScene(GameManager.singleton.levels[profileManager.CurrentProfile.currentStage]);
        }
    }

    //update completed copilot VNs
    //Load next scene after VN
    public void EndVNScene()
    {
        //update this pilots completed VNscenes by 1
        GameManager.singleton.activeCopilot.GetComponent<CopilotInfo>().copilotData.vnScenesCompleted += 1;

        GameManager.singleton.SaveProgress();
        profileManager.SaveCurrentProfile();

        //This should load the next gameplay level
        SceneManager.LoadScene("CopilotUI");
    }
}
