using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VNSceneManager : MonoBehaviour
{
    GameObject flowchart;
    string copilotName;

    private void Awake()
    {
        flowchart = GameObject.Find("Flowchart").gameObject;

        copilotName = SceneManager.GetActiveScene().name;

        //check if copilot exists in dictionary
        if(ProfileManager.instance.CurrentProfile.copilotVNsComplete.ContainsKey(copilotName))
        {
            if(flowchart.GetComponent<SetupFlowchart>().sceneFlowchart.FindBlock(copilotName + ProfileManager.instance.CurrentProfile.copilotVNsComplete[copilotName]))
            {
                //execute block with copilotname and current level of copilot
                flowchart.GetComponent<SetupFlowchart>().sceneFlowchart.ExecuteBlock(
                    copilotName + ProfileManager.instance.CurrentProfile.copilotVNsComplete[copilotName]);
            }
            else
            {
                //TO DO !!!!!!!!!!!
                //continue to next scene because all pilot dialogue is complete
                //SceneManager.LoadScene();
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
        ProfileManager.instance.CurrentProfile.copilotVNsComplete[copilotName] = 
            ProfileManager.instance.CurrentProfile.copilotVNsComplete[copilotName] + 1;

        //TO DO !!!!!!!!!!!!!!!!!!!!!
        //find a way to load next scen that the player needs to complete
        //SceneManager.LoadScene()
    }
}
