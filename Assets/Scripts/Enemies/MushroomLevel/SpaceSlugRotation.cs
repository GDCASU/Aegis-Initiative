using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSlugRotation : MonoBehaviour
{
    public float rotationAmount;
    public bool clockWise;
    // Update is called once per frame
    void Update()
    {
            transform.Rotate(Vector3.right * rotationAmount * ((clockWise) ? 1 : -1),Space.Self);
    }
}
