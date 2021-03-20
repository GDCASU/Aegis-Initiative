using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferbombSpawner : MonoBehaviour
{
    public float height; //how far up to float Pufferbombs from initial y position
    public float liftTimer; //time to float Pufferbombs

    private bool floatUp;
    private Vector3 finalHeight;

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
        finalHeight = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
    }

    //Move the Pufferbombs up at the given height and time
    private IEnumerator FloatUp(float timer)
    {
        Vector3 startPos = transform.position;
        Vector3 finalPos = finalHeight;
        float liftTime = 0;

        while (liftTime < timer)
        {
            transform.position = Vector3.Lerp(startPos, finalPos, (liftTime / timer));
            liftTime += Time.deltaTime;
            yield return null;
        }
    }

    //When the player collides with the PufferbombSpawner, then enable the Pufferbomb Script of all the Pufferbombs
    private void OnTriggerEnter(Collider other)
    {
        if (!floatUp)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(FloatUp(liftTimer));
                floatUp = true;
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Pufferbomb>() != null)
                    {
                        child.GetComponent<Pufferbomb>().enabled = true;
                    }
                }
            }
        }
    }
}
