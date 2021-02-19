using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferbombSpawner : MonoBehaviour
{
    public float height; //how far up to float Pufferbombs from initial y position
    public float speed; //speed to float Pufferbombs

    private bool floatUp;
    private float finalHeight;

    void Start()
    {
        //disable Pufferbomb script for all Pufferbombs
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Pufferbomb>() != null)
            {
                child.GetComponent<Pufferbomb>().enabled = false;
            }
        }

        floatUp = false;
        finalHeight = transform.position.y + height;
    }

    void Update()
    {
        //Move the Pufferbombs up at the given speed
        if (floatUp && (transform.position.y < finalHeight))
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }

    //When the player collides with the PufferbombSpawner, then enable the Pufferbomb Script of all the Pufferbombs
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Pufferbomb>() != null)
                {
                    child.GetComponent<Pufferbomb>().enabled = true;
                }
            }
            floatUp = true;
        }
    }
}
