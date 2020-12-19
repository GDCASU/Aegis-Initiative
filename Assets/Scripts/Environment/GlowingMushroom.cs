using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingMushroom : EnvironmentHealth
{
    public GameObject healingOrbPrefab;

    // Update is called once per frame
    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Instantiate(healingOrbPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
