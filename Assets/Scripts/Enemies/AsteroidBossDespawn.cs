using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBossDespawn : MonoBehaviour
{
    // despawn asteroids when no longer visible
    void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }
}
