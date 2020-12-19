using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingMushroom : EnvironmentHealth
{
    public GameObject healingOrbPrefab;

    // Update is called once per frame
    public override void TakeDamage(int damage)
    {
        health -= damage*(2);
        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(healingOrbPrefab, transform.position, transform.rotation);
        }
    }
}
