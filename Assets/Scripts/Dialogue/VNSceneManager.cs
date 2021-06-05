using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VNSceneManager : MonoBehaviour
{
    GameObject flowchart;
    string copilotName;
    ProfileManager profileManager = ProfileManager.instance;

    private void Awake()
    {
        flowchart = GameObject.Find("Flowchart").gameObject;

        copilotName = SceneManager.GetActiveScene().name;

        //check if copilot exists in dictionary
        if(profileManager.CurrentProfile.copilotVNsComplete.ContainsKey(copilotName))
        {
            //if dialogue box exists, execute it
            if(flowchart.GetComponent<SetupFlowchart>().sceneFlowchart.FindBlock(copilotName + profileManager.CurrentProfile.copilotVNsComplete[copilotName]))
            {
                //execute block with copilotname and current level of copilot
                flowchart.GetComponent<SetupFlowchart>().sceneFlowchart.ExecuteBlock(
                    copilotName + profileManager.CurrentProfile.copilotVNsComplete[copilotName]);
            }
            else
            {
                //TO DO !!!!!!!!!!!
                //continue to next scene because all pilot dialogue is complete
                //SceneManager.LoadScene();

                //this might work? idfk
                profileManager.SaveCurrentProfile();
                SceneManager.LoadScene(profileManager.CurrentProfile.currentStage);
            }
        }
        else
        {
            //play first dialogue block
            flowchart.GetComponent<SetupFlowchart>().sceneFlowchart.ExecuteBlock(copilotName + "0");
        }
    }

    //update completed copilot VNs
    //Load next scene after VN
    public void EndVNScene()
    {
        //update this pilots completed VNscenes by 1
        profileManager.CurrentProfile.copilotVNsComplete[copilotName] =
            profileManager.CurrentProfile.copilotVNsComplete[copilotName] + 1;

        profileManager.SaveCurrentProfile();

        //TO DO !!!!!!!!!!!!!!!!!!!!!
        //find a way to load next scen that the player needs to complete
        //SceneManager.LoadScene()
        //this might work? idfk
        SceneManager.LoadScene(profileManager.CurrentProfile.currentStage);
    }
}
