using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBossSpawner : MonoBehaviour
{
    public GameObject asteroidBoss;
    public Transform dollyCart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 spawnRelativeToPlayer = new Vector3(0, -12.5f, 30);
            Instantiate(asteroidBoss, dollyCart, false).gameObject.transform.localPosition = spawnRelativeToPlayer;
        }
    }
}
