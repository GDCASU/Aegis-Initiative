using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : EnvironmentHealth
{
    /* Notes:
     * The wall object with this script must also have the tag "BreakableEnvironment" and a collider
     */

    public void DestroyWall()
    {
        //Debug.Log("Wall Destroyed");
        Destroy(gameObject);
    }
    
    public override void TakeDamage(int damage)
    {
        //Debug.Log("bullet hit");
        health -= damage;
        if (health <= 0)
        {
            DestroyWall();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit player");
            PlayerHealth.singleton.TakeDamage(collisionDamage);

            DestroyWall();
        }

    }

}
