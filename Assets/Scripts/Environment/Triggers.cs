using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;
public class Triggers : MonoBehaviour
{
    public Flowchart flow;
    bool start = true;
    public float timer=0;
    public CinemachineDollyCart cart;

    //end of level variables
    private Vector3 endDirection;
    private GameObject playerShip;
    private bool levelFinished;
    private float endSpeed;

    private void Start()
    {
        //playerShip = GameObject.FindGameObjectWithTag("Player"); its better to make sure that is set on the inspector rather than looking for it on the start
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
            flow.ExecuteBlock("start");
            start = false;
        }
        if(other.gameObject.name == "End Trigger")
        {
            flow.ExecuteBlock("finish");
            //store final cart speed and execute end of level
            endSpeed = playerShip.GetComponent<ShipMovement>().forwardSpeed;
            endDirection = new Vector3(0, 0, endSpeed * Time.deltaTime);
            levelFinished = true;
        }
        if(other.gameObject.tag == "Dialogue_Trigger")
        {
            flow.ExecuteBlock(other.gameObject.name);
        }
    }

    private void EndLevel()
    {
        //fly ship away from camera at cart speed (can adjust later)
        playerShip.transform.Translate(endDirection);
    }
}
