using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General parent class for environmental objects that can be destroyed and/or
/// deal damage to the player. 
/// 
/// Some components taken from BreakableWall.cs by Matt Corey
/// </summary>
public class EnvironmentHealth : MonoBehaviour
{
    public int health;
    public int collisionDamage;
    public GameObject[] destroyReplacements;
    public bool destroyOnContact = true;
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            for(int x = 0; x < destroyReplacements.Length; x++)
            {
                if (destroyReplacements[x] != null)
                {
                    Instantiate(destroyReplacements[x], transform.position, transform.rotation);
                }
            }

            if (destroyOnContact) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) OnPlayerCollision();
    }

    public virtual void OnPlayerCollision()
    {
        PlayerHealth.singleton.TakeDamage(collisionDamage);

        TakeDamage(health);
    }
}
