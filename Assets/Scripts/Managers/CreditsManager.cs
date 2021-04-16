using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public List<Transform> Path = new List<Transform>();
    public float SpawnSpeed = 1;
    public float NameMoveSpeed = 0.1f;
    public float FaceCameraDistance;
    public Transform Camera;

    public float CreditsDelay = 2;
    public float EndCreditsScrollSpeed = 1;
    public float EndCreditsTime = 30;
    public RectTransform CreditsPanel;

    [Range(0, 1)]
    public float Position = 0;

    public CreditsNameModel.Names[] Names;
    public GameObject CreditsNameParent;

    public int LinePoints = 10;

    // Start is called before the first frame update
    void Start()
    {
        Names = CreditsNameModel.GetNames();
    }

    // Update is called once per frame
    void Update()
    {
        // Used to manually spawn names, if you are reading this in the master branch then I am dumb. Remove it please <3
        if (Input.GetKeyDown(KeyCode.N)) StartCoroutine(SpawnNames());
    }


    public Vector3 LerpPosition(float time)
    {
        Vector3[] lerps = new Vector3[Path.Count];

        for (int i = 0; i < Path.Count - 1; i++)
        {
            lerps[i] = Vector3.Lerp(Path[i].position, Path[i + 1].position, time);
        }

        lerps[lerps.Length - 1] = Path[Path.Count - 1].position;

        Vector3 position = Path[0].position;

        for (int i = 0; i < lerps.Length - 1; i++)
        {
            position = Vector3.Lerp(position, lerps[i + 1], time);
        }

        return position;
    }

    public IEnumerator SpawnNames()
    {
        for (int i=0; i<Names.Length; i++)
        {
            CreateName(Names[i].name, Names[i].title);
            yield return new WaitForSeconds(SpawnSpeed);
        }

        // Waits 2 seconds before starting the end credits
        yield return new WaitUntil(() => GameObject.Find("CreditsName(Clone)") == null);
        StartCoroutine(EndCreditsPanel());
    }

    public void CreateName(string name, string title)
    {
        CreditsNameMovement nameObject = Instantiate(CreditsNameParent, Path[0].position, Path[0].rotation).GetComponentInChildren<CreditsNameMovement>();
        nameObject.creditsManager = this;
        nameObject.Speed = NameMoveSpeed;
        nameObject.UpdateName(name, title);
    }

    public IEnumerator EndCreditsPanel()
    {
        yield return new WaitForSeconds(CreditsDelay);
        float timer = 0;

        while(timer < EndCreditsTime)
        {
            timer += Time.deltaTime;

            CreditsPanel.anchoredPosition += Vector2.up * Time.deltaTime * EndCreditsScrollSpeed;

            yield return null;
        }
    }
}
