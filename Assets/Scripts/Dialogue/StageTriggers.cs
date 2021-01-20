using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;
public class StageTriggers : MonoBehaviour
{
    public Flowchart flow;
    bool start = true;
    public float timer=0;
    public CinemachineDollyCart cart;

    //end of level variables
    private Vector3 endDirection;
    public GameObject playerShip;
    private bool levelFinished;
    private float endSpeed;

    private void Start()
    {
        levelFinished = false;
    }

    void Update()
    {
        if (timer <= 0) cart.enabled = true;
        else timer = timer - Time.deltaTime;

        if(levelFinished)
        {
            EndLevel();
        }
    }

    // TODO: assert that the collision is the correct object
    // start boolean can be taken out if this is easy to do
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name=="Start Trigger")
        {
            //flow.ExecuteBlock("start");
            start = false;
        }
        if(other.gameObject.name == "End Trigger")
        {
            //flow.ExecuteBlock("finish");
            //store final cart speed and execute end of level
            endSpeed = playerShip.GetComponent<ShipMovement>().forwardSpeed;
            endDirection = new Vector3(0, 0, endSpeed * Time.deltaTime);
            levelFinished = true;
        }
        if(other.gameObject.tag == "Dialogue_Trigger")
        {
            GameObject passivePilot = gameObject.GetComponent<SelectedCopilots>().passive.gameObject;
            GameObject activePilot = gameObject.GetComponent<SelectedCopilots>().active.gameObject;
            string specialName = other.gameObject.GetComponent<DialogueTriggers>().hiddenPilot.ToString();
            //check if it is hidden dialogue or not
            if(specialName.CompareTo("Any") != 0)
            {
                //does the player have the hidden pilot selected
                if(passivePilot.name.CompareTo(specialName) == 0)
                {
                    RemarkManager.singleton.ActivateHiddenDialogue(other.gameObject.name, passivePilot);
                }
                else if(activePilot.name.CompareTo(specialName) == 0)
                {
                    RemarkManager.singleton.ActivateHiddenDialogue(other.gameObject.name, activePilot);
                }
            }
            else //it is not hidden dialogue
            {
                //check chosen pilot on dialogue trigger
                if(other.gameObject.GetComponent<DialogueTriggers>().chosenPilot == DialogueTriggers.DialoguePilot.passive)
                {
                    RemarkManager.singleton.ActivateStoryDialogue(other.gameObject.name, passivePilot);
                }
                else
                {
                    RemarkManager.singleton.ActivateStoryDialogue(other.gameObject.name, activePilot);
                }
            }
        }
    }

    private void EndLevel()
    {
        //fly ship away from camera at cart speed (can adjust later)
        playerShip.transform.Translate(endDirection);
    }
}
