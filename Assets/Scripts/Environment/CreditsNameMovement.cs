using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsNameMovement : MonoBehaviour
{
    public bool CanMove = true;
    public float Speed = 1f;
    public CreditsManager creditsManager;
    public TextMesh TitleText;
    public TextMesh NameText;

    private Vector3 previousPoint = Vector3.zero;
    private float position = 0;

    // Start is called before the first frame update
    void Start()
    {
        TitleText.gameObject.AddComponent<BoxCollider>().size += new Vector3(0, 0, 20f);
        NameText.gameObject.AddComponent<BoxCollider>().size += new Vector3(0, 0, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove) Move();
    }

    public void UpdateName(string name, string title)
    {
        NameText.text = name;
        TitleText.text = title;
    }

    public void Move()
    {
        position += Time.deltaTime * Speed;
        Vector3 newPosition = creditsManager.LerpPosition(position);

        if (Vector3.Distance(creditsManager.Camera.position, transform.position) > creditsManager.FaceCameraDistance)
        {
            FaceInDirection(newPosition);
        }
        else
        {
            FaceInDirection(creditsManager.Camera.position);
        }

        transform.position = newPosition;

        if (position > 1) Destroy(gameObject);
    }

    public void FaceInDirection(Vector3 direction)
    {
        //transform.LookAt(direction);
        var targetRotation = Quaternion.LookRotation(direction - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    public void Shot()
    {
        NameText.color = Color.red;
        TitleText.color = Color.red;
        NameText.GetComponent<BoxCollider>().enabled = false;
        TitleText.GetComponent<BoxCollider>().enabled = false;
    }
}
