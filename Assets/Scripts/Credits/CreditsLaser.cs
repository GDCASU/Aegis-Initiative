using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsLaser : MonoBehaviour
{
    public float LazerSpeed;
    public Vector3 Direction;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Direction != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Direction, Time.deltaTime * LazerSpeed);
            if (Vector3.Distance(transform.position, Direction) <= 0.1f) Destroy(gameObject);
        }
    }
}
