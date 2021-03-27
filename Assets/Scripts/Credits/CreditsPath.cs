using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPath : MonoBehaviour
{
    public LineRenderer pathLine;
    public CreditsManager creditsManager;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++) creditsManager.Path.Add(transform.GetChild(i));
        pathLine = GetComponent<LineRenderer>();
        creditsManager = GetComponent<CreditsManager>();
        pathLine.positionCount = creditsManager.LinePoints;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLineRender();
    }

    private void UpdateLineRender()
    {
        for (int i = 0; i < creditsManager.LinePoints; i++)
        {
            pathLine.SetPosition(i, creditsManager.LerpPosition((float)i / (creditsManager.LinePoints - 1)));
        }
    }
}
