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

    private float position = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        transform.position = creditsManager.LerpPosition(position);

        if (position > 1) Destroy(gameObject);
    }
}
