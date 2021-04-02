using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsLaser : MonoBehaviour
{
    public float LazerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * LazerSpeed;
    }
}
