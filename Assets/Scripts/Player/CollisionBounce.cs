﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBounce : MonoBehaviour
{
    void OnCollisionExit(Collision collision)
    {
        if (transform.localPosition.z != 0)
        {
            Vector3 newPostition = transform.localPosition;
            newPostition.z = 0;

            transform.localPosition = newPostition;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
