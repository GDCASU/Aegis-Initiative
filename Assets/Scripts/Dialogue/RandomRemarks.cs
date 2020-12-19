using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;

public class RandomRemarks : MonoBehaviour
{
    public Flowchart pilotFlowchart;

    public string[] takeDamageRemarks;
    public string[] collectPickUpsRemarks;
    public string[] defeatEnemiesRemarks;
    public string[] lostStageRemarks;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(pilotFlowchart);
    }
}
