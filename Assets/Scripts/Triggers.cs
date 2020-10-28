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

    void Update()
    {
        if (timer <= 0) cart.enabled = true;
        else timer = timer - Time.deltaTime;

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
        }
    }
}
