using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;

public class RemarkList : MonoBehaviour
{
    public Flowchart pilotFlowchart;

    const int boxSize = 3;

    [Header("Stage Remarks")]
    [TextArea(boxSize, boxSize)]
    public string[] enteringStage;
    [TextArea(boxSize, boxSize)]
    public string[] exitingStage;

    [Header("Random Remarks")]
    [TextArea(boxSize, boxSize)]
    public string[] takingDamage;
    [TextArea(boxSize, boxSize)]
    public string[] collectingPickUps;
    [TextArea(boxSize, boxSize)]
    public string[] defeatingEnemies;
    [TextArea(boxSize, boxSize)]
    public string[] losingStage;

    private void Start()
    {
        Instantiate(pilotFlowchart);
    }
}
