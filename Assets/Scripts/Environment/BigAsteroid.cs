using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAsteroid : MonoBehaviour
{
    public float delta_pos; //how much to move from starting position
    public float bob_speed;
    public float spin_speed_x;
    public float spin_speed_y;
    public float spin_speed_z;
    private Vector3 start_pos;
    private Quaternion start_rot;
    void Start() 
    {
        start_pos = transform.position;
        start_rot = transform.rotation;
    }

    void Update() 
    {
        Vector3 current_pos = start_pos;
        current_pos.y += delta_pos * Mathf.Sin(Time.time * bob_speed);
        transform.position = current_pos;

        transform.Rotate(new Vector3(Time.time * (spin_speed_x/1000), Time.time * (spin_speed_y/1000), Time.time * (spin_speed_z/1000)));
    }
}
