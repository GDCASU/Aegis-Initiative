using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public Transform[] Path;
    public float SpawnSpeed = 1;
    public float NameMoveSpeed = 0.1f;

    [Range(0, 1)]
    public float Position = 0;

    public string[] Names;
    public GameObject CreditsNameParent;

    [SerializeField]
    private int LinePoints = 10;
    private LineRenderer pathLine;

    // Start is called before the first frame update
    void Start()
    {
        Path = GetComponentsInChildren<Transform>();
        pathLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLineRender();

        // Used to manually spawn names, if you are reading this in the master branch then I am dumb. Remove it please <3
        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(SpawnNames());
    }


    public Vector3 LerpPosition(float time)
    {
        Vector3[] lerps = new Vector3[Path.Length];

        for (int i = 1; i < Path.Length; i++)
        {
            if (i < Path.Length - 1)
            {
                lerps[i] = Vector3.Lerp(Path[i].position, Path[i + 1].position, time);
            }
        }

        lerps[lerps.Length - 1] = lerps[0];

        for (int i = 0; i < lerps.Length; i++)
        {
            if (i < lerps.Length - 1) lerps[lerps.Length - 1] = Vector3.Lerp(lerps[lerps.Length - 1], lerps[i + 1], time);
        }

        return lerps[lerps.Length - 1];
    }

    public IEnumerator SpawnNames()
    {
        for (int i=0; i<Names.Length; i++)
        {
            CreateName(Names[i]);
            yield return new WaitForSeconds(SpawnSpeed);
        }
    }

    public void CreateName(string name)
    {
        CreditsNameMovement nameObject = Instantiate(CreditsNameParent).GetComponentInChildren<CreditsNameMovement>();
        nameObject.creditsManager = this;
        nameObject.Speed = NameMoveSpeed;
        nameObject.UpdateName(name);
    }

    private void UpdateLineRender()
    {
        pathLine.positionCount = LinePoints;
        Vector3[] pointPositions = new Vector3[LinePoints];

        for (int i = 0; i < LinePoints; i++)
        {
            pointPositions[i] = LerpPosition((float)i / (LinePoints - 1));
        }

        pathLine.SetPositions(pointPositions);
    }
}
