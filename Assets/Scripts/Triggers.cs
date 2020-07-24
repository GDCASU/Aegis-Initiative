using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Triggers : MonoBehaviour
{
    public Flowchart flow;
    bool start = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO: assert that the collision is the correct object
    // start boolean can be taken out if this is easy to do
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
        if (start)
        {
            flow.ExecuteBlock("start");
            start = false;
        }
        else //else if ()
        {
            flow.ExecuteBlock("finish");
        }
    }
}
