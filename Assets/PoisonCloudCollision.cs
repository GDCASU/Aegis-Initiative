using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloudCollision : MonoBehaviour
{
    //If player collides with Particle System cloud
    void OnParticleCollision(GameObject other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Deactivate Player's Active Abilities");
        }
    }
}
