using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnorTrail : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(pos1.position, Vector3.forward, Time.deltaTime * speed);
        transform.position += transform.up * Time.deltaTime * speed * -1;
        
    }
}
